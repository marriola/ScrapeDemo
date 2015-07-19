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
        const string HIGHLIGHT_STYLE = "background-color: #f88; color: black;";
        static readonly string[] attributes = new string[] { "id", "name", "className", "href", "type" };
        static readonly HashSet<string> uiTags = new HashSet<string>()
        {
            "A",
            "INPUT",
            "TEXTAREA",
        };
        HtmlDocument document;
        Pen elementHighlightPen;
        HtmlElement lastMouseOverElement = null;
        string lastStyle = string.Empty;

        public Form1()
        {
            InitializeComponent();
            elementHighlightPen = new Pen(new SolidBrush(Color.Blue), 2);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            webBrowser1.Width = Form1.ActiveForm.Size.Width - 42;
            webBrowser1.Height = Form1.ActiveForm.Size.Height - 80;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Update browser controls
            button3.Enabled = webBrowser1.CanGoBack;
            button2.Enabled = webBrowser1.CanGoForward;
            textBox1.Text = webBrowser1.Url.ToString();

            // Attach event handlers to each element on the page
            document = webBrowser1.Document;
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

        // Returns the tag name and values of some attributes of an HTML element.
        public string ElementInfo(HtmlElement element)
        {
            StringBuilder sb = new StringBuilder("<" + element.TagName + "> ");
            try
            {
                foreach (string attr in attributes)
                {
                    string value = element.GetAttribute(attr);
                    if (!string.IsNullOrEmpty(value))
                        sb.Append(string.Format("{0}='{1}' ", attr, value));
                }
            }
            catch (UnauthorizedAccessException)
            {
                sb.Append("#");
            }
            return sb.ToString();
        }

        // Highlights an element and displays info on it on mouseover or click.
        private void InspectElement(object sender, EventArgs e)
        {
            HtmlElement element = (HtmlElement)sender;
            HighlightElement(element);
            // show tag name and values of its attributes
            textBox2.Text = ElementInfo(element);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = checkBox1.Checked;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                webBrowser1.Navigate(textBox1.Text);
                e.SuppressKeyPress = true;
            }
        }
    }
}
