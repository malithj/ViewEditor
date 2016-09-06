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
    public partial class UserMask : Form
    {
        public UserMask(ViewEditor VW)
        {
            view = VW;
            InitializeComponent();
        }

        public ViewEditor view
        {
            get;
            set;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            double result;
            if (double.TryParse(txtBox_0_0.Text, out result) && double.TryParse(txtBox_0_1.Text, out result) && double.TryParse(txtBox_0_2.Text, out result) && double.TryParse(txtBox_1_0.Text, out result) && double.TryParse(txtBox_1_1.Text, out result) && double.TryParse(txtBox_1_2.Text, out result) && double.TryParse(txtBox_2_0.Text, out result) && double.TryParse(txtBox_2_1.Text, out result) && double.TryParse(txtBox_2_2.Text, out result))
            {
                double[,] mask = new double[3, 3];

                mask[0, 0] = Convert.ToDouble(txtBox_0_0.Text);
                mask[0, 1] = Convert.ToDouble(txtBox_0_1.Text);
                mask[0, 2] = Convert.ToDouble(txtBox_0_2.Text);
                mask[1, 0] = Convert.ToDouble(txtBox_1_0.Text);
                mask[1, 1] = Convert.ToDouble(txtBox_1_1.Text);
                mask[1, 2] = Convert.ToDouble(txtBox_1_2.Text);
                mask[2, 0] = Convert.ToDouble(txtBox_2_0.Text);
                mask[2, 1] = Convert.ToDouble(txtBox_2_1.Text);
                mask[2, 2] = Convert.ToDouble(txtBox_2_2.Text);

                view.userFilterCall(mask);
                view.Enabled = true;
                this.Close();
            }
            else
            {
                this.Close();
                String message = "Invalid Input Entered!";
                String caption = "View Editor";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult resultBox = MessageBox.Show(message, caption, buttons);
                view.Enabled = true;
            }
        }

       

        private void UserMask_FormClosed(object sender, FormClosedEventArgs e)
        {
            view.Enabled = true;
        }

        private void txtBox_0_0_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBox_0_1.Focus();
            }
        }

        private void txtBox_0_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBox_0_2.Focus();
            }
        }

        private void txtBox_0_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBox_1_0.Focus();
            }
        }

        private void txtBox_1_0_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBox_1_1.Focus();
            }
        }

        private void txtBox_1_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBox_1_2.Focus();
            }
        }

        private void txtBox_1_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBox_2_0.Focus();
            }
        }

        private void txtBox_2_0_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBox_2_1.Focus();
            }
        }

        private void txtBox_2_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBox_2_2.Focus();
            }
        }

        private void txtBox_2_2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtBox_0_0_Validating(object sender, CancelEventArgs e)
        {
            double result;
            if (!double.TryParse(txtBox_0_0.Text, out result))
            {
                toolTipUser.Show("Input should be a numeric value!", txtBox_0_0);
            }
        }

        private void txtBox_0_1_Validating(object sender, CancelEventArgs e)
        {
            double result;
            if (!double.TryParse(txtBox_0_1.Text, out result))
            {
                toolTipUser.Show("Input should be a numeric value!", txtBox_0_1);
            }

        }

        private void txtBox_0_2_Validating(object sender, CancelEventArgs e)
        {
            double result;
            if (!double.TryParse(txtBox_0_2.Text, out result))
            {
                toolTipUser.Show("Input should be a numeric value!", txtBox_0_2);
            }
        }

        private void txtBox_1_0_Validating(object sender, CancelEventArgs e)
        {
            double result;
            if (!double.TryParse(txtBox_1_0.Text, out result))
            {
                toolTipUser.Show("Input should be a numeric value!", txtBox_1_0);
            }
        }

        private void txtBox_1_1_Validating(object sender, CancelEventArgs e)
        {
            double result;
            if (!double.TryParse(txtBox_1_1.Text, out result))
            {
                toolTipUser.Show("Input should be a numeric value!", txtBox_1_1);
            }
        }

        private void txtBox_1_2_Validating(object sender, CancelEventArgs e)
        {
            double result;
            if (!double.TryParse(txtBox_1_2.Text, out result))
            {
                toolTipUser.Show("Input should be a numeric value!", txtBox_1_2);
            }
        }

        private void txtBox_2_0_Validating(object sender, CancelEventArgs e)
        {
            double result;
            if (!double.TryParse(txtBox_2_0.Text, out result))
            {
                toolTipUser.Show("Input should be a numeric value!", txtBox_2_0);
            }
        }

        private void txtBox_2_1_Validating(object sender, CancelEventArgs e)
        {
            double result;
            if (!double.TryParse(txtBox_2_1.Text, out result))
            {
                toolTipUser.Show("Input should be a numeric value!", txtBox_2_1);
            }
        }

        private void txtBox_2_2_Validating(object sender, CancelEventArgs e)
        {
            double result;
            if (!double.TryParse(txtBox_2_2.Text, out result))
            {
                toolTipUser.Show("Input should be a numeric value!", txtBox_2_2);
            }
        }

       
    }
}
