using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Ideative.Core.Configuration;

namespace yawwww
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod()]
        public void LoadWrittenFile()
        {
            ConfigurationManager<TestSection> TestSectionConfig = new ConfigurationManager<TestSection>();
            var mySection = TestSectionConfig.Settings;
            mySection.myDecimal = 2.15M;
            mySection.myInt = 215;
            mySection.myString = "215";

            TestSectionConfig.Settings = mySection;

            TestSectionConfig.UpdateConfig(mySection);

        }

        [TestMethod]
        public void TestMethod1()
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(TestSection));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var mySection = new TestSection();
            mySection.myDecimal = 2.15M;
            mySection.myInt = 215;
            mySection.myString = "215";
            using (StringWriter sww = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sww, new XmlWriterSettings() { OmitXmlDeclaration = true }))
            {
                xsSubmit.Serialize(writer, mySection, ns);
                var xml = sww.ToString();
            }

            // Save Xml            
            string configutaionFilePath = Environment.CurrentDirectory;
            var Xml = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                        <configuration>
                              <configSections>
                              </configSections>
                        </configuration>
                        ";

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(Xml);
            xdoc.Save(configutaionFilePath + @"\config.xml");
        }
    }

    public class TestSection : ConfigurationSectionHandler
    {
        public string myString { get; set; }
        public int myInt { get; set; }
        public decimal myDecimal { get; set; }
    }
}
