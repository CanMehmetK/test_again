using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Ideative.Core.Configuration
{
    // https://github.com/tmsmith/Dapper-Extensions
    // https://github.com/twistedtwig/CustomConfigurations
    // http://www.codeproject.com/Articles/576286/MVC-Basic-Site-Step-3-Dynamic-Layouts-and-Site-Adm
    public class ConfigurationManager<T> : IConfiguration<T> where T : ConfigurationSectionHandler
    {
        public T Settings { get; set; } //<- Private Set for only access for updatevalues
        string configutaionFilePath;
        public ConfigurationManager(string configutaionFilePath = null)
        {
            // TODO: if null Generate Default Configuration
            if (configutaionFilePath == null)
            {
                this.configutaionFilePath = Environment.CurrentDirectory;
                generateDefaultConfigurationFile();
                UpdateConfig();
            }
            else
            {
                this.configutaionFilePath = configutaionFilePath;
                LoadConfig();
            }
        }

        private void generateDefaultConfigurationFile()
        {
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

        public void LoadConfig()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            System.Configuration.Configuration config;
            fileMap.ExeConfigFilename = configutaionFilePath;
            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            ConfigurationSection section = config.GetSection(typeof(T).ToString().Split('.').Last());
            string xml = section.SectionInformation.GetRawXml();
            //string type = section.SectionInformation.Type;
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            Settings = deserializer.Deserialize(XmlReader.Create(new StringReader(xml))) as T;
        }

        public void UpdateConfig(T newValues = null)
        {
            if (newValues != null)
                Settings = newValues;

            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            
            using (StringWriter sww = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sww, new XmlWriterSettings() { OmitXmlDeclaration = true }))
            {
                xsSubmit.Serialize(writer, Settings, ns);
                var values = sww.ToString();
                var sectionName = typeof(T).ToString().Split('.').Last();

                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                System.Configuration.Configuration config;
                fileMap.ExeConfigFilename = configutaionFilePath;
                config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);                                
                
            }            
            LoadConfig();
        }
    }
}
