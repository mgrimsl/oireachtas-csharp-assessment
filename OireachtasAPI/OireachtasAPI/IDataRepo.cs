using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OireachtasAPI
{
    public interface IDataRepo
    {
        Task<JToken> getMembersAsync();
        Task<JToken> getLegislationAsync();
    }
}
