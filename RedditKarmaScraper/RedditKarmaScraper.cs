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

        public RedditKarmaScraper(string username, string password, TextBox outputControl)
        {
            AddScraperStep(Start);
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
        private bool Start()
        {
            browser.Navigate("www.reddit.com/login");
            return true;
        }

        /// <summary>
        /// Enters username and password from login form and submits
        /// </summary>
        private bool Login()
        {
            try
            {
                browser.Document.GetElementById("user_login").SetAttribute("value", username);
                browser.Document.GetElementById("passwd_login").SetAttribute("value", password);
                browser.Document.GetElementById("login-form").InvokeMember("submit");
                return true;
            }
            catch (Exception ex)
            {
                outputControl.Text = "Error";
                MessageBox.Show(ex.Message);
                SetErrorState();
                return false;
            }
        }

        /// <summary>
        /// Searches the document for an element with a specific CSS class.
        /// </summary>
        /// <param name="tag">The type of element to search, or null or empty to search all elements.</param>
        /// <param name="className">The CSS class name(s) to match.</param>
        /// <returns>The first element that satisfies the criteria.</returns>
        HtmlElement FindByClass(string tag, string className)
        {
            HtmlElementCollection elements;
            if (string.IsNullOrEmpty(tag))
            {
                elements = browser.Document.All;
            }
            else
            {
                elements = browser.Document.GetElementsByTagName(tag);
            }

            foreach (HtmlElement element in elements)
            {
                if (element.GetAttribute("classname") == className)
                {
                    return element;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves link karma from user sidebar
        /// </summary>
        private bool FrontPage()
        {
            // locate <span> element that holds link karma and set the output control text
            HtmlElement userKarmaElement = FindByClass("span", "userkarma");
            outputControl.Text = userKarmaElement == null ? "Not found" : userKarmaElement.InnerText;

            // find logout form and submit
            HtmlElement logoutForm = FindByClass("form", "logout hover");
            if (logoutForm != null)
            {
                Debug.WriteLine("logging out");
                logoutForm.InvokeMember("submit");
            }
            return true;
        }

        private void OnError()
        {
            MessageBox.Show("Something bad happened!");
        }

        /// <summary>
        /// Sets up the scraper state machine and starts the scraping process.
        /// </summary>
        public override void Scrape()
        {
            ErrorHandler += OnError;
            Advance();
        }

    }
}
