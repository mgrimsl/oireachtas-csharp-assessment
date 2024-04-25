using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
namespace OireachtasAPI
{
    public class Program
    {
        public static string LEGISLATION_DATASET = "legislation.json";
        public static string MEMBERS_DATASET = "members.json";
        private static bool API = false;
        static private IEnumerable<string> Filters = new[]
        {
            "Sponsered By",
            "Last Updated",
            "Quit"
        };
        static private IEnumerable<string> DataSource = new[]
{
            "API",
            "File",
        };
        static IDataRepo data = null;

        static async Task Main(string[] args)
        {
            List<JToken> result = new List<JToken>();
            JToken legislations;
            JToken members;

            //start Command Line Interface
            CLI cli = new CLI();
            int selection = cli.pickList(DataSource, "Data Source");
            API = selection == 0 ? true : false;
            //Load from selected data source
            if (API)
            {
                data = new DataApi();
            }
            else
            {
                data = new DataFile();
            }
            legislations = await data.getLegislationAsync();
            members = await data.getMembersAsync();

            //Run main program loop
            do
            {
                try
                {
                    selection = cli.pickList(Filters, "Filter");

                    if (selection == 0)
                    {
                        string filter = cli.askForInput("Enter a PID...");
                        result = filterBillsSponsoredBy(filter, ref legislations, ref members);
                        cli.displayResults(result);
                    }
                    else if (selection == 1)
                    {
                        DateTime since, until;
                        if (!cli.askForDates(out since, out until))
                        {
                            continue;
                        }
                        result = filterBillsByLastUpdated(since, until, ref legislations);
                        cli.displayResults(result);
                    }
                    else
                    {
                        Console.WriteLine("Goodbye!");
                        return;
                    }
                }
                catch(Exception ex)
                {
                    cli.askForInput($"Error: {ex.Message} \nPress any button to continue");
                }
            } while (true);
        }

        /// <summary>
        /// Return bills sponsored by the member with the specified pId
        /// </summary>
        /// <param name="pId">The pId value for the member</param>
        /// <returns>List of bill records</returns>
        public static List<JToken> filterBillsSponsoredBy(string pId, ref JToken legislations, ref JToken members)
        {
            string memberName = getMemberNameByPId(ref members, pId);
            return getLegilationsByMemberNameSponser(ref legislations, memberName);
        }
        /// <summary>
        /// gets member by pId
        /// </summary>
        /// <param name="members">list of members to search</param>
        /// <param name="pId">The pId value for the member</param>
        /// <returns>name of member</returns>
        public static string getMemberNameByPId(ref JToken members, string pId)
        {
            foreach (JToken result in members["results"])
            {
                if (result["member"]["pId"].Value<string>() == pId)
                {
                    return result["member"]["fullName"].Value<string>();
                }
            }
            return "";
        }
        /// <summary>
        /// gets bill by sponser name
        /// </summary>
        /// <param name="legislations">list of bills to search</param>
        /// <param name="memberName">the member name to match to the sponser</param>
        /// <returns>List of bill records</returns>
        public static List<JToken> getLegilationsByMemberNameSponser(ref JToken legislations, string memberName)
        {
            List<JToken> filteredList = new List<JToken>();
            foreach (JToken res in legislations["results"])
            {
                JToken sponsers = res["bill"]["sponsors"];
                foreach (JToken sponser in sponsers)
                {
                    if (memberName == sponser["sponsor"]["by"]["showAs"].Value<string>())
                    {
                        filteredList.Add(res["bill"]);
                    }
                }
            }
            return filteredList;
        }

        /// <summary>
        /// Return bills updated within the specified date range
        /// </summary>
        /// <param name="since">The lastUpdated value for the bill should be greater than or equal to this date</param>
        /// <param name="until">The lastUpdated value for the bill should be less than or equal to this date.If unspecified, until will default to today's date</param>
        /// <returns>List of bill records</returns>
        public static List<JToken> filterBillsByLastUpdated(DateTime since, DateTime until, ref JToken legislations)
        {
            List<JToken> filteredList = new List<JToken>();

            DateTime date = new DateTime();
            foreach (var bill in legislations["results"])
            {

                string dateStr = bill["bill"].Value<string>("lastUpdated");
                DateTime.TryParse(dateStr, out date);
                if (date.Date >= since.Date && date.Date <= until.Date)
                {
                    if(bill["bill"].Value<string>("billNo") == "101")
                    {
                        Console.Write(2);
                    }
                    filteredList.Add(bill["bill"]);
                }
            }
            return filteredList;
        }
    }
}
