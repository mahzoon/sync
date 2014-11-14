using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using sync.classes;

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

        List<User> cached_users = new List<User>();
        List<Design_Idea> cached_ideas = new List<Design_Idea>();
        List<Activity> cached_activities = new List<Activity>();

        TabPage settings_tab_pointer;
        bool settings_visible = true;

        public MainForm()
        {
            Configurations.LoadSettings();
            InitializeComponent();
            
            progressBar_ls.Style = ProgressBarStyle.Continuous;
            progressBar_ls.MarqueeAnimationSpeed = 0; progressBar_ls.Value = 0;

            settings_tab_pointer = tabPage_settings;
            if (!Configurations.show_other_tabs)
            {
                tabControl_sync.TabPages.Remove(tabPage_manual);
                tabControl_sync.TabPages.Remove(tabPage_settings);
                settings_visible = false;
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
                Thread.Sleep(5000);
                button_ls_auto_Click(null, null);
            }

            ContextMenu tab_menu = new ContextMenu();
            MenuItem show_hide_item = new MenuItem();
            show_hide_item.Text = "Show/Hide Settings";
            show_hide_item.Click += new EventHandler(show_hide_item_Click);
            tab_menu.MenuItems.Add(show_hide_item);
            MenuItem dump_item = new MenuItem();
            dump_item.Text = "Dump";
            dump_item.Click += new EventHandler(dump_item_Click);
            tab_menu.MenuItems.Add(dump_item);
            this.tabControl_sync.ContextMenu = tab_menu;
        }

        void dump_item_Click(object sender, EventArgs e)
        {
            DumpServerDB d = new DumpServerDB();
            d.DumpAccounts();
            d.DumpFeedbacks();
            d.DumpSites();
            d.DumpContexts();
            d.DumpNotesAndMedias();
            MessageBox.Show("Done!");
        }

        void show_hide_item_Click(object sender, EventArgs e)
        {
            if (!settings_visible)
                tabControl_sync.TabPages.Add(tabPage_settings);
            else
                tabControl_sync.TabPages.Remove(tabPage_settings);
            settings_visible = !settings_visible;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (local_to_server != null && (Configurations.sync_interactions_to_server || Configurations.sync_interactions_to_dropbox))
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
            try
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
                        print_results(DateTime.UtcNow.ToString() + " (Server -> Local) No change in users.");
                    else
                        print_results(DateTime.UtcNow.ToString() + " (Server -> Local) " + server_to_local.current_accounts.Count.ToString() + " User(s) synced.");
                }
                result = server_to_local.ProcessWebUserChanges();
                if (!result)
                {
                    if (RESTService.Last_Exception != null)
                        print_results(RESTService.Last_Exception.StackTrace);
                }
                else
                {
                    if (server_to_local.current_webaccounts.Count == 0)
                        print_results(DateTime.UtcNow.ToString() + " (Server -> Local) No change in webusers.");
                    else
                        print_results(DateTime.UtcNow.ToString() + " (Server -> Local) " + server_to_local.current_webaccounts.Count.ToString() + " WebUser(s) synced.");
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
                        print_results(DateTime.UtcNow.ToString() + " (Server -> Local) No change in notes.");
                    else
                        print_results(DateTime.UtcNow.ToString() + " (Server -> Local) " + server_to_local.current_notes.Count.ToString() + " Note(s) synced.");
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
                        print_results(DateTime.UtcNow.ToString() + " (Server -> Local) No change in feedbacks.");
                    else
                        print_results(DateTime.UtcNow.ToString() + " (Server -> Local) " + server_to_local.current_feedbacks.Count.ToString() + " Feedback(s) synced.");
                }
                if (Configurations.sync_interactions_from_server)
                {
                    result = server_to_local.ProcessInteractionLogChanges();
                    if (!result)
                    {
                        if (RESTService.Last_Exception != null)
                            print_results(RESTService.Last_Exception.StackTrace);
                    }
                    else
                    {
                        //if (server_to_local.current_interactions.Count == 0)
                        //    print_results(DateTime.UtcNow.ToString() + " (Server -> Local) No change in interaction logs.");
                        //else
                        //    print_results(DateTime.UtcNow.ToString() + " (Server -> Local) " + server_to_local.current_interactions.Count.ToString() + " Interaction Log File(s) synced.");
                    }
                }
                if (Configurations.sync_interactions_from_dropbox)
                {
                    int result2 = server_to_local.ProcessInteractionLogChanges3();
                    if (result2 == 0)
                        print_results(DateTime.Now.ToString() + " (Server -> Local) " + " No change in interaction logs.");
                    else
                        print_results(DateTime.Now.ToString() + " (Server -> Local) " + result2.ToString() + " Interaction(s) synced.");
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(ex);
                return;
            }
        }

        public void start_local_to_server_sync()
        {
            try
            {
                local_to_server.GetChanges();
                if (local_to_server.changes.Count == 0)
                {
                    print_results(DateTime.UtcNow.ToString() + " (Local -> Server) No change.");
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
                            print_results(DateTime.UtcNow.ToString() + " (Local -> Server) Changes synced.");
                    }
                    else
                    {
                        print_results(DateTime.UtcNow.ToString() + " (Local -> Server) Changes synced.");
                    }
                }
                if (Configurations.sync_interactions_to_server || Configurations.sync_interactions_to_dropbox)
                {
                    int r = local_to_server.ProcessInsertInteractions();
                    print_results(DateTime.UtcNow.ToString() + " (Local -> Server) " + r.ToString() + " Interaction(s) synced.");
                }
                if (Configurations.send_notifications)
                {
                    string r = local_to_server.ProcessSendNotification();
                    print_results(DateTime.UtcNow.ToString() + " (Local -> Server) Sending notification: " + r);
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(ex);
                return;
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

        private void button_refresh_users_Click(object sender, EventArgs e)
        {
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var users = from u in db.Users
                        where u.id > 0
                        select u;
            cached_users.Clear();
            cached_users.AddRange(users);
            comboBox_users.Items.Clear();
            for (int counter = 1; counter < cached_users.Count + 1; counter++)
                comboBox_users.Items.Add(counter.ToString() + " - " + cached_users[counter - 1].name);
        }

        private void comboBox_users_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_users.SelectedIndex < 0) return;
            if (cached_users[comboBox_users.SelectedIndex].affiliation != null)
                this.textBox_affiliation.Text = cached_users[comboBox_users.SelectedIndex].affiliation.ToString();
            else
                this.textBox_affiliation.Text = "";
        }

        private void button_update_affiliation_Click(object sender, EventArgs e)
        {
            if (cached_users.Count == 0)
            {
                MessageBox.Show("Please refresh the users list.");
                return;
            }
            if (comboBox_users.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a user.");
                return;
            }
            int id = cached_users[comboBox_users.SelectedIndex].id;
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var users = from u in db.Users
                        where u.id == id
                        select u;
            if (users.Count() != 1)
            {
                MessageBox.Show("Error. User not found.");
                return;
            }
            User the_user = users.Single<User>();
            the_user.affiliation = textBox_affiliation.Text;
            sync.classes.Action action = new classes.Action();
            action.type_id = 2; action.user_id = 0; action.date = DateTime.UtcNow; action.object_id = the_user.id;
            action.object_type = "nature_net.User"; action.technical_info = "Updating affiliation.";
            db.Actions.InsertOnSubmit(action);
            try { db.SubmitChanges(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error (" + ex.Message + "):\r\n" + ex.StackTrace);
                return;
            }
        }

        private void button_remove_user_Click(object sender, EventArgs e)
        {
            if (cached_users.Count == 0)
            {
                MessageBox.Show("Please refresh the users list.");
                return;
            }
            if (comboBox_users.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a user.");
                return;
            }
            int id = cached_users[comboBox_users.SelectedIndex].id;
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var users = from u in db.Users
                        where u.id == id
                        select u;
            if (users.Count() != 1)
            {
                MessageBox.Show("Error. User not found.");
                return;
            }
            User the_user = users.Single<User>();
            var collections = from c in db.Collections
                              where c.user_id == the_user.id
                              select c;
            if (collections.Count() > 0)
            {
                MessageBox.Show("This user cannot be deleted, because of his/her collections.");
                return;
            }
            var feedbacks = from f in db.Feedbacks
                            where f.user_id == the_user.id
                            select f;
            if (feedbacks.Count() > 0)
            {
                MessageBox.Show("This user cannot be deleted, because of his/her feedbacks.");
                return;
            }
            db.Users.DeleteOnSubmit(the_user);
            sync.classes.Action action = new classes.Action();
            action.type_id = 3; action.user_id = 0; action.date = DateTime.UtcNow;
            action.object_type = "nature_net.User"; action.object_id = the_user.id;
            db.Actions.InsertOnSubmit(action); action.technical_info = the_user.name;
            try { db.SubmitChanges(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error (" + ex.Message + "):\r\n" + ex.StackTrace);
                return;
            }
        }

        private void button_refresh_ideas_Click(object sender, EventArgs e)
        {
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var ideas = from u in db.Design_Ideas
                        select u;
            cached_ideas.Clear();
            cached_ideas.AddRange(ideas);
            comboBox_ideas.Items.Clear();
            for (int counter = 1; counter < cached_ideas.Count + 1; counter++)
                comboBox_ideas.Items.Add(counter.ToString() + " - " + cached_ideas[counter - 1].note);
        }

        private void comboBox_ideas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_ideas.SelectedIndex < 0) return;
            if (cached_ideas[comboBox_ideas.SelectedIndex].status != null)
                this.textBox_status.Text = cached_ideas[comboBox_ideas.SelectedIndex].status.ToString();
            else
                this.textBox_status.Text = "";
        }

        private void button_update_status_Click(object sender, EventArgs e)
        {
            if (cached_ideas.Count == 0)
            {
                MessageBox.Show("Please refresh the ideas list.");
                return;
            }
            if (comboBox_ideas.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an idea.");
                return;
            }
            int id = cached_ideas[comboBox_ideas.SelectedIndex].id;
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var ideas = from u in db.Contributions
                        where u.id == id
                        select u;
            if (ideas.Count() != 1)
            {
                MessageBox.Show("Error. Design idea not found.");
                return;
            }
            Contribution the_idea = ideas.Single<Contribution>();
            the_idea.status = textBox_status.Text;
            sync.classes.Action action = new classes.Action();
            action.type_id = 2; action.user_id = 0; action.date = DateTime.UtcNow; action.object_id = the_idea.id;
            action.object_type = "nature_net.Contribution"; action.technical_info = "Updating status.";
            db.Actions.InsertOnSubmit(action);
            try { db.SubmitChanges(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error (" + ex.Message + "):\r\n" + ex.StackTrace);
                return;
            }
        }

        private void button_remove_idea_Click(object sender, EventArgs e)
        {
            if (cached_ideas.Count == 0)
            {
                MessageBox.Show("Please refresh the ideas list.");
                return;
            }
            if (comboBox_ideas.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an idea.");
                return;
            }
            int id = cached_ideas[comboBox_ideas.SelectedIndex].id;
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var ideas = from u in db.Contributions
                        where u.id == id
                        select u;
            if (ideas.Count() != 1)
            {
                MessageBox.Show("Error. Design idea not found.");
                return;
            }
            Contribution the_idea = ideas.Single<Contribution>();
            var mappings = from m in db.Collection_Contribution_Mappings
                           where m.contribution_id == the_idea.id
                           select m;
            db.Collection_Contribution_Mappings.DeleteAllOnSubmit(mappings);
            db.Contributions.DeleteOnSubmit(the_idea);
            sync.classes.Action action = new classes.Action();
            action.type_id = 3; action.user_id = 0; action.date = DateTime.UtcNow; action.object_id = the_idea.id;
            action.object_type = "nature_net.Contribution"; action.technical_info = the_idea.technical_info;
            db.Actions.InsertOnSubmit(action);
            try { db.SubmitChanges(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error (" + ex.Message + "):\r\n" + ex.StackTrace);
                return;
            }
        }

        private void button_refresh_activities_Click(object sender, EventArgs e)
        {
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var activities = from u in db.Activities
                             select u;
            cached_activities.Clear();
            cached_activities.AddRange(activities);
            comboBox_activities.Items.Clear();
            for (int counter = 1; counter < cached_activities.Count + 1; counter++)
                comboBox_activities.Items.Add(counter.ToString() + " - " + cached_activities[counter - 1].name);
        }

        private void comboBox_activities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_activities.SelectedIndex < 0) return;
            this.textBox_activity_name.Text = cached_activities[comboBox_activities.SelectedIndex].name;
            this.textBox_activity_description.Text = cached_activities[comboBox_activities.SelectedIndex].description;
        }

        private void button_update_activity_Click(object sender, EventArgs e)
        {
            if (cached_activities.Count == 0)
            {
                MessageBox.Show("Please refresh the activities list.");
                return;
            }
            if (comboBox_activities.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an activity.");
                return;
            }
            if (textBox_activity_name.Text=="" || textBox_activity_description.Text =="")
            {
                MessageBox.Show("Please fill the name and description textboxes.");
                return;
            }
            int id = cached_activities[comboBox_activities.SelectedIndex].id;
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var activities = from u in db.Activities
                             where u.id == id
                             select u;
            if (activities.Count() != 1)
            {
                MessageBox.Show("Error. Activity not found.");
                return;
            }
            Activity the_activity = activities.Single<Activity>();
            the_activity.name = textBox_activity_name.Text;
            the_activity.description = textBox_activity_description.Text;
            sync.classes.Action action = new classes.Action();
            action.type_id = 2; action.user_id = 0; action.date = DateTime.UtcNow; action.object_id = the_activity.id;
            action.object_type = "nature_net.Activity"; action.technical_info = "Updating name/desc.";
            db.Actions.InsertOnSubmit(action);
            try { db.SubmitChanges(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error (" + ex.Message + "):\r\n" + ex.StackTrace);
                return;
            }
        }

        private void button_add_activity_Click(object sender, EventArgs e)
        {
            if (textBox_activity_name.Text == "" || textBox_activity_description.Text == "")
            {
                MessageBox.Show("Please fill the name and description textboxes.");
                return;
            }
            Activity a = new Activity();
            a.name = textBox_activity_name.Text;
            a.description = textBox_activity_description.Text;
            a.avatar = ""; a.creation_date = DateTime.UtcNow; a.location_id = 0;
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            db.Activities.InsertOnSubmit(a);
            sync.classes.Action action = new classes.Action();
            action.type_id = 1; action.user_id = 0; action.date = DateTime.UtcNow;
            action.object_type = "nature_net.Activity"; action.technical_info = "Adding an activity.";
            try { db.SubmitChanges(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error (" + ex.Message + "):\r\n" + ex.StackTrace);
                return;
            }
            action.object_id = a.id;
            db.Actions.InsertOnSubmit(action);
            db.SubmitChanges();
        }

        private void button_remove_activity_Click(object sender, EventArgs e)
        {
            if (cached_activities.Count == 0)
            {
                MessageBox.Show("Please refresh the activities list.");
                return;
            }
            if (comboBox_activities.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an activity.");
                return;
            }
            int id = cached_activities[comboBox_activities.SelectedIndex].id;
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var activities = from u in db.Activities
                             where u.id == id
                             select u;
            if (activities.Count() != 1)
            {
                MessageBox.Show("Error. Activity not found.");
                return;
            }
            Activity the_activity = activities.Single<Activity>();
            var collections = from c in db.Collections
                              where c.activity_id == the_activity.id
                              select c;
            if (collections.Count() > 0)
            {
                MessageBox.Show("This activity cannot be deleted, because of the contributions in the activity.");
                return;
            }
            db.Activities.DeleteOnSubmit(the_activity);
            sync.classes.Action action = new classes.Action();
            action.type_id = 3; action.user_id = 0; action.date = DateTime.UtcNow; action.object_id = the_activity.id;
            action.object_type = "nature_net.Activity"; action.technical_info = the_activity.technical_info;
            db.Actions.InsertOnSubmit(action);
            try { db.SubmitChanges(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error (" + ex.Message + "):\r\n" + ex.StackTrace);
                return;
            }
        }
    }

    public delegate void ChangeUIThread_1(bool status);
    public delegate void ChangeUIThread_2(string print);
    public delegate void ChangeUIThread_3(int value);
}
