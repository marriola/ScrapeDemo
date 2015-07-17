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

        private Dictionary<int, ScraperStep> scraperFunctions = new Dictionary<int, ScraperStep>();        
        private Dictionary<int, int> transitionTable = new Dictionary<int, int>();

        public delegate void ScraperError();
        public event ScraperError ErrorHandler;

        // Keeps track of the next state to add to the transition table.
        private int addNextState;

        private int state = START_STATE;
        protected WebBrowser browser;

        /// <summary>
        /// Creates a new Browser instance, which is available to the subclass, and sets it to automatically
        /// advance the state of the scraper when it finishes loading a page.
        /// </summary>
        /// <param name="suppressScriptErrors">Whether to suppress script error dialogs. True by default.</param>
        protected Scraper(bool suppressScriptErrors = true)
        {
            browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = suppressScriptErrors;
            browser.DocumentCompleted += Scraper_OnDocumentCompleted;
            addNextState = START_STATE;
        }

        /// <summary>
        /// Registers a new scraper step with an associated callback function.
        /// </summary>
        /// <param name="fun"></param>
        protected int AddScraperStep(ScraperStep fun)
        {
            // Have the last step transition to this step, unless this is the first one.
            if (addNextState != START_STATE)
            {
                transitionTable[addNextState - 1] = addNextState;
            }

            // Have this step transition to the finish state once completed.
            // If we add another step, this will get changed to point to that instead.
            transitionTable[addNextState] = FINISH_STATE;

            // Finally, hook up the callback function to the current step.
            scraperFunctions[addNextState] = fun;
            return addNextState++;
        }

        /// <summary>
        /// If the current state is the error state, it unhooks the scraper from its browser's
        /// DocumentCompleted event. Otherwise, it performs the function indicated for the
        /// current state. The state of the scraper will also be advanced if that function
        /// returns true.
        /// </summary>
        protected void Advance()
        {
            if (state == ERROR_STATE || state == FINISH_STATE)
            {
                browser.DocumentCompleted -= Scraper_OnDocumentCompleted;
            }
            else if (scraperFunctions[state]())
            {
                state = transitionTable[state];
            }

            if (state == ERROR_STATE)
            {
                ErrorHandler();
            }
        }

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
        protected void SetErrorState()
        {
            state = ERROR_STATE;
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
