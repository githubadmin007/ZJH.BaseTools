using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZJH.EsriGIS.Geodatabase;

namespace ZJH.EsriGIS.GeodatabaseUI
{
    public partial class SelectOrCreateWorkspace : Form
    {
        public ZWorkspace workspaceHelper;
        public string gdbPath {
            get {
                if (txt_gdbPath.Text.ToLower().EndsWith(".gdb") && Directory.Exists(txt_gdbPath.Text)) {
                    return txt_gdbPath.Text;
                }
                return "";
            }
        }//GDB路径
        /// <summary>
        /// 构造函数
        /// </summary>
        public SelectOrCreateWorkspace()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }
        /// <summary>
        /// 选择GDB文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_selGDB_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择*.gdb文件";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                if (fbd.SelectedPath.ToLower().EndsWith(".gdb"))
                {
                    txt_gdbPath.Text = fbd.SelectedPath;
                }
                else {
                    MessageBox.Show("请选择*.gdb文件");
                }
            }
        }
        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_GDB_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(gdbPath))
            {
                MessageBox.Show("请选择*.gdb文件");
            }
            else {
                workspaceHelper = ZWorkspace.GetFileGDBWorkspace(gdbPath);
                DialogResult = DialogResult.OK;
            }
        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 显示新建GDB面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_newGDB_Click(object sender, EventArgs e)
        {
            panel_newGDB.Visible = true;
        }

        /// <summary>
        /// 选择GDB存放路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_selGDBFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择GDB存放路径";
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txt_newGDBFolder.Text = fbd.SelectedPath;
            }
        }
        /// <summary>
        /// 创建GDB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_createGDB_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_newGDBFolder.Text)) {
                MessageBox.Show("请选择GDB保存路径");
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_newGDBName.Text))
            {
                MessageBox.Show("请填写GDB名称");
                return;
            }
            try
            {
                string gdbPath = ZWorkspace.CreateGDB(txt_newGDBFolder.Text, txt_newGDBName.Text);
                if (!string.IsNullOrWhiteSpace(gdbPath)) {
                    txt_gdbPath.Text = gdbPath;
                    panel_newGDB.Visible = false;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_cancelNewGDB_Click(object sender, EventArgs e)
        {
            panel_newGDB.Visible = false;
        }
    }
}