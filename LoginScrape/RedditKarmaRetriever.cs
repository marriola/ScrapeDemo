using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginScrape
{
    class RedditKarmaScraper : IScraper
    {
        WebBrowser browser;
        string username, password;
        TextBox outputControl;

        public RedditKarmaScraper(string username, string password, TextBox outputControl)
        {
            this.username = username;
            this.password = password;
            this.outputControl = outputControl;
        }

        private void PrintDocument(object browser)
        {
            Debug.WriteLine("*** " + ((WebBrowser)browser).Url);
            Debug.WriteLine((((WebBrowser)browser).Document.GetElementsByTagName("html"))[0].InnerText);
        }

        /// <summary>
        /// Retrieves link karma from user sidebar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewMainPage(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // iterate through all <span> elements until we find the one with the class "comment-karma"
            string karmaText = string.Empty;
            foreach (HtmlElement element in browser.Document.GetElementsByTagName("span"))
            {
                Debug.WriteLine(element.GetAttribute("classname"));
                if (element.GetAttribute("classname") == "userkarma")
                {
                    karmaText = element.InnerText;
                }
            }

            outputControl.Text = string.IsNullOrEmpty(karmaText) ? "Not found" : karmaText;
            browser.DocumentCompleted -= ViewMainPage;

            // find logout form and submit
            foreach (HtmlElement element in browser.Document.GetElementsByTagName("form"))
            {
                if (element.GetAttribute("classname") == "logout hover")
                {
                    Debug.WriteLine("logging out");
                    element.InvokeMember("submit");
                }
            }
        }

        /// <summary>
        /// Enters username and password from login form and submits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //PrintDocument(browser);
            browser.DocumentCompleted -= Login;
            browser.DocumentCompleted += ViewMainPage;

            try
            {
                browser.Document.GetElementById("user_login").SetAttribute("value", username);
                browser.Document.GetElementById("passwd_login").SetAttribute("value", password);
                browser.Document.GetElementById("login-form").InvokeMember("submit");
            }
            catch (Exception ex)
            {
                //PrintDocument(browser);
                browser.DocumentCompleted -= ViewMainPage;
                outputControl.Text = "Error";
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves login page
        /// </summary>
        public void Scrape()
        {
            browser = new WebBrowser();
            browser.Navigate("www.reddit.com/login");
            browser.DocumentCompleted += Login;
        }
    }
}
