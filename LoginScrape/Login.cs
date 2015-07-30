using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginScrape
{
    public partial class Login : Form
    {
        public string Username
        {
            get
            {
                return txtUsername.Text;
            }
        }

        public string Password
        {
            get
            {
                return txtPassword.Text;
            }
        }

        public Login()
        {
            InitializeComponent();
        }
    }
}
