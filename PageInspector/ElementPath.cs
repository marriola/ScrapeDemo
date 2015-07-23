using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperDesigner
{
    public class ElementPath
    {
        public List<Selector> path;

        public ElementPath()
        {
            path = new List<Selector>();
        }

        public ElementPath(ElementPath existingPath)
        {
            path = new List<Selector>(existingPath.path);
        }

        public static ElementPath FromString(string pathString)
        {
            ElementPath path = new ElementPath();
            StringBuilder thisNode = new StringBuilder();
            foreach (char c in pathString)
            {
                if (c == '/')
                {
                    path.path.Add(Selector.FromString(thisNode.ToString()));
                    thisNode.Clear();
                    continue;
                }

                pathString = pathString.Substring(1);
                thisNode.Append(c);
            }

            if (thisNode.Length > 0)
            {
                path.path.Add(Selector.FromString(thisNode.ToString()));
            }
            return path;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Selector selector in path.Reverse<Selector>())
            {
                sb.Append(selector.ToString(false));
                sb.Append('/');
            }
            return sb.ToString();
        }
    }
}
