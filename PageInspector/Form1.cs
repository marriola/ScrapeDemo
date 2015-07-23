using System;
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
        static readonly string[] identifyingAttributes = new string[] { "id", "name", "href", "class" };
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
        bool selectionComplete = true;

        StepsForm stepsDialog;

        public DesignerWindow()
        {
            InitializeComponent();
            steps = new List<ScraperStep>();
            stepsDialog = new StepsForm(this);
            stepsDialog.Show();
            webBrowser1.AllowNavigation = !chkSelectionMode.Checked;
        }

        internal HtmlElement FindBySelector(ScraperDesigner.Selector selector)
        {
            var pathList = selector.Path.path;
            HtmlElementCollection siblings = siblings = webBrowser1.Document.Body.Children;
            HtmlElement lastElement = null, element;
            int i = 0;

            do
            {
                element = Matches(pathList[i++], siblings);
                lastElement = element;
                if (element != null)
                {
                    siblings = element.Children;
                }
                else if (i < pathList.Count)
                {
                    return null;
                }
            } while (i < pathList.Count);
            return Matches(selector, siblings);
        }
        
        internal HtmlElement Matches(ScraperDesigner.Selector selector, HtmlElementCollection siblings = null)
        {
            if (siblings == null)
            {
                siblings = webBrowser1.Document.Body.All;
            }

            foreach (HtmlElement element in siblings)
            {
                if (selector.Match(element))
                    return element;
            }
            return null;
        }

        private void ResizeControls()
        {
            webBrowser1.Width = this.Size.Width - 32;
            webBrowser1.Height = this.Size.Height - 160;
            txtAddress.Width = this.Size.Width - 150;
            txtElementPath.Width = this.Size.Width - 90;
            txtElementAttributes.Width = this.Size.Width - 90;
        }

        /// <summary>
        /// Builds a path from the document root to an HTML element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static ElementPath BuildPath(HtmlElement element)
        {
            ElementPath path = new ElementPath();
            element = element.Parent;
            while (element != null)
            {
                if (element.TagName != "HTML" && element.TagName != "BODY")
                {
                    path.path.Insert(0, new ScraperDesigner.Selector(element, false));
                }
                element = element.Parent;
            }
            return path;
        }
        
        /// <summary>
        /// Returns the path from the root element to the indicated element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string ElementPathString(HtmlElement element, bool shortForm = false)
        {
            StringBuilder sb = new StringBuilder(shortForm ? "/" : "");
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
                        elementIdentifier = string.Format("{0}={1}", attr, value, shortForm ? "" : " ");
                        break;
                    }
                }

                string thisElement = string.Format("{0} {1}", ancestor.TagName, elementIdentifier);

                if (firstElement)
                    firstElement = false;
                else
                    thisElement += shortForm ? "/" : "-> ";

                ancestor = ancestor.Parent;
                path = thisElement + path;
            }
            sb.Append(path);
            return sb.ToString();
        }

        /// <summary>
        /// Returns the tag name and values of some attributes of an HTML element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string ElementInfoString(HtmlElement element)
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

        /// <summary>
        /// Applies the highlight style to an element while restoring the style of the last highlighted element.
        /// </summary>
        /// <param name="element"></param>
        internal void HighlightElement(HtmlElement element)
        {
            // Unhighlight last control moused over
            if (lastMouseOverElement != null)
                lastMouseOverElement.Style = lastStyle;

            lastMouseOverElement = element;
            if (element != null)
            {
                lastStyle = element.Style;
                element.Style += HIGHLIGHT_STYLE;
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

        ///////////////////////////////////////////////////////////////////////
        // Event handlers
        ///////////////////////////////////////////////////////////////////////

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            ResizeControls();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResizeControls();
        }

        /// <summary>
        /// Called when the browser finishes loading a document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                element.GotFocus += ElementGotFocus;
                element.Click += ElementClicked;
                element.MouseEnter += InspectElement;
            }
        }

        /// <summary>
        /// Highlights an element when it receives focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElementGotFocus(object sender, HtmlElementEventArgs e)
        {
            HighlightElement((HtmlElement)sender);
        }
        
        /// <summary>
        /// Pops up a dialog to save an element to the step's element collection, so long
        /// as:
        ///  * There is a step to add to
        ///  * Selection mode is on
        ///  * 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElementClicked(object sender, HtmlElementEventArgs e)
        {
            // Are we getting GotFocus events from parent elements too?
            HtmlElement element = (HtmlElement)sender;
            //HtmlElement element = lastMouseOverElement;
            if (!chkSelectionMode.Checked ||        // Selection mode disabled
                steps.Count == 0 ||                 // No steps to add to
                element == null ||                  // Nothing hovered over yet
                !selectionComplete ||
                element != lastMouseOverElement)
            {
                return;
            }
            selectionComplete = false;

            // Prompt for a name for the new element
            string elementName;
            var elementNameDialog = new ElementNameDialog(element);
            elementNameDialog.txtElementInfo.Text = ElementInfoString(element);
            if (elementNameDialog.ShowDialog() == DialogResult.OK)
            {
                elementName = elementNameDialog.txtElementName.Text;
            }
            else
            {
                selectionComplete = true;
                return;
            }
            element = elementNameDialog.Element;

            // This statement unhighlights the currently highlighted element and forgets it so that
            // this method is only triggered once.
            HighlightElement(null);

            // Add to listbox and elements collection.
            int elementsIndex = stepsDialog.lstElements.SelectedIndex;
            stepsDialog.lstElements.Items.Insert(elementsIndex + 1, elementName);
            stepsDialog.lstElements.SelectedIndex = elementsIndex + 1;

            // Add the element to our list as well.
            Selector selector = new Selector(element, elementNameDialog.chkOptional.Checked);
            var definition = new ElementDefinition()
            {
                Element = element,
                Id = elementName,
                ElementSelector = selector
            };
            int stepsIndex = stepsDialog.lstSteps.SelectedIndex;
            steps[stepsIndex].Elements.Insert(elementsIndex + 1, definition);
            selectionComplete = true;
        }
        
        // Highlights an element and displays info on it on mouseover or click.
        private void InspectElement(object sender, EventArgs e)
        {
            if (!chkSelectionMode.Checked)
                return;

            HtmlElement element = (HtmlElement)sender;
            HighlightElement(element);

            // show tag name and values of its attributes
            txtElementPath.Text = ElementPathString(element);
            txtElementAttributes.Text = ElementInfoString(element);
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
            webBrowser1.AllowNavigation = !chkSelectionMode.Checked;
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
