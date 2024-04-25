using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OireachtasAPI
{
    internal static class Data
    {
        public static Func<string, Task<JToken>> loadFromEndPoint = async endPoint =>
        {

            using (var httpClient = new HttpClient())
            {
                string json = "";
                json = await httpClient.GetStringAsync(endPoint);
                return JObject.Parse(json);
            }
        };

        public static Func<string, JToken> load = jfname => JsonConvert.DeserializeObject<JToken>((new System.IO.StreamReader(jfname)).ReadToEnd());
    }
}
