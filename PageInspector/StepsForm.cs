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
        private int flashCounter;
        private static int NUM_FLASHES = 4;
        private static int FLASH_INTERVAL = 35;
        private Timer flashTimer;

        public StepsForm(DesignerWindow designer)
        {
            InitializeComponent();
            this.designer = designer;
            flashTimer = new Timer();
            flashTimer.Interval = FLASH_INTERVAL;
        }

        /// <summary>
        /// Swaps the foreground and background colors of the "Add step" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlashAddStepButton(object sender, EventArgs e)
        {
            Color foreColor = btnAddStep.ForeColor;
            Color backColor = btnAddStep.BackColor;
            btnAddStep.ForeColor = backColor;
            btnAddStep.BackColor = foreColor;

            if (--flashCounter == 0)
                flashTimer.Tick -= FlashAddStepButton;
        }

        /// <summary>
        /// Beeps and flashes the "Add step" button.
        /// </summary>
        public void AlertUser()
        {
            System.Media.SystemSounds.Beep.Play();

            flashCounter = NUM_FLASHES * 2;
            flashTimer.Tick += FlashAddStepButton;
            flashTimer.Start();
        }

        private void btnAddStep_Click(object sender, EventArgs e)
        {
            if (designer.webBrowser1.Url != null)
            {
                int index = lstSteps.SelectedIndex;
                designer.steps.Insert(index + 1, new ScraperStep(designer.webBrowser1.Url.ToString()));
                lstSteps.Items.Insert(index + 1, designer.webBrowser1.Url.ToString());
                lstSteps.SelectedIndex = index + 1;
                designer.changesMade = true;
            }
        }

        /// <summary>
        /// Highlights an element on the page when it's selected in the elements list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstElements_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int stepIndex = lstSteps.SelectedIndex;
            int elementIndex = lstElements.SelectedIndex;
            //designer.HighlightElement(designer.steps[stepIndex].Elements[elementIndex].Element);
            ElementDefinition def = designer.steps[stepIndex].Elements[elementIndex];
            designer.HighlightElement(designer.FindBySelector(def.ElementSelector));
        }

        /// <summary>
        /// Updates the elements list when a different step is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
