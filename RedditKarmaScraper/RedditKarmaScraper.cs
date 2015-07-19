using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scraper
{
    public class RedditKarmaScraper : Scraper
    {
        string username, password;
        TextBox outputControl;

        public RedditKarmaScraper(string definition, string username, string password, TextBox outputControl)
            : base(definition, true)
        {
            AddScraperStep(StartNavigation);
            AddScraperStep(Login);
            AddScraperStep(FrontPage);

            this.username = username;
            this.password = password;
            this.outputControl = outputControl;
        }

        /// <summary>
        /// Navigates to the Reddit login page.
        /// </summary>
        /// <returns></returns>
        private bool StartNavigation()
        {
            Browser.Navigate("www.reddit.com/login");
            return true;
        }

        /// <summary>
        /// Enters username and password from login form and submits
        /// </summary>
        private bool Login()
        {
            try
            {
                PageElements["username"].SetAttribute("value", username);
                PageElements["password"].SetAttribute("value", password);
                PageElements["login"].InvokeMember("submit");
                return true;
            }
            catch (Exception ex)
            {
                outputControl.Text = "Error";
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                SetErrorState();
                return false;
            }
        }

        /// <summary>
        /// Retrieves link karma from user sidebar
        /// </summary>
        private bool FrontPage()
        {
            // locate <span> element that holds link karma and set the output control text
            HtmlElement userKarmaElement = PageElements["karma"];
            outputControl.Text = userKarmaElement == null ? "Not found" : userKarmaElement.InnerText;

            // find logout form and submit
            HtmlElement logoutForm = PageElements["logout"];
            if (logoutForm != null)
                logoutForm.InvokeMember("submit");
            return true;
        }

        private void OnError(string errorMessage, Exception exception)
        {
            if (exception == null)
                MessageBox.Show(errorMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Sets up the scraper state machine and starts the scraping process.
        /// </summary>
        public override void Scrape()
        {
            ScraperError += OnError;
            outputControl.Text = "Loading...";
            Start();
        }

    }
}
