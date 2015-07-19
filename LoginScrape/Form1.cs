using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Scraper;

namespace LoginScrape
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        ScraperManager scraperManager;

        public Form1()
        {
            InitializeComponent();
            //try
            //{
                scraperManager = ScraperManager.GetSharedInstance();
                foreach (string scraper in scraperManager.ScraperNames)
                {
                    lstScrapers.Items.Add(scraper);
                }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message, "ScraperManager threw an exception");
            //}
        }

        private void btnScrapeReddit_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Scraper.Scraper scraper = scraperManager.GetScraper("RedditKarmaScraper", new object[3] { loginForm.Username, loginForm.Password, txtKarma });
                    scraper.Scrape(); // scrapers gonna scrape
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Couldn't load RedditKarmaScraper");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            loginForm.Dispose();
        }

        private void btnScrapePSECU_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();

            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Scraper.Scraper scraper = scraperManager.GetScraper("PsecuAccountScraper", new object[3] { loginForm.Username, loginForm.Password, listBox1 });
                    scraper.Scrape();
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Couldn't load PsecuAccountScraper");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            loginForm.Dispose();
        }
    }
}
