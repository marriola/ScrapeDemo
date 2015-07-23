using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScraperDesigner
{
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