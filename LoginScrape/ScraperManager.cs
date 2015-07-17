using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Scraper;

namespace LoginScrape
{
    class ScraperManager
    {
        private static string rootNamespace = "Scraper";
        public static ScraperManager sharedInstance = null;

        private string m_scraperDefinitionFile = "scrapers.xml";
        private List<string> m_scraperNames;
        private Dictionary<string, Type> m_externalScrapers;

        /// <summary>
        /// Exposes a list of names of loaded scrapers.
        /// </summary>
        public IEnumerable<string> ScraperNames
        {
            get
            {
                return m_scraperNames.AsReadOnly();
            }
        }

        private ScraperManager(string scraperDefinitionFile = null)
        {
            m_scraperNames = new List<string>();
            m_externalScrapers = new Dictionary<string, Type>();
            if (!string.IsNullOrEmpty(scraperDefinitionFile))
            {
                m_scraperDefinitionFile = scraperDefinitionFile;
            }
            LoadScrapers();
        }

        /// <summary>
        /// Loads a scraper from an external assembly.
        /// </summary>
        /// <param name="assemblyPath">Path to an external assembly.</param>
        /// <param name="classNames">A list of names of scraper classes exported by the assembly.</param>
        private void LoadExternalScraper(string assemblyPath, List<string> classNames)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyPath);

            foreach (string className in classNames)
            {
                m_scraperNames.Add(className);
                Type scraperType = assembly.GetType(rootNamespace + "." + className);
                if (scraperType == null)
                {
                    System.Windows.Forms.MessageBox.Show("Couldn't load class " + className + " from " + assemblyPath);
                }
                else
                {
                    m_externalScrapers[className] = scraperType;
                }
            }
        }
        /// <summary>
        /// Instantiates a scraper by name.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Scraper.Scraper GetScraper(string className, object[] args)
        {
            if (!m_externalScrapers.ContainsKey(className))
            {
                throw new ArgumentException("Scraper class " + className + " not found");
            }

            return (Scraper.Scraper)Activator.CreateInstance(m_externalScrapers[className], args);
        }

        /// <summary>
        /// Loads scrapers from external assemblies indicated in the scraper definition file.
        /// </summary>
        private void LoadScrapers()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(m_scraperDefinitionFile);

            // Load the root element.
            XmlNode root;
            XmlNodeList nodeList = doc.SelectNodes("/externalScrapers");
            if (nodeList == null)
            {
                throw new FileFormatException("Missing root node");
            }
            root = nodeList[0];

            // Get the root assembly path from the <externalScrapers> node if present.
            XmlAttribute rootPathNode = root.Attributes["root"];
            string rootPath = rootPathNode == null ? ".\\" : rootPathNode.Value;
            if (!rootPath.EndsWith("\\") && rootPath.EndsWith("/"))
            {
                rootPath += System.IO.Path.DirectorySeparatorChar;
            }

            // Iterate through the child <assembly> elements
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name != "assembly")
                {
                    throw new FileFormatException("Invalid element " + node.Name);
                }

                // Get the path attribute
                XmlAttribute assemblyPath = node.Attributes["path"];
                if (assemblyPath == null || string.IsNullOrEmpty(assemblyPath.Value))
                {
                    throw new FileFormatException("assemblyPath attribute missing from assembly element");
                }

                List<string> scraperNames = new List<string>();

                // Look in the name attribute and <class> child elements for classes exported by the assembly.
                XmlAttribute className = node.Attributes["className"];
                if (className != null && !string.IsNullOrEmpty(className.Value))
                {
                    // Element has a className attribute
                    scraperNames.Add(className.Value);
                }

                // Try looking for <class> child elements.
                XmlNodeList classNameNodes = node.SelectNodes("descendant::class");
                if (classNameNodes.Count > 0)
                {
                    foreach (XmlNode classNameNode in classNameNodes)
                    {
                        className = classNameNode.Attributes["name"];
                        if (className == null || string.IsNullOrEmpty(className.Value))
                        {
                            throw new FileFormatException("Missing class name");
                        }
                        scraperNames.Add(className.Value);
                    }
                }

                if (scraperNames.Count == 0)
                {
                    throw new FileFormatException("No classes listed for assembly " + assemblyPath.Value);
                }
                LoadExternalScraper(rootPath + assemblyPath.Value, scraperNames);
            }
        }

        /// <summary>
        /// Returns a shared instance of ScraperManager.
        /// </summary>
        /// <param name="scraperDefinitionFile">An optional alternative path to the scraper definition file.</param>
        /// <returns></returns>
        public static ScraperManager GetSharedInstance(string scraperDefinitionFile = null)
        {
            if (sharedInstance == null)
            {
                sharedInstance = new ScraperManager(scraperDefinitionFile);
            }
            return sharedInstance;
        }
    }
}
