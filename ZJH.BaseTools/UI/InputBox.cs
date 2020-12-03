using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZJH.BaseTools.UI
{
    public partial class InputBox : Form
    {
        public bool AllowEmpty = false;//是否允许空字符串



        public InputBox(string Tips, string title = "",string OKButton = "确定",string CancelButton="取消")
        {
            InitializeComponent();
            this.Text = title;
            lb_tips.Text = Tips;
            btn_OK.Text = OKButton;
            btn_calcel.Text = CancelButton;
            DialogResult = DialogResult.Cancel;
        }

        public string InputText
        {
            get
            {
                return txt_input.Text;
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (!AllowEmpty && string.IsNullOrEmpty(txt_input.Text)) {
                MessageBox.Show("不允许为空！");
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btn_calcel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txt_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                btn_OK_Click(null, null);
            }
        }
    }
}
