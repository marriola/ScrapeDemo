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

            var values = new Tuple<string, string>[]
                {
                    Tuple.Create("tag", element.ElementSelector.Tag),
                    Tuple.Create("client-id", element.ElementSelector.ClientId),
                    Tuple.Create("name", element.ElementSelector.Name),
                    Tuple.Create("class", element.ElementSelector.ClassName)
                };

            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value.Item2))
                {
                    writer.WriteAttributeString(value.Item1, value.Item2);
                }
            }
            writer.WriteEndElement();
        }


    }
}
