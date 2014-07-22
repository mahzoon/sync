using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace sync
{
    public partial class MainForm : Form
    {
        TableTopDBManager local_to_server = new TableTopDBManager();
        ServerDBManager server_to_local = new ServerDBManager();

        Thread server_to_local_thread;
        Thread local_to_server_thread;

        System.Threading.Timer server_to_local_timer;
        System.Threading.Timer local_to_server_timer;

        bool server_to_local_started = false;
        bool local_to_server_started = false;

        long server_to_local_time = 0;
        long local_to_server_time = 0;

        public MainForm()
        {
            Configurations.LoadSettings();
            InitializeComponent();
            progressBar_ls.Style = ProgressBarStyle.Continuous;
            progressBar_ls.MarqueeAnimationSpeed = 0; progressBar_ls.Value = 0;

            if (!Configurations.show_other_tabs)
            {
                tabControl_sync.TabPages.Remove(tabPage_manual);
                tabControl_sync.TabPages.Remove(tabPage_settings);
            }

            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
            server_to_local.server_api = new classes.ServerDataClasses();

            server_to_local_thread = new Thread(new ThreadStart(this.start_server_to_local_sync));
            local_to_server_thread = new Thread(new ThreadStart(this.start_local_to_server_sync));

            server_to_local_timer = new System.Threading.Timer(new TimerCallback(this.server_to_local_timer_changed), null, Timeout.Infinite, Configurations.server_to_local_step);
            local_to_server_timer = new System.Threading.Timer(new TimerCallback(this.local_to_server_timer_changed), null, Timeout.Infinite, Configurations.local_to_server_step);

            server_to_local.UpdateLocalActivitites();
            if (Configurations.start_auto)
            {
                button_sl_auto_Click(null, null);
                button_ls_auto_Click(null, null);
            }
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (local_to_server != null && Configurations.sync_interactions_to_dropbox)
                local_to_server.ProcessInsertInteractions_Forced();
            Configurations.SaveSettings();
        }

        private void button_ls_show_Click(object sender, EventArgs e)
        {
            if ((string)comboBox_ls_man.SelectedItem == "GetChanges")
            {
                if (local_to_server.changes != null)
                    local_to_server.changes.Clear();
                local_to_server.GetChanges();
                if (local_to_server.changes.Count == 0)
                    this.textBox_log_manual.Text = "No Change.";
                else
                    this.textBox_log_manual.Text = "Changes: \r\n";
                for(int counter=0;counter<local_to_server.changes.Count;counter++)
                {
                    this.textBox_log_manual.Text = this.textBox_log_manual.Text + GetString(local_to_server.changes[counter]) + "\r\n";
                }
            }
            if ((string)comboBox_ls_man.SelectedItem == "ProcessInsertUser")
            {
                if (local_to_server.changes != null)
                    local_to_server.changes.Clear();
                local_to_server.GetChanges();
                bool has_members = false;
                for (int counter = 0; counter < local_to_server.changes.Count; counter++)
                {
                    if (local_to_server.changes[counter].type_id == 1 && local_to_server.changes[counter].object_type == "nature_net.User")
                    {
                        if (!has_members) this.textBox_log_manual.Text = "Changes: \r\n";
                        this.textBox_log_manual.Text = this.textBox_log_manual.Text + GetString(local_to_server.changes[counter]) + "\r\n";
                        has_members = true;
                    }
                }
                if (!has_members)
                    this.textBox_log_manual.Text = "No Change.";
            }
            if ((string)comboBox_ls_man.SelectedItem == "ProcessInsertDesignIdea")
            {
                if (local_to_server.changes != null)
                    local_to_server.changes.Clear();
                local_to_server.GetChanges();
                bool has_members = false;
                for (int counter = 0; counter < local_to_server.changes.Count; counter++)
                {
                    if (local_to_server.changes[counter].type_id == 1 && local_to_server.changes[counter].object_type == "nature_net.Contribution"
                    && local_to_server.changes[counter].technical_info == "Design Idea")
                    {
                        if (!has_members) this.textBox_log_manual.Text = "Changes: \r\n";
                        this.textBox_log_manual.Text = this.textBox_log_manual.Text + GetString(local_to_server.changes[counter]) + "\r\n";
                        has_members = true;
                    }
                }
                if (!has_members)
                    this.textBox_log_manual.Text = "No Change.";
            }
            if ((string)comboBox_ls_man.SelectedItem == "ProcessInsertFeedback")
            {
                if (local_to_server.changes != null)
                    local_to_server.changes.Clear();
                local_to_server.GetChanges();
                bool has_members = false;
                for (int counter = 0; counter < local_to_server.changes.Count; counter++)
                {
                    if (local_to_server.changes[counter].type_id == 1 && local_to_server.changes[counter].object_type == "nature_net.Feedback")
                    {
                        if (!has_members) this.textBox_log_manual.Text = "Changes: \r\n";
                        this.textBox_log_manual.Text = this.textBox_log_manual.Text + GetString(local_to_server.changes[counter]) + "\r\n";
                        has_members = true;
                    }
                }
                if (!has_members)
                    this.textBox_log_manual.Text = "No Change.";
            }
        }

        private void button_sl_show_Click(object sender, EventArgs e)
        {

        }

        private void button_ls_exe_Click(object sender, EventArgs e)
        {

        }

        private void button_sl_exe_Click(object sender, EventArgs e)
        {

        }

        private void button_ls_auto_Click(object sender, EventArgs e)
        {
            if (local_to_server_started)
            {
                local_to_server_timer.Change(Timeout.Infinite, Timeout.Infinite);
                change_button_image_ls(false);
            }
            else
            {
                local_to_server_timer.Change(0, Configurations.local_to_server_step);
                change_button_image_ls(true);
            }
            local_to_server_started = !local_to_server_started;
        }

        private void button_sl_auto_Click(object sender, EventArgs e)
        {
            if (server_to_local_started)
            {
                server_to_local_timer.Change(Timeout.Infinite, Timeout.Infinite);
                change_button_image_sl(false);
            }
            else
            {
                server_to_local_timer.Change(0, Configurations.server_to_local_step);
                change_button_image_sl(true);
            }
            server_to_local_started = !server_to_local_started;
        }

        public void start_server_to_local_sync()
        {
            bool result = false;
            result = server_to_local.ProcessUserChanges();
            if (!result)
            {
                if (RESTService.Last_Exception != null)
                    print_results(RESTService.Last_Exception.StackTrace);
            }
            else
            {
                if (server_to_local.current_accounts.Count == 0)
                    print_results(DateTime.Now.ToString() + " (Server -> Local) No change in users.");
                else
                    print_results(DateTime.Now.ToString() + " (Server -> Local) " + server_to_local.current_accounts.Count.ToString() + " User(s) synced.");
            }
            result = server_to_local.ProcessContributionChanges();
            if (!result)
            {
                if (RESTService.Last_Exception != null)
                    print_results(RESTService.Last_Exception.StackTrace);
            }
            else
            {
                if (server_to_local.current_notes.Count == 0)
                    print_results(DateTime.Now.ToString() + " (Server -> Local) No change in notes.");
                else
                    print_results(DateTime.Now.ToString() + " (Server -> Local) " + server_to_local.current_notes.Count.ToString() + " Note(s) synced.");
            }
            result = server_to_local.ProcessFeedbackChanges();
            if (!result)
            {
                if (RESTService.Last_Exception != null)
                    print_results(RESTService.Last_Exception.StackTrace);
            }
            else
            {
                if (server_to_local.current_feedbacks.Count == 0)
                    print_results(DateTime.Now.ToString() + " (Server -> Local) No change in feedbacks.");
                else
                    print_results(DateTime.Now.ToString() + " (Server -> Local) " + server_to_local.current_feedbacks.Count.ToString() + " Feedback(s) synced.");
            }
            if (Configurations.sync_interactions_from_dropbox)
            {
                result = server_to_local.ProcessInteractionLogChanges2();
                if (!result)
                {
                    if (RESTService.Last_Exception != null)
                        print_results(RESTService.Last_Exception.StackTrace);
                }
                else
                {
                    if (server_to_local.current_interactions.Count == 0)
                        print_results(DateTime.Now.ToString() + " (Server -> Local) No change in interaction logs.");
                    else
                        print_results(DateTime.Now.ToString() + " (Server -> Local) " + server_to_local.current_interactions.Count.ToString() + " Interaction Log File(s) synced.");
                }
            }
        }

        public void start_local_to_server_sync()
        {
            local_to_server.GetChanges();
            if (local_to_server.changes.Count == 0)
            {
                print_results(DateTime.Now.ToString() + " (Local -> Server) No change.");
            }
            else
            {
                local_to_server.ProcessChanges();
                if (local_to_server.errors != null)
                {
                    string results = "";
                    for (int counter = 0; counter < local_to_server.errors.Count; counter++)
                        results = results + local_to_server.errors[counter].InnerException.StackTrace + "\r\n";
                    if (results != "")
                        print_results(results);
                    else
                        print_results(DateTime.Now.ToString() + " (Local -> Server) Changes synced.");
                }
                else
                {
                    print_results(DateTime.Now.ToString() + " (Local -> Server) Changes synced.");
                }
            }
        }

        void local_to_server_timer_changed(Object stateInfo)
        {
            if (local_to_server_time % Configurations.local_to_server_refresh_rate == 0)
            {
                local_to_server_time = Configurations.local_to_server_step;
                SetProgressBar_1(0);
                restart_local_to_server_thread(stateInfo);
            }
            else
            {
                local_to_server_time = local_to_server_time + Configurations.local_to_server_step;
                SetProgressBar_1((int)((local_to_server_time * 100) / Configurations.local_to_server_refresh_rate));
            }
        }

        void server_to_local_timer_changed(Object stateInfo)
        {
            if (server_to_local_time % Configurations.server_to_local_refresh_rate == 0)
            {
                server_to_local_time = Configurations.server_to_local_step;
                SetProgressBar_2(0);
                restart_server_to_local_thread(stateInfo);
            }
            else
            {
                server_to_local_time = server_to_local_time + Configurations.server_to_local_step;
                SetProgressBar_2((int)((server_to_local_time * 100) / Configurations.server_to_local_refresh_rate));
            }
        }

        void restart_local_to_server_thread(Object stateInfo)
        {
            if (local_to_server_thread != null)
            {
                if (local_to_server_thread.ThreadState == ThreadState.Stopped || local_to_server_thread.ThreadState == ThreadState.Unstarted)
                {
                    local_to_server_thread = new Thread(new ThreadStart(this.start_local_to_server_sync));
                    //change_button_image_ls(true);
                    local_to_server_thread.Start();
                }
            }
            else
            {
                local_to_server_thread = new Thread(new ThreadStart(this.start_local_to_server_sync));
                //change_button_image_ls(true);
                local_to_server_thread.Start();
            }
        }

        void restart_server_to_local_thread(Object stateInfo)
        {
            if (server_to_local_thread != null)
            {
                if (server_to_local_thread.ThreadState == ThreadState.Stopped || server_to_local_thread.ThreadState == ThreadState.Unstarted)
                {
                    server_to_local_thread = new Thread(new ThreadStart(this.start_server_to_local_sync));
                    //change_button_image_sl(true);
                    server_to_local_thread.Start();
                }
            }
            else
            {
                server_to_local_thread = new Thread(new ThreadStart(this.start_server_to_local_sync));
                //change_button_image_sl(true);
                server_to_local_thread.Start();
            }
        }

        private void print_results(string r)
        {
            if (this.textBox_log_auto.InvokeRequired)
            {
                this.textBox_log_auto.Invoke(new ChangeUIThread_2(this.print_results), new object[] { r });
            }
            else
            {
                this.textBox_log_auto.Text = this.textBox_log_auto.Text + r + "\r\n";
                if (textBox_log_auto.Text.Length > Configurations.console_buffer_length)
                {
                    if (Configurations.store_cmd_log_in_file)
                    {
                        Log.WriteCmdLog(textBox_log_auto.Text);
                        textBox_log_auto.Text = "To see previous commands look at " + Configurations.GetAbsoluteLogCmdFilePath() + "\r\n";
                    }
                    else
                    {
                        textBox_log_auto.Text = "";
                    }
                }
            }
        }

        private void change_button_image_sl(bool status)
        {
            if (this.button_sl_auto.InvokeRequired)
            {
                this.button_sl_auto.Invoke(new ChangeUIThread_1(this.change_button_image_sl), new object[] { status });
            }
            else
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
                if (!status)
                    this.button_sl_auto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_sl_auto.BackgroundImage")));
                else
                    this.button_sl_auto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_hidden.BackgroundImage")));
            }
        }

        private void change_button_image_ls(bool status)
        {
            if (this.button_ls_auto.InvokeRequired)
            {
                this.button_ls_auto.Invoke(new ChangeUIThread_1(this.change_button_image_ls), new object[] { status });
            }
            else
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
                if (!status)
                    this.button_ls_auto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_ls_auto.BackgroundImage")));
                else
                    this.button_ls_auto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_hidden.BackgroundImage")));
            }
        }

        private void SetProgressBar_1(int value)
        {
            if (this.progressBar_ls.InvokeRequired)
            {
                this.progressBar_ls.Invoke(new ChangeUIThread_3(this.SetProgressBar_1), new object[] { value });
            }
            else
            {
                this.progressBar_ls.Value = value;
                this.progressBar_ls.Invalidate();
                this.progressBar_ls.Update();
            }
        }

        private void SetProgressBar_2(int value)
        {
            if (this.progressBar_sl.InvokeRequired)
            {
                this.progressBar_sl.Invoke(new ChangeUIThread_3(this.SetProgressBar_2), new object[] { value });
            }
            else
            {
                this.progressBar_sl.Value = value;
                this.progressBar_sl.Invalidate();
                this.progressBar_sl.Update();
            }
        }

        private string GetString(sync.classes.Action a)
        {
            string r = "";
            r = r + a.id.ToString() + " | " + a.date.ToString() + " | " + a.Action_Type.name + " | " +
                "object_type = " + a.object_type + " | " + "object_id = " + a.object_id.ToString() + " | " +
                "username = " + a.User.name + " | " + "technical_info = " + a.technical_info;
            return r;
        }
    }

    public delegate void ChangeUIThread_1(bool status);
    public delegate void ChangeUIThread_2(string print);
    public delegate void ChangeUIThread_3(int value);
}
