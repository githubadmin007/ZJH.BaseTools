namespace ZJH.BaseTools.UI
{
    partial class InputBox
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
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_calcel = new System.Windows.Forms.Button();
            this.lb_tips = new System.Windows.Forms.Label();
            this.txt_input = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(379, 16);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 0;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_calcel
            // 
            this.btn_calcel.Location = new System.Drawing.Point(379, 56);
            this.btn_calcel.Name = "btn_calcel";
            this.btn_calcel.Size = new System.Drawing.Size(75, 23);
            this.btn_calcel.TabIndex = 1;
            this.btn_calcel.Text = "取消";
            this.btn_calcel.UseVisualStyleBackColor = true;
            this.btn_calcel.Click += new System.EventHandler(this.btn_calcel_Click);
            // 
            // lb_tips
            // 
            this.lb_tips.AutoSize = true;
            this.lb_tips.Location = new System.Drawing.Point(12, 21);
            this.lb_tips.Name = "lb_tips";
            this.lb_tips.Size = new System.Drawing.Size(41, 12);
            this.lb_tips.TabIndex = 2;
            this.lb_tips.Text = "label1";
            // 
            // txt_input
            // 
            this.txt_input.Location = new System.Drawing.Point(14, 92);
            this.txt_input.Name = "txt_input";
            this.txt_input.Size = new System.Drawing.Size(440, 21);
            this.txt_input.TabIndex = 0;
            this.txt_input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_input_KeyDown);
            // 
            // InputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 121);
            this.Controls.Add(this.txt_input);
            this.Controls.Add(this.lb_tips);
            this.Controls.Add(this.btn_calcel);
            this.Controls.Add(this.btn_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InputBox";
            this.Text = "InputBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_calcel;
        private System.Windows.Forms.Label lb_tips;
        private System.Windows.Forms.TextBox txt_input;
    }
}