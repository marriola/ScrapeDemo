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
        const string HIGHLIGHT_STYLE = "background-color: #ff8900; color: #772200;";
        static readonly string[] identifyingAttributes = new string[] { "id", "name", "href", "class" };
        static readonly string[] attributes = new string[] { "id", "name", "className", "href", "type" };

        const string windowTitle = "Page inspector";

        HtmlDocument document;
        HtmlElement lastMouseOverElement = null;
        string lastStyle = string.Empty;
        bool selectionComplete = true;
        internal bool changesMade = false;

        StepsForm stepsDialog;

        public DesignerWindow()
        {
            InitializeComponent();
            steps = new List<ScraperStep>();
            stepsDialog = new StepsForm(this);
            stepsDialog.Show();
            webBrowser1.AllowNavigation = !chkSelectionMode.Checked;
        }

        /// <summary>
        /// Tries to match an element in the currently loaded document by a selector.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        internal HtmlElement FindBySelector(ScraperDesigner.Selector selector)
        {
            var pathList = selector.Path.path;
            HtmlElementCollection siblings = siblings = webBrowser1.Document.Body.Children;
            HtmlElement lastElement = null, element;
            int i = 0;

            do
            {
                element = Match(pathList[i++], siblings);
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
            return Match(selector, siblings);
        }
        
        /// <summary>
        /// Attempts to match a selector to an element in the currently loaded document.
        /// </summary>
        /// <param name="selector">A selector specifying an element to match.</param>
        /// <param name="siblings">An element collection from which to match the selector. If null, the elements at the top level of the document body are searched.</param>
        /// <returns></returns>
        internal HtmlElement Match(ScraperDesigner.Selector selector, HtmlElementCollection siblings = null)
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

        /// <summary>
        /// Resizes the page controls to fill the entire browser window.
        /// </summary>
        private void ResizeControls()
        {
            webBrowser1.Width = Size.Width - 32;
            webBrowser1.Height = Size.Height - 160;
            txtAddress.Width = Size.Width - 167;
            pbThrobber.Location = new Point(Size.Width - 36, pbThrobber.Location.Y);
            txtElementPath.Width = Size.Width - 90;
            txtElementAttributes.Width = Size.Width - 90;
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
            HtmlElement ancestor = element.Parent;
            string path = shortForm ? "/" : "";
            bool addSeparator = false;

            while (ancestor != null)
            {
                // This is the display name of the control. Iterate through each of the
                // identifying attributes until we find one.
                string elementIdentifier = string.Empty;
                foreach (string attr in identifyingAttributes)
                {
                    string value = ancestor.GetAttribute(attr);
                    if (!string.IsNullOrEmpty(value))
                    {
                        elementIdentifier = string.Format("{0}={1}", attr, value);
                        break;
                    }
                }

                string thisElement = string.Format("{0} {1}", ancestor.TagName, elementIdentifier);

                if (addSeparator)
                    thisElement += shortForm ? "/" : "-> ";
                else
                    addSeparator = true;

                ancestor = ancestor.Parent;
                path = thisElement + path;
            }

            return path;
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
        /// <param name="element">The element to highlight. Null may be passed to unhighlight the currently highlighted element if there is one.</param>
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

        /// <summary>
        /// Saves the current scraper specification to disk.
        /// </summary>
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

        /// <summary>
        /// Loads a scraper specification from disk.
        /// </summary>
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
            }
            dlgOpen.Dispose();
        }

        /// <summary>
        /// Prompts the user to save changes made to the scraper. If no changes have been made, the user is not prompted.
        /// </summary>
        /// <returns>True if the designer should continue to shut down, false if not.</returns>
        private bool PromptSave()
        {
            if (!changesMade)
                return true;

            DialogResult savePromptResult = MessageBox.Show("Do you want to save changes to the scraper specification?", "Scraper designer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            switch (savePromptResult)
            {
                case DialogResult.Yes:
                    SaveScraper();
                    return true;

                case DialogResult.No:
                    return true;

                case DialogResult.Cancel:
                    return false;

                default:
                    return false;
            }
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
            pbThrobber.Visible = false;
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
            if (!chkSelectionMode.Checked)
                return;
            HighlightElement((HtmlElement)sender);
        }

        /// <summary>
        /// Pops up a dialog to save an element to the step's element collection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElementClicked(object sender, HtmlElementEventArgs e)
        {
            HtmlElement element = (HtmlElement)sender;

            // Return if...
            if (!chkSelectionMode.Checked ||        // ...selection mode is disabled
                element == null ||                  // ...nothing hovered over yet
                !selectionComplete ||               // ...the element name dialog is still open
                element != lastMouseOverElement)    // ...or the element receiving this event isn't the last one hovered over
            {
                return;
            }

            // If there are no steps in the specification, alert the user and return.
            if (steps.Count == 0)
            {
                stepsDialog.AlertUser();
                return;
            }

            selectionComplete = false;

            // Prompt for a name for the new element.
            var elementNameDialog = new ElementNameDialog(element);
            elementNameDialog.txtElementInfo.Text = ElementInfoString(element);

            if (elementNameDialog.ShowDialog() == DialogResult.Cancel)
            {
                selectionComplete = true;
                return;
            }

            string elementName = elementNameDialog.txtElementName.Text;
            element = elementNameDialog.Element; // If the user selects one of its ancestors instead, use that.

            // Unhighlight the currently highlighted element.
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
            changesMade = true;

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
                pbThrobber.Visible = true;
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
            if (PromptSave())
                LoadScraper();
        }

        private void itmExit_Click(object sender, EventArgs e)
        {
            if (PromptSave())
                Environment.Exit(0);
        }

        private void DesignerWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !PromptSave();
        }
    }
}
