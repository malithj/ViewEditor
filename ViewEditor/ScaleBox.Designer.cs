namespace ViewEditor
{
    partial class ScaleBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblXFactor = new System.Windows.Forms.Label();
            this.lblYFactor = new System.Windows.Forms.Label();
            this.txtXScale = new System.Windows.Forms.TextBox();
            this.txtYScale = new System.Windows.Forms.TextBox();
            this.toolTipInput = new System.Windows.Forms.ToolTip(this.components);
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblXFactor
            // 
            this.lblXFactor.AutoSize = true;
            this.lblXFactor.Location = new System.Drawing.Point(25, 24);
            this.lblXFactor.Name = "lblXFactor";
            this.lblXFactor.Size = new System.Drawing.Size(77, 13);
            this.lblXFactor.TabIndex = 0;
            this.lblXFactor.Text = "X Scale Factor";
            // 
            // lblYFactor
            // 
            this.lblYFactor.AutoSize = true;
            this.lblYFactor.Location = new System.Drawing.Point(25, 51);
            this.lblYFactor.Name = "lblYFactor";
            this.lblYFactor.Size = new System.Drawing.Size(77, 13);
            this.lblYFactor.TabIndex = 1;
            this.lblYFactor.Text = "Y Scale Factor";
            // 
            // txtXScale
            // 
            this.txtXScale.Location = new System.Drawing.Point(117, 22);
            this.txtXScale.Name = "txtXScale";
            this.txtXScale.Size = new System.Drawing.Size(66, 20);
            this.txtXScale.TabIndex = 2;
            this.txtXScale.Validating += new System.ComponentModel.CancelEventHandler(this.txtXScale_Validating);
            this.txtXScale.Validated += new System.EventHandler(this.txtXScale_Validated);
            // 
            // txtYScale
            // 
            this.txtYScale.Location = new System.Drawing.Point(117, 48);
            this.txtYScale.Name = "txtYScale";
            this.txtYScale.Size = new System.Drawing.Size(66, 20);
            this.txtYScale.TabIndex = 3;
            this.txtYScale.Validating += new System.ComponentModel.CancelEventHandler(this.txtYScale_Validating);
            // 
            // toolTipInput
            // 
            this.toolTipInput.IsBalloon = true;
            this.toolTipInput.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.toolTipInput.ToolTipTitle = "Invalid";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(73, 83);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ScaleBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 121);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtYScale);
            this.Controls.Add(this.txtXScale);
            this.Controls.Add(this.lblYFactor);
            this.Controls.Add(this.lblXFactor);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScaleBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ScaleBox";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ScaleBox_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblXFactor;
        private System.Windows.Forms.Label lblYFactor;
        private System.Windows.Forms.TextBox txtXScale;
        private System.Windows.Forms.TextBox txtYScale;
        private System.Windows.Forms.ToolTip toolTipInput;
        private System.Windows.Forms.Button btnOK;
    }
}