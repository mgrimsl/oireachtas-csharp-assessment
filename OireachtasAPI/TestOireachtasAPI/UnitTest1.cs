using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OireachtasAPI;
using System;
using System.Collections.Generic;
using System.IO;

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
        static IDataRepo dataApi = new DataApi();
        static IDataRepo dataFile = new DataFile();

        [TestMethod]
        public void TestSponsor()
        {
            JToken leg = dataFile.getLegislationAsync().Result;
            JToken mem = dataFile.getMembersAsync().Result;
            List<JToken> results = Program.filterBillsSponsoredBy("IvanaBacik", ref leg, ref mem);
            Assert.IsTrue(results.Count >= 2);
        }      
        [TestMethod]
        public void TestSponsorAPI()
        {
            JToken leg = dataApi.getLegislationAsync().Result;
            JToken mem = dataApi.getMembersAsync().Result;
            List<JToken> results = Program.filterBillsSponsoredBy("IvanaBacik", ref leg, ref mem);
            Assert.IsTrue(results.Count >= 1);
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
                   "141","91","111","55","129","103","131","135","101","77","138","139","106","134","133","58","132"
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
