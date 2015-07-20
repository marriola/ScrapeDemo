using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiteInspector
{
    public partial class Form1 : Form
    {
        const string HIGHLIGHT_STYLE = "background-color: orangered; color: black;";
        static readonly string[] identifyingAttributes = new string[3] { "id", "name", "class" };
        static readonly string[] attributes = new string[] { "id", "name", "className", "href", "type" };
        static readonly HashSet<string> uiTags = new HashSet<string>()
        {
            "A",
            "INPUT",
            "TEXTAREA",
        };

        const string windowTitle = "Page inspector";

        HtmlDocument document;
        HtmlElement lastMouseOverElement = null;
        string lastStyle = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void ResizeControls()
        {
            webBrowser1.Width = Form1.ActiveForm.Size.Width - 32;
            webBrowser1.Height = Form1.ActiveForm.Size.Height - 160;
            txtAddress.Width = Form1.ActiveForm.Size.Width - 150;
            txtElementPath.Width = Form1.ActiveForm.Size.Width - 90;
            txtElementAttributes.Width = Form1.ActiveForm.Size.Width - 90;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            ResizeControls();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResizeControls();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            document = webBrowser1.Document;

            // Update browser controls
            Form1.ActiveForm.Text = windowTitle + " - " + document.Title;
            btnBack.Enabled = webBrowser1.CanGoBack;
            btnForward.Enabled = webBrowser1.CanGoForward;
            txtAddress.Text = webBrowser1.Url.ToString();

            // Attach event handlers to each element on the page
            foreach (HtmlElement element in document.All)
            {
                element.GotFocus += InspectElement;
                element.MouseEnter += InspectElement;
            }
        }

        // Apply the highlight style to an element.
        private void HighlightElement(HtmlElement element)
        {
            // Unhighlight last control moused over
            if (lastMouseOverElement != null)
                lastMouseOverElement.Style = lastStyle;

            lastMouseOverElement = element;
            lastStyle = element.Style;
            element.Style += HIGHLIGHT_STYLE;
        }

        /// <summary>
        /// Returns the path from the root element to the indicated element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public string ElementPath(HtmlElement element)
        {
            StringBuilder sb = new StringBuilder();
            HtmlElement ancestor = element.Parent;
            string path = "";
            bool firstElement = true;

            while (ancestor != null)
            {
                // The display name of the control. If the element's Name property is blank, try
                // its "name" attribute instead. If that's blank, try the CSS class name.
                string elementIdentifier = string.Empty;
                foreach (string attr in identifyingAttributes)
                {
                    string value = ancestor.GetAttribute(attr);
                    if (!string.IsNullOrEmpty(value))
                    {
                        elementIdentifier = string.Format("{0}={1} ", attr, value);
                        break;
                    }
                }

                string thisElement = string.Format("{0} {1}", ancestor.TagName, elementIdentifier);

                if (firstElement)
                    firstElement = false;
                else
                    thisElement += "-> ";

                ancestor = ancestor.Parent;
                path = thisElement + path;
            }
            sb.Append(path);
            return sb.ToString();
        }

        // Returns the tag name and values of some attributes of an HTML element.
        public string ElementInfo(HtmlElement element)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<" + element.TagName + "> ");

                foreach (string attr in attributes)
                {
                    string value = element.GetAttribute(attr);
                    if (!string.IsNullOrEmpty(value))
                        sb.Append(string.Format("{0}='{1}' ", attr, value));
                }
            }
            catch (UnauthorizedAccessException)
            {
                sb.Append("<<unauthorized access exception>>");
            }
            return sb.ToString();
        }

        // Highlights an element and displays info on it on mouseover or click.
        private void InspectElement(object sender, EventArgs e)
        {
            HtmlElement element = (HtmlElement)sender;
            HighlightElement(element);

            // show tag name and values of its attributes
            txtElementPath.Text = ElementPath(element);
            txtElementAttributes.Text = ElementInfo(element);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = chkSuppressScriptErrors.Checked;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                webBrowser1.Navigate(txtAddress.Text);
                e.SuppressKeyPress = true;
            }
        }

        private void chkHover_CheckedChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(chkHoverDisplay.Checked);
            // Attach event handlers to each element on the page
            if (document != null)
            {
                foreach (HtmlElement element in document.All)
                {
                    if (chkHoverDisplay.Checked)
                        element.MouseEnter += InspectElement;
                    else
                        element.MouseEnter -= InspectElement;
                }
            }
        }
    }
}
