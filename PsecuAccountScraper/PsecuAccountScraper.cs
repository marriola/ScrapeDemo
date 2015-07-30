using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scraper
{
    public class PsecuAccountScraper : Scraper
    {
        private string m_username;
        private string m_password;
        private ListBox m_listControl;

        public PsecuAccountScraper(string definition, string username, string password, ListBox listControl)
            : base(definition, true)
        {
            AddScraperStep(StartNavigation);
            AddScraperStep(MainPage);
            AddScraperStep(Login);
            AddScraperStep(AccountPage);

            m_username = username;
            m_password = password;
            m_listControl = listControl;
        }

        private bool StartNavigation()
        {
            Browser.Navigate("www.psecu.com");
            return true;
        }

        private void PrintPage(string header = "")
        {
            if (header.Length > 0)
                Debug.WriteLine("===== " + header + " =====");
            Debug.WriteLine(Browser.Document.Body.InnerHtml);
            Debug.WriteLine(new string('*', 80));
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
                elements = Browser.Document.All;
            }
            else
            {
                elements = Browser.Document.GetElementsByTagName(tag);
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

        private bool MainPage()
        {
            try
            {
                PrintPage("Main page");
                System.Threading.Thread.Sleep(5000);
                HtmlElement loginFrame;
                Debug.WriteLine("===== IFRAME =====\n");
                HtmlElement iframe = FindByClass("iframe", "LoginIframe");
                Debug.WriteLine(new string('*', 40) + "\n" + iframe.Document.Body.OuterHtml);
                //string loginUrl = loginForm.GetAttribute("src");
                //Debug.WriteLine("Login form at " + loginUrl);
                //Browser.Navigate(loginUrl);
                return true;
            }
            catch (NullReferenceException)
            {
                SetErrorState();
                return false;
            }
        }

        private bool Login()
        {
            PrintPage("Login");
            try
            {
                Browser.Document.GetElementById("AccountNumber").SetAttribute("value", m_username);
                Browser.Document.GetElementById("Pwd").SetAttribute("value", m_password);
                Browser.Document.Forms["Form1"].InvokeMember("submit");
                return true;
            }
            catch (NullReferenceException)
            {
                SetErrorState();
                return false;
            }
        }

        private bool AccountPage()
        {
            PrintPage();
            return true;
        }

        public void OnError(string errorMessage, Exception exception)
        {
            MessageBox.Show(exception.Source + " " + exception.Message, errorMessage);
        }
        
        public override void Scrape()
        {
            ScraperError += OnError;
            Start();
        }
    }
}
