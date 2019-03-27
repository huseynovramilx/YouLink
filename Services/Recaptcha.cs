using LinkShortener.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace LinkShortener.Services
{

    public class RecaptchaHttpClient
    {
        private readonly RecaptchaOptions _recaptchaOptions;

        private readonly HttpClient _client;

        public RecaptchaHttpClient(HttpClient client, IOptions<RecaptchaOptions> recaptchaOptions)
        {
            _client = client;
            _recaptchaOptions = recaptchaOptions.Value;
        }

        [DataContract(Name = "")]
        public class RecaptchaResponse
        {
            [DataMember(Name ="success")]
            public bool Success { get; set; }

            [DataMember(Name = "challenge_ts")]
            public string Timestamp { get; set; }

            [DataMember(Name = "hostname")]
            public string Hostname { get; set; }
            

        }

        public async Task<bool> ValidateToken(string token)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, _recaptchaOptions.Url);

            var parameters = new Dictionary<string, string>()
            {
                { "secret", _recaptchaOptions.SecretKey },
                { "response", token }
            };
            message.Content = new FormUrlEncodedContent(parameters);

            
            HttpResponseMessage response = await _client.SendAsync(message);

            RecaptchaResponse responseData = await response.Content.ReadAsAsync<RecaptchaResponse>();

            return responseData.Success;
        }
    }
    
}
