using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

namespace ConvertJsonToXml
{
    class JsonXmlConverter
    {
        public void ConvertJsonFileToXmlFile(string jsonFilePath, string xmlFilePath)
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            string xmlString = ConvertJsonToXml(jsonString);
            File.WriteAllText(xmlFilePath, xmlString);
        }

        public string ConvertJsonToXml(string jsonString)
        {
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;
                XDocument xmlDocument = new XDocument(ConvertJsonElementToXmlElement(root));
                return xmlDocument.ToString();
            }
        }

        public string ConvertJsonFileToXml(string jsonFilePath)
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            return ConvertJsonToXml(jsonString);
        }

        private XElement ConvertJsonElementToXmlElement(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                var objElement = new XElement("object");
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    objElement.Add(new XElement(property.Name, ConvertJsonElementToXmlElement(property.Value)));
                }
                return objElement;
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                var arrayElement = new XElement("array");
                foreach (JsonElement arrayItem in element.EnumerateArray())
                {
                    arrayElement.Add(ConvertJsonElementToXmlElement(arrayItem));
                }
                return arrayElement;
            }
            else
            {
                return new XElement("value", element.ToString());
            }
        }
    }

    internal class Program
    {
        static void Main()
        {
            string jsonFilePath = "response.json";
            string xmlFilePath = "output.xml";

            JsonXmlConverter converter = new JsonXmlConverter();
            converter.ConvertJsonFileToXmlFile(jsonFilePath, xmlFilePath);

            Console.WriteLine("Conversion completed. XML file has been created.");
        }
    }
}
