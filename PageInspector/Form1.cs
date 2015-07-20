﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ScraperDesigner
{
    public partial class DesignerWindow : Form
    {
        internal List<ScraperStep> steps;

        const string SCRAPER_FILTER = "Scraper definitions (*.xml)|*.xml|All files|*";
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

        StepsForm stepsDialog;

        public DesignerWindow()
        {
            InitializeComponent();
            steps = new List<ScraperStep>();
            stepsDialog = new StepsForm(this);
            stepsDialog.Show();
        }

        private void ResizeControls()
        {
            webBrowser1.Width = this.Size.Width - 32;
            webBrowser1.Height = this.Size.Height - 160;
            txtAddress.Width = this.Size.Width - 150;
            txtElementPath.Width = this.Size.Width - 90;
            txtElementAttributes.Width = this.Size.Width - 90;
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
            this.Text = windowTitle + " - " + document.Title;
            btnBack.Enabled = webBrowser1.CanGoBack;
            btnForward.Enabled = webBrowser1.CanGoForward;
            txtAddress.Text = webBrowser1.Url.ToString();

            // Attach event handlers to each element on the page
            foreach (HtmlElement element in document.All)
            {
                element.GotFocus += ElementClicked;
                element.MouseEnter += InspectElement;
            }
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

        // Apply the highlight style to an element.
        internal void HighlightElement(HtmlElement element)
        {
            // Unhighlight last control moused over
            if (lastMouseOverElement != null)
                lastMouseOverElement.Style = lastStyle;

            lastMouseOverElement = element;
            lastStyle = element.Style;
            element.Style += HIGHLIGHT_STYLE;
        }

        private void ElementClicked(object sender, EventArgs e)
        {
            HtmlElement element = lastMouseOverElement; //(HtmlElement)sender;
            System.Diagnostics.Debug.WriteLine(ElementInfo(element) + " // " + ElementInfo(lastMouseOverElement));
            if (!chkSelectionMode.Checked || steps.Count == 0 || element != lastMouseOverElement)
                return;

            HighlightElement(element);

            // Prompt for a name for the new element
            string elementName;
            var elementNameDialog = new ElementNameDialog();
            elementNameDialog.txtElementInfo.Text = ElementInfo(element);
            if (elementNameDialog.ShowDialog() == DialogResult.OK)
            {
                elementName = elementNameDialog.txtElementName.Text;
            }
            else
            {
                return;
            }

            // Add to listbox and elements collection.
            int elementsIndex = stepsDialog.lstElements.SelectedIndex;
            stepsDialog.lstElements.Items.Insert(elementsIndex + 1, elementName);
            stepsDialog.lstElements.SelectedIndex = elementsIndex + 1;

            // Add the element to our list as well.
            Selector selector = new Selector()
                {
                    Tag = element.TagName,
                    ClientId = element.GetAttribute("id"),
                    Name = element.GetAttribute("name"),
                    ClassName = element.GetAttribute("className")
                };
            var definition = new ElementDefinition()
            {
                Element = element,
                Id = elementName,
                ElementSelector = selector
            };
            int stepsIndex = stepsDialog.lstSteps.SelectedIndex;
            steps[stepsIndex].Elements.Insert(elementsIndex + 1, definition);
        }
        
        // Highlights an element and displays info on it on mouseover or click.
        private void InspectElement(object sender, EventArgs e)
        {
            if (!chkSelectionMode.Checked)
                return;

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

        private void chkSelectionMode_CheckedChanged(object sender, EventArgs e)
        {
            // Unhighlight the currently highlighted element, if any, when the "Selection mode"
            // checkbox is unchecked.
            if (!chkSelectionMode.Checked && lastMouseOverElement != null)
            {
                lastMouseOverElement.Style = lastStyle;
                lastStyle = "";
                lastMouseOverElement = null;
            }
        }

        private void SaveScraper()
        {
            var dlgSave = new SaveFileDialog()
            {
                Filter = SCRAPER_FILTER
            };

            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                ScraperDefinitionWriter.Write(dlgSave.FileName, steps);
            }
            dlgSave.Dispose();
        }

        private void LoadScraper()
        {
            var dlgOpen = new OpenFileDialog()
            {
                Filter = SCRAPER_FILTER
            };

            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                Scraper.ScraperDefinitionReader reader = new Scraper.ScraperDefinitionReader(dlgOpen.FileName);
                reader.ReadScraperDefinition();
                steps.Clear();
                //foreach (var dic in reader.n
            }
            dlgOpen.Dispose();
        }
            
        private void itmSave_Click(object sender, EventArgs e)
        {
            SaveScraper();
        }

        private void itmOpen_Click(object sender, EventArgs e)
        {
            LoadScraper();
        }
    }
}
