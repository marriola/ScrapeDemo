using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scraper
{
    public abstract class Scraper
    {
        protected static int START_STATE = 0;
        private static int FINISH_STATE = unchecked((int)0xfffffffe);
        private static int ERROR_STATE = unchecked((int)0xffffffff);

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

        /// <summary>
        /// Creates a new Browser instance, which is available to the subclass, and sets it to automatically
        /// advance the state of the scraper when it finishes loading a page.
        /// </summary>
        /// <param name="suppressScriptErrors">Whether to suppress script error dialogs. True by default.</param>
        protected Scraper(bool suppressScriptErrors = true)
        {
            Browser = new WebBrowser();
            Browser.ScriptErrorsSuppressed = suppressScriptErrors;
            Browser.DocumentCompleted += Scraper_OnDocumentCompleted;
            addNextState = START_STATE;
            scraperFunctions = new List<ScraperStep>();
        }

        /// <summary>
        /// Registers a new scraper step with an associated callback function.
        /// </summary>
        /// <param name="fun"></param>
        protected int AddScraperStep(ScraperStep fun)
        {
            // Hook up the callback function to the current step.
            scraperFunctions[addNextState] = fun;
            return addNextState++;
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
                if (scraperFunctions[state]())
                {
                    // Set the completed state if we've finished the last step.
                    // Otherwise, on to the next step.
                    state = state >= scraperFunctions.Count ? FINISH_STATE : state++;
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
