using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScraperDesigner
{
    public struct Selector
    {
        private static readonly Regex reSelector = new Regex(@"(?<tag>\w*)(\s*(?<attributes>.*))?");
        private static readonly Regex reAttributes = new Regex(@"(?<name>[^ '""<>/=]+)=(?<value>\S+)\s*");

        private static readonly string[] standardAttributes = { "id", "name", "className", "href" };

        private Dictionary<string, string> _attributes;
        private System.Windows.Forms.HtmlElement _element;
        private ElementPath _path;
        private string _tag;
        private bool _optional;

        public ReadOnlyDictionary<string, string> Attributes
        {
            get
            {
                return new ReadOnlyDictionary<string, string>(_attributes);
            }
        }

        public System.Windows.Forms.HtmlElement Element
        {
            get
            {
                return _element;
            }
        }

        public ElementPath Path
        {
            get
            {
                return _path;
            }
        }

        public string Tag
        {
            get
            {
                return _tag;
            }
        }
        public bool Optional
        {
            get
            {
                return _optional;
            }
        }

        /// <summary>
        /// Create a selector by supplying fields manually.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="id"></param>
        public Selector(string tag, string id, bool optional, string path, Dictionary<string, string> attributes)
        {
            _element = null;
            _optional = optional;
            _tag = tag;
            _path = string.IsNullOrEmpty(path) ? null : ElementPath.FromString(path);
            _attributes = new Dictionary<string, string>(attributes);
            _attributes["id"] = id;
        }

        /// <summary>
        /// Create a selector from an HTML element.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="optional"></param>
        public Selector(System.Windows.Forms.HtmlElement element, bool optional)
        {
            _attributes = new Dictionary<string, string>();
            foreach (string attr in standardAttributes)
            {
                _attributes[attr] = element.GetAttribute(attr);
            }

            _element = element;
            _path = DesignerWindow.BuildPath(element);
            _tag = element.TagName;
            _optional = optional;
        }

        public bool Match(System.Windows.Forms.HtmlElement element)
        {
            bool hasAll = true;
            bool hasAny = false;

            foreach (string attr in standardAttributes)
            {
                bool hasField = element.GetAttribute(attr) == _attributes[attr];
                hasAll &= hasField;
                hasAny |= hasField;
            }

            bool tagsMatch = !string.IsNullOrEmpty(_tag) && _tag == element.TagName;

            return hasAll || (tagsMatch && hasAny);
        }

        public static Selector FromString(string selectorString)
        {
            string tag;
            string id = string.Empty;
            string attributesString;
            bool optional = true;
            var attributes = new Dictionary<string, string>();

            Match selectorMatch = reSelector.Match(selectorString);
            tag = selectorMatch.Groups["tag"].Value;
            attributesString = selectorMatch.Groups["attributes"].Value;

            MatchCollection attributesMatches = reAttributes.Matches(attributesString);
            foreach (Match attributesMatch in attributesMatches)
            {
                string name = attributesMatch.Groups["name"].Value;
                string value = attributesMatch.Groups["value"].Value;
                attributes[name] = value;
            }

            if (attributes.ContainsKey("optional"))
            {
                optional = Convert.ToBoolean(attributes["optional"]);
                attributes.Remove("optional");
            }

            if (attributes.ContainsKey("id"))
            {
                id = attributes["id"];
                attributes.Remove("id");
            }

            return new Selector(tag, id, optional, null, attributes);
        }

        public string ToString(bool showPath = true)
        {
            var attrValues = new List<string>();
            foreach (var key in standardAttributes)
            {
                if (_attributes.ContainsKey(key) && !string.IsNullOrEmpty(_attributes[key]))
                {
                    attrValues.Add(string.Format("{0}={1}", key, _attributes[key]));
                }
            }

            string pathString = string.Empty;
            if (showPath)
            {
                pathString = string.Format(" path={0}", _path.ToString());
            }

            return string.Format("{0}{1} optional={2} {3}", _tag, pathString, _optional.ToString(), string.Join(" ", attrValues));
        }
    }
}
