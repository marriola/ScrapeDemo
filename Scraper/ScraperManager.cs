using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Scraper
{
    public class ScraperManager
    {
        private static string rootNamespace = "Scraper";
        private static ScraperManager sharedInstance = null;

        private string rootPath;
        private string assemblyListFile = "scrapers.xml";
        private List<string> scraperNames;
        private Dictionary<string, Type> externalScrapers;
        private Dictionary<string, string> paths;

        /// <summary>
        /// Exposes a list of names of loaded scrapers.
        /// </summary>
        public IEnumerable<string> ScraperNames
        {
            get
            {
                return scraperNames.AsReadOnly();
            }
        }

        private ScraperManager(string scraperDefinitionFile = null)
        {
            scraperNames = new List<string>();
            externalScrapers = new Dictionary<string, Type>();
            if (!string.IsNullOrEmpty(scraperDefinitionFile))
            {
                this.assemblyListFile = scraperDefinitionFile;
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
                scraperNames.Add(className);
                Type scraperType = assembly.GetType(rootNamespace + "." + className);
                if (scraperType == null)
                {
                    System.Windows.Forms.MessageBox.Show("Couldn't load class " + className + " from " + assemblyPath);
                }
                else
                {
                    externalScrapers[className] = scraperType;
                }
            }
        }
        /// <summary>
        /// Instantiates a scraper by name.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Scraper GetScraper(string className, object[] args)
        {
            if (!externalScrapers.ContainsKey(className))
            {
                throw new ArgumentException("Scraper class " + className + " not found");
            }

            object[] argsWithDefinitionFile = new object[args.Length + 1];
            argsWithDefinitionFile[0] = paths[className] + "scraper.xml";
            for (int i = 0; i < args.Length; i++)
            {
                argsWithDefinitionFile[i + 1] = args[i];
            }

            return (Scraper)Activator.CreateInstance(externalScrapers[className], argsWithDefinitionFile);
        }

        /// <summary>
        /// Loads scrapers from external assemblies indicated in the scraper definition file.
        /// </summary>
        private void LoadScrapers()
        {
            paths = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(assemblyListFile);

            // Load the root element.
            XmlNode root;
            XmlNodeList nodeList = doc.SelectNodes("/externalScrapers");
            if (nodeList == null)
            {
                throw new FileFormatException("Missing root node");
            }
            root = nodeList[0];

            // Get the root assembly path from the <externalScrapers> element if present.
            // Otherwise, use the current directory.
            XmlAttribute rootPathNode = root.Attributes["root"];
            rootPath = rootPathNode == null ? ".\\" : rootPathNode.Value;
            if (!rootPath.EndsWith("\\") && rootPath.EndsWith("/"))
            {
                rootPath += System.IO.Path.DirectorySeparatorChar;
            }

            // Iterate through the child <assembly> elements
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "assembly")
                {
                    ReadAssemblyInformation(rootPath, node);
                }
                else
                {
                    throw new FileFormatException("Invalid element " + node.Name);
                }
            }
        }

        /// <summary>
        /// Reads assembly information from an &lt;assembly&gt; element
        /// </summary>
        /// <param name="rootPath">The root path to prefix to assembly paths.</param>
        /// <param name="node">An &lt;assembly&gt; element.</param>
        private void ReadAssemblyInformation(string rootPath, XmlNode node)
        {
            // Get the path attribute
            XmlAttribute assemblyPath = node.Attributes["path"];
            if (assemblyPath == null || string.IsNullOrEmpty(assemblyPath.Value))
            {
                throw new FileFormatException("path attribute missing from assembly element");
            }

            XmlAttribute dllFile = node.Attributes["dll"];
            if (dllFile == null || string.IsNullOrEmpty(dllFile.Value))
            {
                throw new FileFormatException("dll attribute midding from assembly element");
            }

            List<string> scraperNames;
            ReadAssemblyClassList(rootPath + assemblyPath.Value, node, out scraperNames);
            if (scraperNames.Count == 0)
            {
                throw new FileFormatException("No classes listed for assembly " + assemblyPath.Value);
            }
            LoadExternalScraper(rootPath + assemblyPath.Value + dllFile.Value, scraperNames);
        }

        /// <summary>
        /// Reads list of classes to import from an assembly specified by an &lt;assembly&gt; element.
        /// </summary>
        /// <param name="node">An &lt;assembly&gt; node.</param>
        /// <param name="scraperNames">Holds the names of scrapers exported by the assembly.</param>
        private void ReadAssemblyClassList(string path, XmlNode node, out List<string> scraperNames)
        {
            scraperNames = new List<string>();

            // Look in the name attribute and <class> child elements for classes exported by the assembly.
            XmlAttribute className = node.Attributes["className"];
            if (className != null && !string.IsNullOrEmpty(className.Value))
            {
                // Element has a className attribute
                scraperNames.Add(className.Value);
                paths[className.Value] = path;
            }

            // Try looking for <class> child elements.
            XmlNodeList classNameNodes = node.SelectNodes("descendant::class");
            foreach (XmlNode classNameNode in classNameNodes)
            {
                if (classNameNode.Name == "class")
                {
                    className = classNameNode.Attributes["name"];
                    if (className == null || string.IsNullOrEmpty(className.Value))
                    {
                        throw new FileFormatException("Missing class name");
                    }
                    scraperNames.Add(className.Value);
                    paths[className.Value] = path;
                }
                else
                {
                    throw new FileFormatException("Invalid element " + classNameNode.Name);
                }
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
