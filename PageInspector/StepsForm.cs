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
    public partial class StepsForm : Form
    {
        private DesignerWindow designer;

        public StepsForm(DesignerWindow designer)
        {
            InitializeComponent();
            this.designer = designer;
        }

        private void btnAddStep_Click(object sender, EventArgs e)
        {
            if (designer.webBrowser1.Url != null)
            {
                int index = lstSteps.SelectedIndex;
                designer.steps.Insert(index + 1, new ScraperStep(designer.webBrowser1.Url.ToString()));
                lstSteps.Items.Insert(index + 1, designer.webBrowser1.Url.ToString());
                lstSteps.SelectedIndex = index + 1;
            }
        }

        private void lstElements_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int stepIndex = lstSteps.SelectedIndex;
            int elementIndex = lstElements.SelectedIndex;
            //designer.HighlightElement(designer.steps[stepIndex].Elements[elementIndex].Element);
            ElementDefinition def = designer.steps[stepIndex].Elements[elementIndex];
            designer.HighlightElement(designer.FindBySelector(def.ElementSelector));
        }

        private void lstSteps_SelectedIndexChanged(object sender, EventArgs e)
        {
            int stepsIndex = lstSteps.SelectedIndex;
            lstElements.Items.Clear();
            foreach (var def in designer.steps[stepsIndex].Elements)
            {
                lstElements.Items.Add(def.Id);
            }
        }
    }
}
