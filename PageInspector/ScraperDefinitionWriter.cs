using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ScraperDesigner
{
    class ScraperDefinitionWriter
    {
        public static void Write(string filename, List<ScraperStep> steps)
        {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true
            };
            XmlWriter writer = XmlWriter.Create(filename, settings);

            writer.WriteStartElement("scraper");
            foreach (var step in steps)
            {
                writer.WriteStartElement("step");
                writer.WriteAttributeString("url", step.Url);
                foreach (var element in step.Elements)
                {
                    WriteElement(writer, element);
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();
        }

        private static void WriteElement(XmlWriter writer, ElementDefinition element)
        {
            writer.WriteStartElement("element");
            writer.WriteAttributeString("id", element.Id);
            writer.WriteAttributeString("path", element.ElementSelector.Path.ToString());
            writer.WriteAttributeString("optional", element.ElementSelector.Optional.ToString());

            foreach (KeyValuePair<string, string> pair in element.ElementSelector.Attributes)
            {
                if (!string.IsNullOrEmpty(pair.Value))
                {
                    string key = pair.Key;
                    if (key == "id")
                        key = "client-id";
                    else if (key == "class")
                        key = "className";

                    writer.WriteAttributeString(key, pair.Value);
                }
            }
            writer.WriteEndElement();
        }


    }
}
