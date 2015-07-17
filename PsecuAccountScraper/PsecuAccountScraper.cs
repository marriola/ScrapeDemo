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

        public PsecuAccountScraper(string username, string password, ListBox listControl)
        {
            AddScraperStep(Start);
            AddScraperStep(Login);
            AddScraperStep(AccountPage);

            m_username = username;
            m_password = password;
            m_listControl = listControl;
        }

        private bool Start()
        {
            browser.Navigate("www.psecu.com");
            return true;
        }

        private bool Login()
        {
            try
            {
                browser.Document.GetElementById("AccountNumber").SetAttribute("value", m_username);
                browser.Document.GetElementById("Pwd").SetAttribute("value", m_password);
                browser.Document.Forms["Form1"].InvokeMember("submit");
                return true;
            }
            catch (NullReferenceException nre)
            {
                SetErrorState();
                return false;
            }
        }

        private bool AccountPage()
        {
            Debug.WriteLine(browser.Document.Body.InnerHtml);
            return true;
        }

        public override void Scrape()
        {
            Advance();
        }
    }
}
