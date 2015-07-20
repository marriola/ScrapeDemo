using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using ScraperDesigner;

namespace Scraper
{
    public class ScraperDefinitionReader
    {
        public List<Dictionary<string, ScraperDesigner.Selector>> namedElements;
        string definitionFile;

        public ScraperDefinitionReader(string definitionFile)
        {
            this.definitionFile = definitionFile;
        }

        public void ReadScraperDefinition()
        {
            namedElements = new List<Dictionary<string, ScraperDesigner.Selector>>();
            XmlDocument doc = new XmlDocument();
            doc.Load(definitionFile);

            XmlNodeList steps = doc.SelectNodes("/scraper/step");
            foreach (XmlNode step in steps)
            {
                ReadStep(step);
            }
        }

        /// <summary>
        /// Reads page elements from a step element into the element dictionary.
        /// </summary>
        /// <param name="step"></param>
        public void ReadStep(XmlNode step)
        {
            var dic = new Dictionary<string, Selector>();
            XmlNodeList elements = step.SelectNodes("descendant::element");
            foreach (XmlNode element in elements)
            {
                XmlAttribute idAttr = element.Attributes["id"];
                XmlAttribute optionalAttr = element.Attributes["optional"];
                XmlAttribute tagAttr = element.Attributes["tag"];
                XmlAttribute clientIdAttr = element.Attributes["client-id"];
                XmlAttribute nameAttr = element.Attributes["name"];
                XmlAttribute classNameAttr = element.Attributes["class"];

                string id = idAttr == null ? "" : idAttr.Value;
                string tag = tagAttr == null ? "" : tagAttr.Value;
                string clientId = clientIdAttr == null ? "" : clientIdAttr.Value;
                string name = nameAttr == null ? "" : nameAttr.Value;
                string className = classNameAttr == null ? "" : classNameAttr.Value;

                bool optional = false;
                if (optionalAttr != null)
                {
                    optional = optionalAttr.Value == "true" ? true : false;
                }

                if (string.IsNullOrEmpty(id))
                {
                    throw new FileFormatException("Unnamed element");
                }
                dic[id] = new Selector
                {
                    Tag = tag,
                    ClientId = clientId,
                    Name = name,
                    ClassName = className,
                    Optional = optional
                };
            }
            namedElements.Add(dic);
        }
    }
}
