using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperDesigner
{
    public struct ElementDefinition
    {
        public System.Windows.Forms.HtmlElement Element;
        public string Id;
        public Selector ElementSelector;
    }
}
