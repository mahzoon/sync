namespace sync
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl_sync = new System.Windows.Forms.TabControl();
            this.tabPage_auto = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel_auto = new System.Windows.Forms.TableLayoutPanel();
            this.textBox_log_auto = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_ls_auto = new System.Windows.Forms.Button();
            this.progressBar_ls = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button_sl_auto = new System.Windows.Forms.Button();
            this.progressBar_sl = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage_manual = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel_manual = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox_sl_man = new System.Windows.Forms.ComboBox();
            this.textBox_log_manual = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_ls_exe = new System.Windows.Forms.Button();
            this.button_sl_exe = new System.Windows.Forms.Button();
            this.button_ls_show = new System.Windows.Forms.Button();
            this.button_sl_show = new System.Windows.Forms.Button();
            this.comboBox_ls_man = new System.Windows.Forms.ComboBox();
            this.tabPage_settings = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel_settings = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.button_hidden = new System.Windows.Forms.Button();
            this.tabControl_sync.SuspendLayout();
            this.tabPage_auto.SuspendLayout();
            this.tableLayoutPanel_auto.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage_manual.SuspendLayout();
            this.tableLayoutPanel_manual.SuspendLayout();
            this.tabPage_settings.SuspendLayout();
            this.tableLayoutPanel_settings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_sync
            // 
            this.tabControl_sync.Controls.Add(this.tabPage_auto);
            this.tabControl_sync.Controls.Add(this.tabPage_manual);
            this.tabControl_sync.Controls.Add(this.tabPage_settings);
            this.tabControl_sync.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_sync.Location = new System.Drawing.Point(0, 0);
            this.tabControl_sync.Name = "tabControl_sync";
            this.tabControl_sync.SelectedIndex = 0;
            this.tabControl_sync.Size = new System.Drawing.Size(510, 559);
            this.tabControl_sync.TabIndex = 0;
            // 
            // tabPage_auto
            // 
            this.tabPage_auto.Controls.Add(this.tableLayoutPanel_auto);
            this.tabPage_auto.Location = new System.Drawing.Point(4, 22);
            this.tabPage_auto.Name = "tabPage_auto";
            this.tabPage_auto.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_auto.Size = new System.Drawing.Size(502, 533);
            this.tabPage_auto.TabIndex = 0;
            this.tabPage_auto.Text = "Auto";
            this.tabPage_auto.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel_auto
            // 
            this.tableLayoutPanel_auto.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel_auto.ColumnCount = 1;
            this.tableLayoutPanel_auto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_auto.Controls.Add(this.textBox_log_auto, 0, 2);
            this.tableLayoutPanel_auto.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel_auto.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel_auto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_auto.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel_auto.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel_auto.Name = "tableLayoutPanel_auto";
            this.tableLayoutPanel_auto.RowCount = 3;
            this.tableLayoutPanel_auto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel_auto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel_auto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_auto.Size = new System.Drawing.Size(496, 527);
            this.tableLayoutPanel_auto.TabIndex = 0;
            // 
            // textBox_log_auto
            // 
            this.textBox_log_auto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_log_auto.Location = new System.Drawing.Point(1, 83);
            this.textBox_log_auto.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_log_auto.Multiline = true;
            this.textBox_log_auto.Name = "textBox_log_auto";
            this.textBox_log_auto.ReadOnly = true;
            this.textBox_log_auto.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_log_auto.Size = new System.Drawing.Size(494, 443);
            this.textBox_log_auto.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_ls_auto);
            this.panel1.Controls.Add(this.progressBar_ls);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(494, 40);
            this.panel1.TabIndex = 5;
            // 
            // button_ls_auto
            // 
            this.button_ls_auto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_ls_auto.BackgroundImage")));
            this.button_ls_auto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_ls_auto.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_ls_auto.Location = new System.Drawing.Point(460, 0);
            this.button_ls_auto.Margin = new System.Windows.Forms.Padding(0);
            this.button_ls_auto.Name = "button_ls_auto";
            this.button_ls_auto.Size = new System.Drawing.Size(34, 35);
            this.button_ls_auto.TabIndex = 2;
            this.button_ls_auto.UseVisualStyleBackColor = true;
            this.button_ls_auto.Click += new System.EventHandler(this.button_ls_auto_Click);
            // 
            // progressBar_ls
            // 
            this.progressBar_ls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar_ls.Location = new System.Drawing.Point(0, 35);
            this.progressBar_ls.Margin = new System.Windows.Forms.Padding(0);
            this.progressBar_ls.Name = "progressBar_ls";
            this.progressBar_ls.Size = new System.Drawing.Size(494, 5);
            this.progressBar_ls.Step = 5;
            this.progressBar_ls.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar_ls.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Local -> Server";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button_sl_auto);
            this.panel2.Controls.Add(this.progressBar_sl);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(1, 42);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(494, 40);
            this.panel2.TabIndex = 6;
            // 
            // button_sl_auto
            // 
            this.button_sl_auto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_sl_auto.BackgroundImage")));
            this.button_sl_auto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_sl_auto.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_sl_auto.Location = new System.Drawing.Point(460, 0);
            this.button_sl_auto.Margin = new System.Windows.Forms.Padding(0);
            this.button_sl_auto.Name = "button_sl_auto";
            this.button_sl_auto.Size = new System.Drawing.Size(34, 35);
            this.button_sl_auto.TabIndex = 3;
            this.button_sl_auto.UseVisualStyleBackColor = true;
            this.button_sl_auto.Click += new System.EventHandler(this.button_sl_auto_Click);
            // 
            // progressBar_sl
            // 
            this.progressBar_sl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar_sl.Location = new System.Drawing.Point(0, 35);
            this.progressBar_sl.Margin = new System.Windows.Forms.Padding(0);
            this.progressBar_sl.Name = "progressBar_sl";
            this.progressBar_sl.Size = new System.Drawing.Size(494, 5);
            this.progressBar_sl.Step = 5;
            this.progressBar_sl.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar_sl.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Server -> Local";
            // 
            // tabPage_manual
            // 
            this.tabPage_manual.Controls.Add(this.tableLayoutPanel_manual);
            this.tabPage_manual.Location = new System.Drawing.Point(4, 22);
            this.tabPage_manual.Name = "tabPage_manual";
            this.tabPage_manual.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_manual.Size = new System.Drawing.Size(502, 533);
            this.tabPage_manual.TabIndex = 1;
            this.tabPage_manual.Text = "Manual";
            this.tabPage_manual.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel_manual
            // 
            this.tableLayoutPanel_manual.ColumnCount = 4;
            this.tableLayoutPanel_manual.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel_manual.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_manual.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel_manual.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel_manual.Controls.Add(this.comboBox_sl_man, 1, 1);
            this.tableLayoutPanel_manual.Controls.Add(this.textBox_log_manual, 0, 2);
            this.tableLayoutPanel_manual.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel_manual.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel_manual.Controls.Add(this.button_ls_exe, 3, 0);
            this.tableLayoutPanel_manual.Controls.Add(this.button_sl_exe, 3, 1);
            this.tableLayoutPanel_manual.Controls.Add(this.button_ls_show, 2, 0);
            this.tableLayoutPanel_manual.Controls.Add(this.button_sl_show, 2, 1);
            this.tableLayoutPanel_manual.Controls.Add(this.comboBox_ls_man, 1, 0);
            this.tableLayoutPanel_manual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_manual.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel_manual.Name = "tableLayoutPanel_manual";
            this.tableLayoutPanel_manual.RowCount = 3;
            this.tableLayoutPanel_manual.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel_manual.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel_manual.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_manual.Size = new System.Drawing.Size(496, 527);
            this.tableLayoutPanel_manual.TabIndex = 1;
            // 
            // comboBox_sl_man
            // 
            this.comboBox_sl_man.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_sl_man.FormattingEnabled = true;
            this.comboBox_sl_man.Items.AddRange(new object[] {
            "ProcessUserChanges",
            "ProcessContributionChanges",
            "ProcessFeedbackChanges"});
            this.comboBox_sl_man.Location = new System.Drawing.Point(103, 49);
            this.comboBox_sl_man.Name = "comboBox_sl_man";
            this.comboBox_sl_man.Size = new System.Drawing.Size(310, 21);
            this.comboBox_sl_man.TabIndex = 9;
            // 
            // textBox_log_manual
            // 
            this.tableLayoutPanel_manual.SetColumnSpan(this.textBox_log_manual, 4);
            this.textBox_log_manual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_log_manual.Location = new System.Drawing.Point(3, 83);
            this.textBox_log_manual.Multiline = true;
            this.textBox_log_manual.Name = "textBox_log_manual";
            this.textBox_log_manual.ReadOnly = true;
            this.textBox_log_manual.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_log_manual.Size = new System.Drawing.Size(490, 441);
            this.textBox_log_manual.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Local -> Server";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Server -> Local";
            // 
            // button_ls_exe
            // 
            this.button_ls_exe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ls_exe.Location = new System.Drawing.Point(459, 8);
            this.button_ls_exe.Name = "button_ls_exe";
            this.button_ls_exe.Size = new System.Drawing.Size(34, 23);
            this.button_ls_exe.TabIndex = 4;
            this.button_ls_exe.Text = "Exe";
            this.button_ls_exe.UseVisualStyleBackColor = true;
            this.button_ls_exe.Click += new System.EventHandler(this.button_ls_exe_Click);
            // 
            // button_sl_exe
            // 
            this.button_sl_exe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button_sl_exe.Location = new System.Drawing.Point(459, 48);
            this.button_sl_exe.Name = "button_sl_exe";
            this.button_sl_exe.Size = new System.Drawing.Size(34, 23);
            this.button_sl_exe.TabIndex = 6;
            this.button_sl_exe.Text = "Exe";
            this.button_sl_exe.UseVisualStyleBackColor = true;
            this.button_sl_exe.Click += new System.EventHandler(this.button_sl_exe_Click);
            // 
            // button_ls_show
            // 
            this.button_ls_show.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ls_show.Location = new System.Drawing.Point(419, 8);
            this.button_ls_show.Name = "button_ls_show";
            this.button_ls_show.Size = new System.Drawing.Size(34, 23);
            this.button_ls_show.TabIndex = 3;
            this.button_ls_show.Text = "Res";
            this.button_ls_show.UseVisualStyleBackColor = true;
            this.button_ls_show.Click += new System.EventHandler(this.button_ls_show_Click);
            // 
            // button_sl_show
            // 
            this.button_sl_show.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button_sl_show.Location = new System.Drawing.Point(419, 48);
            this.button_sl_show.Name = "button_sl_show";
            this.button_sl_show.Size = new System.Drawing.Size(34, 23);
            this.button_sl_show.TabIndex = 5;
            this.button_sl_show.Text = "Res";
            this.button_sl_show.UseVisualStyleBackColor = true;
            this.button_sl_show.Click += new System.EventHandler(this.button_sl_show_Click);
            // 
            // comboBox_ls_man
            // 
            this.comboBox_ls_man.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_ls_man.FormattingEnabled = true;
            this.comboBox_ls_man.Items.AddRange(new object[] {
            "GetChanges",
            "ProcessInsertUser",
            "ProcessInsertDesignIdea",
            "ProcessInsertFeedback"});
            this.comboBox_ls_man.Location = new System.Drawing.Point(103, 9);
            this.comboBox_ls_man.Name = "comboBox_ls_man";
            this.comboBox_ls_man.Size = new System.Drawing.Size(310, 21);
            this.comboBox_ls_man.TabIndex = 8;
            // 
            // tabPage_settings
            // 
            this.tabPage_settings.Controls.Add(this.tableLayoutPanel_settings);
            this.tabPage_settings.Location = new System.Drawing.Point(4, 22);
            this.tabPage_settings.Name = "tabPage_settings";
            this.tabPage_settings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_settings.Size = new System.Drawing.Size(502, 533);
            this.tabPage_settings.TabIndex = 2;
            this.tabPage_settings.Text = "Settings";
            this.tabPage_settings.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel_settings
            // 
            this.tableLayoutPanel_settings.ColumnCount = 2;
            this.tableLayoutPanel_settings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel_settings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_settings.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel_settings.Controls.Add(this.button_hidden, 0, 3);
            this.tableLayoutPanel_settings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_settings.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel_settings.Name = "tableLayoutPanel_settings";
            this.tableLayoutPanel_settings.RowCount = 4;
            this.tableLayoutPanel_settings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel_settings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel_settings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_settings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel_settings.Size = new System.Drawing.Size(496, 527);
            this.tableLayoutPanel_settings.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Site name:";
            // 
            // button_hidden
            // 
            this.button_hidden.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_hidden.BackgroundImage")));
            this.button_hidden.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_hidden.Location = new System.Drawing.Point(3, 490);
            this.button_hidden.Name = "button_hidden";
            this.button_hidden.Size = new System.Drawing.Size(75, 23);
            this.button_hidden.TabIndex = 2;
            this.button_hidden.UseVisualStyleBackColor = true;
            this.button_hidden.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 559);
            this.Controls.Add(this.tabControl_sync);
            this.Name = "MainForm";
            this.Text = "Sync";
            this.tabControl_sync.ResumeLayout(false);
            this.tabPage_auto.ResumeLayout(false);
            this.tableLayoutPanel_auto.ResumeLayout(false);
            this.tableLayoutPanel_auto.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPage_manual.ResumeLayout(false);
            this.tableLayoutPanel_manual.ResumeLayout(false);
            this.tableLayoutPanel_manual.PerformLayout();
            this.tabPage_settings.ResumeLayout(false);
            this.tableLayoutPanel_settings.ResumeLayout(false);
            this.tableLayoutPanel_settings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_sync;
        private System.Windows.Forms.TabPage tabPage_auto;
        private System.Windows.Forms.TabPage tabPage_manual;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_auto;
        private System.Windows.Forms.TabPage tabPage_settings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_manual;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_settings;
        private System.Windows.Forms.Button button_ls_auto;
        private System.Windows.Forms.Button button_sl_auto;
        private System.Windows.Forms.TextBox textBox_log_auto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_log_manual;
        private System.Windows.Forms.Button button_ls_exe;
        private System.Windows.Forms.Button button_sl_exe;
        private System.Windows.Forms.Button button_ls_show;
        private System.Windows.Forms.Button button_sl_show;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_hidden;
        private System.Windows.Forms.ComboBox comboBox_sl_man;
        private System.Windows.Forms.ComboBox comboBox_ls_man;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar progressBar_ls;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ProgressBar progressBar_sl;
        private System.Windows.Forms.Label label3;
    }
}

