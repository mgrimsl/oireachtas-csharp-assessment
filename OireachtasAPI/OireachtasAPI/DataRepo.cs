using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net;
using System.Configuration;

namespace OireachtasAPI
{
    //Class for all data calls
    public class DataFile : IDataRepo
    {
        public async Task<JToken> getLegislationAsync()
        {
            return JsonConvert.DeserializeObject<JToken>(
                new System.IO.StreamReader(ConfigurationManager.ConnectionStrings["Legislation"].ConnectionString)
                .ReadToEnd()
            );
        }

        public async Task<JToken> getMembersAsync()
        {
            using (var httpClient = new HttpClient())
            {
                return JsonConvert.DeserializeObject<JToken>(
                    new System.IO.StreamReader(ConfigurationManager.ConnectionStrings["Member"].ConnectionString)
                    .ReadToEnd()
                );
            }
        }

    }  
    public class DataApi : IDataRepo
    {
        public async Task<JToken> getLegislationAsync()
        {
            using (var httpClient = new HttpClient())
            {
                string json = "";
                json = await httpClient.GetStringAsync(ConfigurationManager.ConnectionStrings["LegislationAPI"].ConnectionString);
                return JObject.Parse(json);
            }
        }

        public async Task<JToken> getMembersAsync()
        {
            using (var httpClient = new HttpClient())
            {
                string json = "";
                json = await httpClient.GetStringAsync(ConfigurationManager.ConnectionStrings["MemberAPI"].ConnectionString);
                return JObject.Parse(json);
            }
        }
    }
}
