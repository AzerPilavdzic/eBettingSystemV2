using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Models;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezultatiImporter.Services
{
    public class ApiService
    {
        private static string _resource = "CacheLearn/";
        public static string _endpoint = "https://localhost:44318/";
        public static string _Command = "TakeDataFromCache";


        public ApiService(/*string resource*/)
        {
            //_resource = resource;
        }

        public static async Task Post<T>(List<T> request)
        {
           


            try
            {
                List<PodaciSaStranice> result = await $"{_endpoint}{_resource}{_Command}".PostJsonAsync(request).ReceiveJson<List<PodaciSaStranice>>();
                Console.WriteLine(result[0].Competitionname.ToString());
                //return result;
            }
            catch (FlurlHttpException ex)
            {
                var errors = await ex.GetResponseJsonAsync<Dictionary<string, string[]>>();

                var stringBuilder = new StringBuilder();
                foreach (var error in errors)
                {
                    stringBuilder.AppendLine($"{error.Key}, ${string.Join(",", error.Value)}");
                }

                Console.WriteLine("Greška");


                //return default(T);
            }

        }








    }
}
