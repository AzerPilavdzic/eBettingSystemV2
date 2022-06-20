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
        private string _resource = null;
        public string _endpoint = "https://localhost:7192/";


        public ApiService(string resource)
        {
            _resource = resource;
        }

        public async Task<T> Post<T>(object request)
        {
            try
            {
                var result = await $"{_endpoint}{_resource}".PostJsonAsync(request).ReceiveJson<T>();
                return result;
            }
            catch (FlurlHttpException ex)
            {
                var errors = await ex.GetResponseJsonAsync<Dictionary<string, string[]>>();

                var stringBuilder = new StringBuilder();
                foreach (var error in errors)
                {
                    stringBuilder.AppendLine($"{error.Key}, ${string.Join(",", error.Value)}");
                }

                //MessageBox.Show(stringBuilder.ToString(), "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default(T);
            }

        }


    }
}
