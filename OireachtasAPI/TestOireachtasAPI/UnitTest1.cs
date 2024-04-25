using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OireachtasAPI;

namespace TestOireachtasAPI
{
    [TestClass]
    public class LoadDatasetTest
    {
        dynamic expected;
        IDataRepo api;
        IDataRepo file;
        
        [TestInitialize]
        public void SetUp()
        {
            api = new DataApi();
            file = new DataFile();
            using (StreamReader r = new StreamReader(OireachtasAPI.Program.MEMBERS_DATASET))
            {
                string json = r.ReadToEnd();
                expected = JsonConvert.DeserializeObject(json);
            }
        }
        [TestMethod]
        public void TestLoadFromFile()
        {
            dynamic loaded = file.getMembersAsync().Result;
            Assert.AreEqual(loaded["results"].Count, expected["results"].Count);

        }

        [TestMethod]
        public void TestLoadFromUrl()
        {
            dynamic loaded = api.getMembersAsync().Result;
            Assert.AreEqual(loaded["results"].Count, expected["results"].Count);

        }
    }
    [TestClass]
    public class FilterBillsSponsoredByTest
    {
        static IDataRepo data = new DataFile();
        JToken leg = data.getLegislationAsync().Result;
        JToken mem = data.getMembersAsync().Result;

        [TestMethod]
        public void TestSponsor()
        {
            List<JToken> results = Program.filterBillsSponsoredBy("IvanaBacik",ref leg, ref mem);
            Assert.IsTrue(results.Count>=2);
        }
    }

    [TestClass]
    public class FilterBillsByLastUpdatedTest
    {
        static IDataRepo data = new DataFile();
        JToken leg = data.getLegislationAsync().Result;

        [TestMethod]
        public void Testlastupdated()
        {
            List<string> expected = new List<string>(){
                "77", "58", "141", "55", "94", "133", "132", "131",
                "111", "135", "134", "91", "129", "103", "138", "106", "139"
            };
            List<string> received = new List<string>();

            DateTime since = new DateTime(2018, 12, 1);
            DateTime until = new DateTime(2019, 1, 1);

            foreach (dynamic bill in Program.filterBillsByLastUpdated(since, until, ref leg))
            {
                string str = bill["billNo"];
                received.Add(str);
            }
            CollectionAssert.AreEqual(expected, received);
        }
    }
}
