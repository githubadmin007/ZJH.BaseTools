namespace ZJH.EsriGIS.GeodatabaseUI
{
    partial class SelectOrCreateWorkspace
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
            this.tabGDB = new System.Windows.Forms.TabPage();
            this.panel_newGDB = new System.Windows.Forms.Panel();
            this.btn_createGDB = new System.Windows.Forms.Button();
            this.txt_newGDBName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_selGDBFolder = new System.Windows.Forms.Button();
            this.txt_newGDBFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_cancel_GDB = new System.Windows.Forms.Button();
            this.btn_OK_GDB = new System.Windows.Forms.Button();
            this.btn_newGDB = new System.Windows.Forms.Button();
            this.btn_selGDB = new System.Windows.Forms.Button();
            this.txt_gdbPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.btn_cancelNewGDB = new System.Windows.Forms.Button();
            this.tabGDB.SuspendLayout();
            this.panel_newGDB.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabGDB
            // 
            this.tabGDB.Controls.Add(this.panel_newGDB);
            this.tabGDB.Controls.Add(this.btn_cancel_GDB);
            this.tabGDB.Controls.Add(this.btn_OK_GDB);
            this.tabGDB.Controls.Add(this.btn_newGDB);
            this.tabGDB.Controls.Add(this.btn_selGDB);
            this.tabGDB.Controls.Add(this.txt_gdbPath);
            this.tabGDB.Controls.Add(this.label1);
            this.tabGDB.Location = new System.Drawing.Point(4, 22);
            this.tabGDB.Name = "tabGDB";
            this.tabGDB.Padding = new System.Windows.Forms.Padding(3);
            this.tabGDB.Size = new System.Drawing.Size(709, 375);
            this.tabGDB.TabIndex = 0;
            this.tabGDB.Text = "GDB";
            this.tabGDB.UseVisualStyleBackColor = true;
            // 
            // panel_newGDB
            // 
            this.panel_newGDB.Controls.Add(this.btn_cancelNewGDB);
            this.panel_newGDB.Controls.Add(this.btn_createGDB);
            this.panel_newGDB.Controls.Add(this.txt_newGDBName);
            this.panel_newGDB.Controls.Add(this.label3);
            this.panel_newGDB.Controls.Add(this.btn_selGDBFolder);
            this.panel_newGDB.Controls.Add(this.txt_newGDBFolder);
            this.panel_newGDB.Controls.Add(this.label2);
            this.panel_newGDB.Location = new System.Drawing.Point(16, 100);
            this.panel_newGDB.Name = "panel_newGDB";
            this.panel_newGDB.Size = new System.Drawing.Size(681, 264);
            this.panel_newGDB.TabIndex = 7;
            this.panel_newGDB.Visible = false;
            // 
            // btn_createGDB
            // 
            this.btn_createGDB.Location = new System.Drawing.Point(513, 98);
            this.btn_createGDB.Name = "btn_createGDB";
            this.btn_createGDB.Size = new System.Drawing.Size(75, 23);
            this.btn_createGDB.TabIndex = 9;
            this.btn_createGDB.Text = "确定";
            this.btn_createGDB.UseVisualStyleBackColor = true;
            this.btn_createGDB.Click += new System.EventHandler(this.btn_createGDB_Click);
            // 
            // txt_newGDBName
            // 
            this.txt_newGDBName.Location = new System.Drawing.Point(79, 54);
            this.txt_newGDBName.Name = "txt_newGDBName";
            this.txt_newGDBName.Size = new System.Drawing.Size(590, 21);
            this.txt_newGDBName.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "GDB名称";
            // 
            // btn_selGDBFolder
            // 
            this.btn_selGDBFolder.Location = new System.Drawing.Point(634, 14);
            this.btn_selGDBFolder.Name = "btn_selGDBFolder";
            this.btn_selGDBFolder.Size = new System.Drawing.Size(35, 23);
            this.btn_selGDBFolder.TabIndex = 6;
            this.btn_selGDBFolder.Text = "...";
            this.btn_selGDBFolder.UseVisualStyleBackColor = true;
            this.btn_selGDBFolder.Click += new System.EventHandler(this.btn_selGDBFolder_Click);
            // 
            // txt_newGDBFolder
            // 
            this.txt_newGDBFolder.Location = new System.Drawing.Point(79, 15);
            this.txt_newGDBFolder.Name = "txt_newGDBFolder";
            this.txt_newGDBFolder.Size = new System.Drawing.Size(553, 21);
            this.txt_newGDBFolder.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "所在文件夹";
            // 
            // btn_cancel_GDB
            // 
            this.btn_cancel_GDB.Location = new System.Drawing.Point(610, 62);
            this.btn_cancel_GDB.Name = "btn_cancel_GDB";
            this.btn_cancel_GDB.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel_GDB.TabIndex = 6;
            this.btn_cancel_GDB.Text = "取消";
            this.btn_cancel_GDB.UseVisualStyleBackColor = true;
            this.btn_cancel_GDB.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_OK_GDB
            // 
            this.btn_OK_GDB.Location = new System.Drawing.Point(529, 62);
            this.btn_OK_GDB.Name = "btn_OK_GDB";
            this.btn_OK_GDB.Size = new System.Drawing.Size(75, 23);
            this.btn_OK_GDB.TabIndex = 5;
            this.btn_OK_GDB.Text = "确定";
            this.btn_OK_GDB.UseVisualStyleBackColor = true;
            this.btn_OK_GDB.Click += new System.EventHandler(this.btn_OK_GDB_Click);
            // 
            // btn_newGDB
            // 
            this.btn_newGDB.Location = new System.Drawing.Point(72, 62);
            this.btn_newGDB.Name = "btn_newGDB";
            this.btn_newGDB.Size = new System.Drawing.Size(75, 23);
            this.btn_newGDB.TabIndex = 4;
            this.btn_newGDB.Text = "新建GDB";
            this.btn_newGDB.UseVisualStyleBackColor = true;
            this.btn_newGDB.Click += new System.EventHandler(this.btn_newGDB_Click);
            // 
            // btn_selGDB
            // 
            this.btn_selGDB.Location = new System.Drawing.Point(663, 8);
            this.btn_selGDB.Name = "btn_selGDB";
            this.btn_selGDB.Size = new System.Drawing.Size(35, 23);
            this.btn_selGDB.TabIndex = 3;
            this.btn_selGDB.Text = "...";
            this.btn_selGDB.UseVisualStyleBackColor = true;
            this.btn_selGDB.Click += new System.EventHandler(this.btn_selGDB_Click);
            // 
            // txt_gdbPath
            // 
            this.txt_gdbPath.Enabled = false;
            this.txt_gdbPath.Location = new System.Drawing.Point(72, 9);
            this.txt_gdbPath.Name = "txt_gdbPath";
            this.txt_gdbPath.Size = new System.Drawing.Size(591, 21);
            this.txt_gdbPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "gdb路径";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGDB);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(717, 401);
            this.tabControl1.TabIndex = 0;
            // 
            // btn_cancelNewGDB
            // 
            this.btn_cancelNewGDB.Location = new System.Drawing.Point(594, 98);
            this.btn_cancelNewGDB.Name = "btn_cancelNewGDB";
            this.btn_cancelNewGDB.Size = new System.Drawing.Size(75, 23);
            this.btn_cancelNewGDB.TabIndex = 8;
            this.btn_cancelNewGDB.Text = "取消";
            this.btn_cancelNewGDB.UseVisualStyleBackColor = true;
            this.btn_cancelNewGDB.Click += new System.EventHandler(this.btn_cancelNewGDB_Click);
            // 
            // SelectOrCreateWorkspace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 401);
            this.Controls.Add(this.tabControl1);
            this.Name = "SelectOrCreateWorkspace";
            this.Text = "SelectOrCreateWorkspace";
            this.tabGDB.ResumeLayout(false);
            this.tabGDB.PerformLayout();
            this.panel_newGDB.ResumeLayout(false);
            this.panel_newGDB.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_gdbPath;
        private System.Windows.Forms.Button btn_selGDB;
        private System.Windows.Forms.Button btn_newGDB;
        private System.Windows.Forms.Button btn_OK_GDB;
        private System.Windows.Forms.Button btn_cancel_GDB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_newGDBFolder;
        private System.Windows.Forms.Button btn_selGDBFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_newGDBName;
        private System.Windows.Forms.Button btn_createGDB;
        private System.Windows.Forms.Panel panel_newGDB;
        private System.Windows.Forms.TabPage tabGDB;
        private System.Windows.Forms.Button btn_cancelNewGDB;
    }
}