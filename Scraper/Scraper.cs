using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Scraper
{
    using NamedElementDictionary = Dictionary<string, Selector>;
    using PageElementDictionary = Dictionary<string, HtmlElement>;
    using ReadOnlyPageElementDictionary = ReadOnlyDictionary<string, HtmlElement>;

    struct Selector
    {
        public string Tag;
        public string ClientId;
        public string Name;
        public string ClassName;
        public bool Optional;

        public override string ToString()
        {
            return string.Format("tag={0}, id={1}, name={2}, class={3}", Tag, ClientId, Name, ClassName);
        }
    }

    public abstract class Scraper
    {
        // Static fields
        protected static int START_STATE = 0;
        private static int FINISH_STATE = unchecked((int)0xfffffffe);
        private static int ERROR_STATE = unchecked((int)0xffffffff);

        // Path to the XML file that defines the page elements present at each step.
        private string definitionFile;
        
        // Instance fields
        private List<NamedElementDictionary> namedElements;
        private PageElementDictionary pageElements;

        /// <summary>
        /// A function that is called whenever the site being navigated by the scraper finishes loading a new page.
        /// </summary>
        /// <returns>True if the scraper should advance to the next state, i.e. if another page has
        /// been requested. Return false to keep the scraper from automatically moving to the next
        /// state, i.e. if loading more dynamically generated data from the same page, or if an error
        /// condition has been encountered.</returns>
        protected delegate bool ScraperStep();
        private List<ScraperStep> scraperFunctions;
        
        public delegate void ErrorFunction(string errorMessage, Exception exception);
        public event ErrorFunction ScraperError;

        // Keeps track of the next state to add to the transition table.
        private int addNextState;

        private int state = START_STATE;

        // Properties
        protected WebBrowser Browser
        {
            get;
            private set;
        }

        protected Exception ScraperException
        {
            get;
            private set;
        }

        protected string ErrorString
        {
            get;
            private set;
        }

        protected ReadOnlyPageElementDictionary PageElements
        {
            get
            {
                return new ReadOnlyPageElementDictionary(pageElements);
            }
        }

        /// <summary>
        /// Creates a new Browser instance, which is available to the subclass, and sets it to automatically
        /// advance the state of the scraper when it finishes loading a page.
        /// </summary>
        /// <param name="suppressScriptErrors">Whether to suppress script error dialogs. True by default.</param>
        protected Scraper(string definitionFile, bool suppressScriptErrors = true)
        {
            this.definitionFile = definitionFile;
            
            Browser = new WebBrowser();
            Browser.ScriptErrorsSuppressed = suppressScriptErrors;
            Browser.DocumentCompleted += Scraper_OnDocumentCompleted;

            addNextState = START_STATE;
            scraperFunctions = new List<ScraperStep>();

            // Load the elements defined for each step from the scraper definition file.
            var reader = new ScraperDefinitionReader(definitionFile);
            reader.ReadScraperDefinition();
            namedElements = reader.namedElements;
        }

        /// <summary>
        /// Registers a new scraper step with an associated callback function.
        /// </summary>
        /// <param name="fun"></param>
        protected int AddScraperStep(ScraperStep fun)
        {
            // Hook up the callback function to the current step.
            scraperFunctions.Add(fun);
            return addNextState++;
        }

        /// <summary>
        /// Retrieves the elements indicated by selectors in the scraper definition for the
        /// current step.
        /// </summary>
        /// <returns></returns>
        private void GrabPageElements()
        {
            pageElements = new PageElementDictionary();
            if (state < 0 || state >= addNextState)
                return;

            // Iterate over name-selector pairs.
            foreach (var keyValPair in namedElements[state])
            {
                string name = keyValPair.Key;
                Selector selector = keyValPair.Value;
                string tag = selector.Tag;

                // Filter by tag name if specified.
                HtmlElementCollection elementCollection;
                if (string.IsNullOrEmpty(tag))
                {
                    elementCollection = Browser.Document.All;
                }
                else
                {
                    elementCollection = Browser.Document.GetElementsByTagName(tag);
                }

                // Attempt to match a page element to the selector. If found, add to the
                // dictionary of page elements.

                //Debug.WriteLine(string.Format("***** tag={0} selector {{ {1} }}", tag, selector.ToString()));
                //Debug.WriteLine(Browser.Document.Body.OuterHtml);
                //Debug.Indent();

                bool found = false;
                foreach (HtmlElement element in elementCollection)
                {
                    //Debug.WriteLine(string.Format("id={0} name={1} class={2}", element.GetAttribute("id"), element.GetAttribute("name"), element.GetAttribute("className")));

                    if ((!string.IsNullOrEmpty(selector.ClientId) && selector.ClientId == element.GetAttribute("id")) ||
                        (!string.IsNullOrEmpty(selector.Name) && selector.Name == element.GetAttribute("name")) ||
                        (!string.IsNullOrEmpty(selector.ClassName) && selector.ClassName == element.GetAttribute("className")))
                    {
                        //Debug.WriteLine("FOUND!!");
                        pageElements[name] = element;
                        found = true;
                        break;
                    }
                }
                //Debug.Unindent();

                if (!found)
                {
                    if (selector.Optional)
                        pageElements[name] = null;
                    else
                        throw new FileFormatException("Couldn't find element " + selector.ToString());
                }
            }
        }

        /// <summary>
        /// If the current state is the error state, it unhooks the scraper from its browser's
        /// DocumentCompleted event. Otherwise, it performs the function indicated for the
        /// current state. The state of the scraper will also be advanced if that function
        /// returns true.
        /// </summary>
        private void Advance()
        {
            if (state == ERROR_STATE || state == FINISH_STATE)
            {
                Browser.DocumentCompleted -= Scraper_OnDocumentCompleted;
                return;
            }

            try
            {
                GrabPageElements();
                if (scraperFunctions[state]())
                {
                    // Set the completed state if we've finished the last step.
                    // Otherwise, on to the next step.
                    state++;
                    if (state == scraperFunctions.Count)
                        state = FINISH_STATE;
                }
            }
            catch (Exception e)
            {
                SetErrorState(e);
            }
        }

        /// <summary>
        /// Sets the scraper state.
        /// </summary>
        /// <param name="state">A step ID number, as returned by AddScraperStep.</param>
        protected void SetState(int state)
        {
            if (state < START_STATE || state >= addNextState)
            {
                throw new ArgumentException("Invalid state ID");
            }
            this.state = state;
        }

        /// <summary>
        /// Sets the scraper to the error state.
        /// </summary>
        /// <param name="errorString">Optional. A string describing the error condition.</param>
        /// <param name="exception">Optional. An uncaught exception.</param>
        protected void SetErrorState(string errorString = null, Exception exception = null)
        {
            state = ERROR_STATE;
            ErrorString = errorString;
            ScraperException = exception;
            if (ScraperError != null)
                ScraperError(errorString, exception);
        }

        /// <summary>
        /// Sets the scraper to the error state.
        /// </summary>
        /// <param name="exception">An uncaught exception.</param>
        protected void SetErrorState(Exception exception)
        {
            SetErrorState("An uncaught exception was encountered", exception);
        }

        /// <summary>
        /// Executes the first scraper step and starts the state machine.
        /// </summary>
        protected void Start()
        {
            if (state != START_STATE)
            {
                throw new InvalidOperationException("State machine is not in start state");
            }
            Advance();
        }

        /// <summary>
        /// Runs the scraper function indicated by the current state when the browser's document finished loading.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scraper_OnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Advance();
        }

        /// <summary>
        /// Starts the scraping process.
        /// </summary>
        public abstract void Scrape();
    }
}
