using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScraperDesigner
{
    public partial class ElementNameDialog : Form
    {
        private HtmlElement _element;
        
        public HtmlElement Element
        {
            private set
            {
                _element = value;
            }

            get
            {
                return _element;
            }
        }

        private Stack<HtmlElement> skippedElements;

        public ElementNameDialog(HtmlElement element)
        {
            InitializeComponent();
            this._element = element;
            skippedElements = new Stack<HtmlElement>();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            btnDown.Enabled = skippedElements.Count > 0;
            btnUp.Enabled = _element != null && _element.Parent != null && _element.Parent.TagName != "BODY";
        }

        private void btnUp_Click(object sender, EventArgs e)
        {

            txtElementInfo.Text = DesignerWindow.ElementInfoString(_element.Parent);
            skippedElements.Push(_element);
            _element = _element.Parent;
            _element.Focus();
            UpdateButtons();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            _element = skippedElements.Pop();
            txtElementInfo.Text = DesignerWindow.ElementInfoString(_element);
            _element.Focus();
            UpdateButtons();
        }

        private void txtElementName_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = txtElementName.Text.Length > 0;
        }

    }
}
