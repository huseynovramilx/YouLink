using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LinkShortener.Common
{
    public class PaymentMethods
    {
        private JObject jO;
        public PaymentMethods(string filePath)
        {
            try
            {
                jO = JObject.Parse(File.ReadAllText(filePath));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<string> GetAllMethods()
        {
            var methods = from method in jO["methods"]
                          select (string)method["name"];
            return methods.ToList();
        }

        public List<string> GetRecipientTypes(string methodName)
        {
            var recTypes = from method in jO["methods"]
                           where method["name"].ToString() == methodName
                           select method["recipientTypes"].Values<string>();
            return recTypes.First().ToList();
                           
        }
    }
}
