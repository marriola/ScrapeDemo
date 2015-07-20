using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperDesigner
{
    public struct Selector
    {
        public string Tag;
        public string Name;
        public string ClientId;
        public string ClassName;
    }

    public struct ElementDefinition
    {
        public System.Windows.Forms.HtmlElement Element;
        public string Id;
        public Selector ElementSelector;
    }

    public class ScraperStep
    {
        public string Url;
        public List<ElementDefinition> Elements;

        public ScraperStep(string url)
        {
            Url = url;
            Elements = new List<ElementDefinition>();
        }
    }
}
