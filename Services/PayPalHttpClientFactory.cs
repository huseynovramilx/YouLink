using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LinkShortener.Areas.Admin.Models;
using LinkShortener.Models;
using Newtonsoft.Json;

namespace LinkShortener.Models {
    public class PayPalHttpClientFactory {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly bool _isLive;

        public PayPalHttpClientFactory (string clientId, string clientSecret, bool isLive) {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _isLive = isLive;
        }

        public PayPalHttpClient GetClient () {
            if (_isLive) {
                // Live Environment
                return new PayPalHttpClient (_clientId, _clientSecret, new LiveEnvironment ());
            }

            // Sandbox Environment
            return new PayPalHttpClient (_clientId, _clientSecret, new SandboxEnvironment ());
        }
    }

    public class PayPalHttpClient : HttpClient {
        private string _clientId;
        private string _clientSecret;

        private AuthorizationResponse _authResponse;
        public Environment Environment { get; private set; }
        public PayPalHttpClient (string clientId, string clientSecret, Environment environment) {
            _clientId = clientId;
            _clientSecret = clientSecret;
            Environment = environment;
            BaseAddress = new Uri (environment.BaseAddress);
        }

        public async Task AuthorizeAsync () {
            HttpRequestMessage message = new HttpRequestMessage ();
            message.Method = HttpMethod.Post;
            message.Headers.Add ("Authorization","Basic "+ Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}",_clientId, _clientSecret ))));
            message.RequestUri = new Uri(this.BaseAddress+"oauth2/token");
            message.Content = new StringContent("grant_type=client_credentials",Encoding.UTF8,
                "application/x-www-form-urlencoded"); //CONTENT-TYPE header
            HttpResponseMessage response = await this.SendAsync(message);
            _authResponse =(AuthorizationResponse)response.Content.ReadAsAsync(typeof(AuthorizationResponse)).Result;
        }

        public async Task CreatePayouts(PayoutBatch payoutBatch) {
            HttpRequestMessage message = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(BaseAddress + "payments/payouts")
            };
            message.Headers.Add("Authorization", "Bearer " + _authResponse.access_token);
            var obj = new
            {
                sender_batch_header = new
                {
                    sender_batch_id=payoutBatch.ID,
                    email_subject = payoutBatch.EmailSubject,
                    email_message = payoutBatch.EmailMessage
                },
                items = payoutBatch.Payouts.Select(p => new
                {
                    recipient_type = p.RecipientType,
                    amount = new
                    {
                        value = p.Money,
                        currency = p.Currency
                    },
                    note = "Thank you",
                    sender_item_id = p.ID,
                    receiver = p.Receiver
                }).ToList()
            };
            string objJson = JsonConvert.SerializeObject(obj);
            message.Content = new StringContent(objJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await SendAsync(message);
            CreatePayoutsResponse payoutsResponse = (CreatePayoutsResponse)response.Content.ReadAsAsync(typeof(CreatePayoutsResponse)).Result;
            payoutBatch.PayoutPaypalBatchId = payoutsResponse.batch_header.payout_batch_id;
            payoutBatch.Status = payoutsResponse.batch_header.batch_status;
            Console.WriteLine(JsonConvert.SerializeObject(payoutsResponse));
         } 

        private class AuthorizationResponse{
            public string scope{get; set;}
            public string nonce{get; set;}
            public string access_token{get; set;}
            public string token_type{get ;set;}
            public string app_id{get ;set;}
            public string expires_in{get;set;}
        }
        private class CreatePayoutsResponse
        {
            
            public BatchHeader batch_header { get; set; }
            public List<Link> links { get; set; }
            public class Link
            {
                public string href { get; set; }
                public string rel { get; set; }
                public string method { get; set; }
                public string encType { get; set; }
            }
            public class BatchHeader
            {
                public string payout_batch_id { get; set; }
                public string batch_status { get; set; }

                public class SenderBatchHeader
                {
                    public string sender_batch_id { get; set; }
                    public string email_subject { get; set; }
                }
            }
        }

    }

    public abstract class Environment {
        public string BaseAddress { get; protected set; }
        public Environment () {

        }
    }

    public class LiveEnvironment : Environment {
        public LiveEnvironment () {
            BaseAddress = "https://api.paypal.com/v1/";
        }
    }

    public class SandboxEnvironment : Environment {
        public SandboxEnvironment () {

            BaseAddress = "https://api.sandbox.paypal.com/v1/";
        }
    }
}