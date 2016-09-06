using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewEditor
{
    public partial class ScaleBox : Form
    {
        public ScaleBox(ViewEditor vw)
        {
            InitializeComponent();
            view = vw;
        }

        public ViewEditor view
        {
            get;
            set;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            double result_x, result_y;
            if (double.TryParse(txtYScale.Text, out result_x) && double.TryParse(txtXScale.Text, out result_y))
            {
                view.x_scale = Convert.ToDouble(txtXScale.Text.ToString());
                view.y_scale = Convert.ToDouble(txtYScale.Text.ToString());
                view.customScale();
                this.Close();
            }
            else
            {
                this.Close();  
                String message = "Invalid Input Entered!";
                String caption = "View Editor";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, caption, buttons);
                view.Enabled = true;  
            }
        }

        private void txtXScale_Validating(object sender, CancelEventArgs e)
        {
            double result;
            if (!double.TryParse(txtXScale.Text, out result))
            {
                toolTipInput.Show("Input should be a numeric value!", txtXScale);
            }
        }

        private void txtYScale_Validating(object sender, CancelEventArgs e)
        {
            double result;
            if (!double.TryParse(txtYScale.Text, out result))
            {
                toolTipInput.Show("Input should be a numeric value!", txtYScale);
            }
        }

        private void txtXScale_Validated(object sender, EventArgs e)
        {
            
        }

        private void ScaleBox_FormClosed(object sender, FormClosedEventArgs e)
        {
            view.Enabled = true;
        }
    }
}
