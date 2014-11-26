using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sync.classes;
using System.Data.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace sync
{
    class ServerDBManager
    {
		public ServerDataClasses server_api;
        public List<SAccount> current_accounts;
        public List<SWebAccount> current_webaccounts;
        public List<SNote> current_notes;
        public List<SFeedback> current_feedbacks;
        public List<string> current_interactions = new List<string>();

		public bool ProcessUserChanges()
		{
            DateTime save_time = DateTime.UtcNow;
			List<SAccount> accounts = null;
            accounts = server_api.GetAccountsCreatedSince(Configurations.last_change_server_users.Year.ToString(),
                Configurations.last_change_server_users.Month.ToString(), Configurations.last_change_server_users.Day.ToString(),
                Configurations.last_change_server_users.Hour.ToString(), Configurations.last_change_server_users.Minute.ToString(), true);
            if (accounts == null)
                return false;
            else
                this.current_accounts = accounts;
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            for (int counter = 0; counter < accounts.Count; counter++)
            {
                if (accounts[counter].username == "default") continue;
                var ru = from u in db.Users
                         where u.name == accounts[counter].username
                         select u;
                if (ru.Count() == 0)
                {
                    User u_new = new User();
                    u_new.name = accounts[counter].username; u_new.password = accounts[counter].password;
                    u_new.email = accounts[counter].email;
                    u_new.technical_info = accounts[counter].consent;
                    u_new.affiliation = accounts[counter].affiliation;
                    if (accounts[counter].icon_url.StartsWith("nn_"))
                        u_new.avatar = accounts[counter].icon_url;
                    else
                        u_new.avatar = Configurations.GetRandomAvatar();
                    db.Users.InsertOnSubmit(u_new);
                }
                else
                {
                    if (ru.Count() == 1)
                    {
                        User u = ru.Single<User>();
                        u.password = accounts[counter].password; u.affiliation = accounts[counter].affiliation;
                        if (accounts[counter].icon_url.StartsWith("nn_"))
                            u.avatar = accounts[counter].icon_url;
                        u.email = accounts[counter].email;
                        u.technical_info = accounts[counter].consent;
                        db.SubmitChanges();
                    }
                }
            }
            if (accounts.Count == 0)
                return true;
            if (SubmitChangesToLocalDB(db))
            {
                Configurations.last_change_server_users = save_time;
                Configurations.SaveSettings();
                return true;
            }
            return false;
		}

        public bool ProcessWebUserChanges()
        {
            DateTime save_time = DateTime.UtcNow;
            List<SWebAccount> accounts = null;
            accounts = server_api.GetWebAccountsCreatedSince(Configurations.last_change_server_webusers.Year.ToString(),
                Configurations.last_change_server_webusers.Month.ToString(), Configurations.last_change_server_webusers.Day.ToString(),
                Configurations.last_change_server_webusers.Hour.ToString(), Configurations.last_change_server_webusers.Minute.ToString());
            if (accounts == null)
                return false;
            else
                this.current_webaccounts = accounts;
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            for (int counter = 0; counter < accounts.Count; counter++)
            {
                if (accounts[counter].username == "default") continue;
                var ru = from u in db.WebUsers
                         where u.username == accounts[counter].username
                         select u;
                if (ru.Count() == 0)
                {
                    WebUser u_new = new WebUser();
                    u_new.username = accounts[counter].username; u_new.password = accounts[counter].password;
                    u_new.email = accounts[counter].email; u_new.fullname = accounts[counter].name;
                    u_new.technical_info = accounts[counter].consent;
                    u_new.affiliation = accounts[counter].affiliation;
                    if (accounts[counter].icon_url.StartsWith("nn_"))
                        u_new.avatar = accounts[counter].icon_url;
                    else
                        u_new.avatar = Configurations.GetRandomAvatar();
                    try
                    {
                        SAccount related_account = server_api.GetAccount(accounts[counter].account_id);
                        string uname = "default";
                        if (related_account != null)
                            uname = related_account.username;

                        if (uname == "default")
                            u_new.user_id = 0;
                        else
                        {
                            var u = from u0 in db.Users
                                    where u0.name == uname
                                    select u0;
                            User related_user = u.Single<User>();
                            u_new.user_id = related_user.id;
                        }
                    }
                    catch (Exception ex) { Log.WriteErrorLog(ex); }
                    db.WebUsers.InsertOnSubmit(u_new);
                }
                else
                {
                    if (ru.Count() == 1)
                    {
                        WebUser u = ru.Single<WebUser>();
                        u.password = accounts[counter].password; u.affiliation = accounts[counter].affiliation;
                        if (accounts[counter].icon_url.StartsWith("nn_"))
                            u.avatar = accounts[counter].icon_url;
                        u.email = accounts[counter].email;
                        u.technical_info = accounts[counter].consent;
                        try
                        {
                            SAccount related_account = server_api.GetAccount(accounts[counter].account_id);
                            string uname = related_account.username;
                            if (uname == "default")
                                u.user_id = 0;
                            else
                            {
                                var u1 = from u0 in db.Users
                                         where u0.name == uname
                                         select u0;
                                User related_user = u1.Single<User>();
                                u.user_id = related_user.id;
                            }
                        }
                        catch (Exception ex) { Log.WriteErrorLog(ex); }
                        db.SubmitChanges();
                    }
                }
            }
            if (accounts.Count == 0)
                return true;
            if (SubmitChangesToLocalDB(db))
            {
                Configurations.last_change_server_webusers = save_time;
                Configurations.SaveSettings();
                return true;
            }
            return false;
        }

		public bool ProcessContributionChanges()
		{
            DateTime save_time = DateTime.UtcNow;
			List<SNote> notes;
            notes = server_api.GetNotesCreatedSince(Configurations.last_change_server_contributions.Year.ToString(),
                Configurations.last_change_server_contributions.Month.ToString(), Configurations.last_change_server_contributions.Day.ToString(),
                Configurations.last_change_server_contributions.Hour.ToString(), Configurations.last_change_server_contributions.Minute.ToString(), true);

            if (notes == null)
                return false;
            else
                this.current_notes = notes;
            bool submit_changes = true;
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            for (int counter = 0; counter < notes.Count; counter++)
            {
                var cntribs = from c in db.Contributions
                              where c.technical_info == notes[counter].id.ToString()
                              select c;
                if (cntribs.Count() > 0)
                {
                    // existing contribution -- update it
                    Contribution c1 = cntribs.First<Contribution>();
                    c1.modified_date = (Configurations.unix_epoch.AddMilliseconds(notes[counter].modified_at)).ToLocalTime();
                    c1.location_id = find_location_id(notes[counter]);
                    c1.note = notes[counter].content;
                    c1.status = notes[counter].status;
                    c1.web_username = notes[counter].webusername;
                    if (notes[counter].kind == "DesignIdea")
                        c1.tags = "Design Idea";
                    else
                    {
                        if (notes[counter].medias != null && notes[counter].medias.Count > 0)
                        {
                            c1.tags = notes[counter].medias[0].kind;
                            c1.media_url = notes[counter].medias[0].link;
                        }
                    }
                    int a_id = 0; // free observation
                    if (notes[counter].context.name.Substring(Configurations.GetSiteNameForServer().Count() + 1) == "design_idea")
                        a_id = 1; // design idea
                    var activities0 = from a in db.Activities
                                     where a.name == notes[counter].context.title
                                     select a;
                    if (activities0.Count() > 0)
                        a_id = activities0.First<Activity>().id;
                    c1.technical_info = notes[counter].id.ToString();
                    db.SubmitChanges();
                    //if (a_id != 1)
                        update_or_create_collection(db, c1.id, a_id, c1.date);
                    // updating the user
                    string new_username = notes[counter].account.username;
                    int new_user_id = 0;
                    var usrs = from u in db.Users
                               where u.name == new_username
                               select u;
                    if (usrs.Count() == 1) new_user_id = usrs.Single<User>().id;
                    this.update_user_for_contribution(db, c1.id, new_user_id, c1.date);
                    continue;
                }
                string username = notes[counter].account.username;
				DateTime note_date = (Configurations.unix_epoch.AddMilliseconds(notes[counter].created_at)).ToLocalTime();
                DateTime note_modified_date = note_date;
                if (notes[counter].modified_at != null && notes[counter].modified_at > 0)
                    note_modified_date = (Configurations.unix_epoch.AddMilliseconds(notes[counter].modified_at)).ToLocalTime();
                //DateTime note_date= DateTime.FromOADate(notes[counter].created_at
				//int activity_id = notes[counter].context.??
				int activity_id = 0; // free observation
				//int activity_id = 1; // design idea
				//int activity_id = other; // other activities
                if (notes[counter].context.name.Substring(Configurations.GetSiteNameForServer().Count() + 1) == "design_idea")
                    activity_id = 1;

                var activities = from a in db.Activities
                                 where a.name == notes[counter].context.title
                                 select a;
                if (activities.Count() > 0)
                    activity_id = activities.First<Activity>().id;
				int col_id = this.get_or_create_collection(db, username, activity_id, note_date);
                if (col_id == -1) continue;
                foreach (SMedia media in notes[counter].medias)
                {
                    bool could_create = create_contribution(notes[counter], media, note_date, note_modified_date, notes[counter].content, notes[counter].id.ToString(), (notes[counter].kind == "DesignIdea"), col_id, notes[counter].status, db);
                    if (!could_create)
                        submit_changes = false;
                }
                if (notes[counter].medias == null)
                    notes[counter].medias = new List<SMedia>();
                if (notes[counter].medias.Count == 0)
                {
                    bool could_create = create_contribution(notes[counter], null, note_date, note_modified_date, notes[counter].content, notes[counter].id.ToString(), (notes[counter].kind == "DesignIdea"), col_id, notes[counter].status, db);
                    if (!could_create)
                        submit_changes = false;
                }
            }
            if (notes.Count == 0)
                return true;
            if (submit_changes)
            {
                Configurations.last_change_server_contributions = save_time;
                Configurations.SaveSettings();
                return true;
            }
            return false;
		}

        public bool ProcessFeedbackChanges()
        {
            DateTime save_time = DateTime.UtcNow;
            List<SFeedback> feedbacks = server_api.GetFeedbacksCreatedSince(Configurations.last_change_server_feedbacks.Year.ToString(),
                Configurations.last_change_server_feedbacks.Month.ToString(), Configurations.last_change_server_feedbacks.Day.ToString(),
                Configurations.last_change_server_feedbacks.Hour.ToString(), Configurations.last_change_server_feedbacks.Minute.ToString(), true);

            if (feedbacks == null)
                return false;
            else
                current_feedbacks = feedbacks;
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            bool all_fine = true;
            for (int counter = 0; counter < feedbacks.Count; counter++)
            {
                if (feedbacks[counter].kind == "Landmark")
                    continue;
                var fdbacks = from f in db.Feedbacks
                              where f.technical_info == feedbacks[counter].id.ToString()
                              select f;
                if (fdbacks.Count() > 1) continue;
                if (fdbacks.Count() == 1)
                {
                    // update feedback
                    Feedback f = fdbacks.Single<Feedback>();
                    f.note = feedbacks[counter].content;
                    f.web_username = feedbacks[counter].webusername;
                    //f.date = (Configurations.unix_epoch.AddMilliseconds(feedbacks[counter].modified_at)).ToLocalTime();
                    var usrs = from u in db.Users
                                where u.name == feedbacks[counter].account.username
                                select u;
                    if (usrs.Count() > 0)
                        f.user_id = usrs.First<User>().id;
                    else
                        f.user_id = 0;
                    db.SubmitChanges();
                    continue;
                }

                Feedback fb = new Feedback();
                fb.date = (Configurations.unix_epoch.AddMilliseconds(feedbacks[counter].created_at)).ToLocalTime();
                fb.web_username = feedbacks[counter].webusername;
                if (feedbacks[counter].kind.ToLower() == "comment")
                    fb.type_id = 1; // comment
                if (feedbacks[counter].kind.ToLower() == "like")
                    fb.type_id = 2; // like
                if (feedbacks[counter].kind.ToLower() == "rating")
                    fb.type_id = 3; // rating

                if (feedbacks[counter].kind.ToLower() == "like")
                    fb.note = "true";
                else
                    fb.note = feedbacks[counter].content;

                // find local user id
                var users = from u in db.Users
                            where u.name == feedbacks[counter].account.username
                            select u;
                if (users.Count() > 0)
                    fb.user_id = users.First<User>().id;
                else
                    fb.user_id = 0;

                fb.parent_id = 0;
                if (feedbacks[counter].parent_id != 0)
                {
                    // find local parent id
                    var parents = from p in db.Feedbacks
                                  where p.technical_info == feedbacks[counter].parent_id.ToString()
                                  select p;
                    if (parents.Count() == 1)
                    {
                        fb.parent_id = parents.Single<Feedback>().id;
                    }
                }

                if (feedbacks[counter].target.model.ToLower() == "note")
                {
                    // search for the object_id in local database
                    var cntribs = from c in db.Contributions
                                  where c.technical_info == feedbacks[counter].target.id.ToString()
                                  select c;
                    if (cntribs.Count() > 0)
                    {
                        fb.object_id = cntribs.First<Contribution>().id;
                        fb.object_type = "nature_net.Contribution";
                    }
                }
                if (feedbacks[counter].target.model.ToLower() == "account")
                {
                    string username = find_account(feedbacks[counter].target.id);
                    // search for the object_id in local database
                    var cntribs = from c in db.Users
                                  where c.name == username
                                  select c;
                    if (cntribs.Count() > 0)
                    {
                        fb.object_id = cntribs.First<User>().id;
                        fb.object_type = "nature_net.User";
                    }
                }
                if (feedbacks[counter].target.model.ToLower() == "context")
                {
                    string context_name = find_context(feedbacks[counter].target.id);
                    // search for the object_id in local database
                    if (context_name.StartsWith(Configurations.GetSiteNameForServer() + "_landmark"))
                    {
                        //search for locations
                        var cntribs = from c in db.Locations
                                      where c.id == Convert.ToInt32(context_name.Substring(Configurations.GetSiteNameForServer().Length + "_landmark".Length))
                                      select c;
                        if (cntribs.Count() > 0)
                        {
                            fb.object_id = cntribs.First<Location>().id;
                            fb.object_type = "nature_net.Location";
                        }
                    }
                    else
                    {
                        //search for activities
                        var cntribs = from c in db.Activities
                                      where c.name == context_name
                                      select c;
                        if (cntribs.Count() > 0)
                        {
                            fb.object_id = cntribs.First<Activity>().id;
                            fb.object_type = "nature_net.Activity";
                        }
                    }
                }

                fb.technical_info = feedbacks[counter].id.ToString();
                //fb.technical_info = "";

                db.Feedbacks.InsertOnSubmit(fb);
                if (!SubmitChangesToLocalDB(db))
                {
                    all_fine = false;
                    return false;
                }
            }
            if (feedbacks.Count == 0)
                return true;
            if (all_fine)
            {
                Configurations.last_change_server_feedbacks = save_time;
                Configurations.SaveSettings();
                return true;
            }
            return false;
        }

        public bool ProcessInteractionLogChanges()
        {
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            List<SInteractionLog> logs;
            logs = server_api.GetInteractionLogs(Configurations.last_change_server_interactions.Year.ToString(),
                Configurations.last_change_server_interactions.Month.ToString(), Configurations.last_change_server_interactions.Day.ToString(),
                Configurations.last_change_server_interactions.Hour.ToString(), Configurations.last_change_server_interactions.Minute.ToString());

            if (logs == null) return false;
            if (logs.Count == 0) return false;
            for (int counter = 0; counter < logs.Count; counter++)
            {
                var existings = from i in db.Interaction_Logs
                                where i.technical_info == logs[counter].id.ToString()
                                select i;
                if (existings.Count() > 0) continue;
                Interaction_Log log = new Interaction_Log();
                log.date = Convert.ToDateTime(logs[counter].date); log.details = logs[counter].details;
                log.touch_id = logs[counter].touch_id; log.touch_x = logs[counter].touch_x; log.touch_y = logs[counter].touch_y;
                log.type = logs[counter].type; log.technical_info = logs[counter].id.ToString();
                db.Interaction_Logs.InsertOnSubmit(log);
                db.SubmitChanges();
            }
            Configurations.last_change_server_interactions = DateTime.UtcNow;
            return true;
        }

        //public bool ProcessInteractionLogChanges2()
        //{
        //    TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
        //    DirectoryInfo info = new DirectoryInfo(Configurations.GetAbsoluteInteractionLogFilePath());
        //    FileInfo[] files = info.GetFiles();
        //    long max_times = 0; current_interactions.Clear();
        //    for (int counter = 0; counter < files.Count(); counter++)
        //    {
        //        long time = 0;
        //        try { time = Convert.ToInt64(files[counter].Name.Substring(2)); }
        //        catch (Exception) { continue; }
        //        if (time > Configurations.last_change_interaction_files)
        //        {
        //            try
        //            {
        //                Stream writer = File.OpenWrite(files[counter].FullName);
        //                writer.Close(); // these two lines are to check if it is being used by another process or not
        //                StreamReader reader = new StreamReader(files[counter].FullName);
        //                string[] contents = reader.ReadToEnd().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        //                reader.Close();
        //                int n = contents.Count();
        //                List<Interaction_Log> new_logs = new List<Interaction_Log>();
        //                for (int counter_c = 0; counter_c < n; counter_c++)
        //                {
        //                    string[] ilog = contents[counter_c].Split(new char[] { '\t' });
        //                    if (ilog.Count() < 6)
        //                    {
        //                        continue;
        //                    }
        //                    Interaction_Log log = new Interaction_Log();
        //                    log.date = Convert.ToDateTime(ilog[0]);
        //                    log.details = ilog[5];
        //                    log.touch_id = Convert.ToInt32(ilog[2]);
        //                    log.touch_x = Convert.ToDouble(ilog[3]);
        //                    log.touch_y = Convert.ToDouble(ilog[4]);
        //                    int log_type_id = (from t in db.Interaction_Types
        //                                       where t.type == ilog[1]
        //                                       select t.id).Single<int>();
        //                    log.type = log_type_id;
        //                    //new_logs.Add(log);
        //                    db.Interaction_Logs.InsertOnSubmit(log);
        //                    if (counter_c == Configurations.max_submit_changes)
        //                        db.SubmitChanges();
        //                }
        //                //db.Interaction_Logs.InsertAllOnSubmit(new_logs);
        //                db.SubmitChanges();
        //            }
        //            catch (Exception e) { Log.WriteErrorLog(e); continue; }
        //            if (max_times < time) max_times = time;
        //            current_interactions.Add(files[counter].Name);
        //        }
        //    }
        //    if (max_times != 0) Configurations.last_change_interaction_files = max_times;
        //    return true;
        //}

        public int ProcessInteractionLogChanges3()
        {
            int result = 0;
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            DirectoryInfo info = new DirectoryInfo(Configurations.GetAbsoluteInteractionLogFilePath());
            FileInfo[] files = info.GetFiles();
            DateTime max_times = Configurations.last_change_interaction_files;
            for (int counter = 0; counter < files.Count(); counter++)
            {
                DateTime time;
                try { time = DateTime.ParseExact(files[counter].Name, "yyMMdd", System.Globalization.CultureInfo.InvariantCulture); }
                catch (Exception) { continue; }
                if (time >= Configurations.last_change_interaction_files)
                {
                    if (time > max_times) max_times = time;
                    StreamReader reader = new StreamReader(files[counter].FullName);
                    while (!reader.EndOfStream)
                    {
                        string ilog = reader.ReadLine();
                        byte[] mem_data = Convert.FromBase64String(ilog);
                        MemoryStream str_mem = new MemoryStream(mem_data);
                        BinaryFormatter bformatter = new BinaryFormatter();
                        Interaction_Log_Serializable i = (Interaction_Log_Serializable)bformatter.Deserialize(str_mem);
                        if (i.id > Configurations.last_change_interaction_files_id)
                        {
                            Interaction_Log i2 = new Interaction_Log();
                            i2.date = i.date; i2.details = i.details; i2.technical_info = i.technical_info;
                            i2.touch_id = i.touch_id; i2.touch_x = i.touch_x; i2.touch_y = i.touch_y; i2.type = i.type;
                            db.Interaction_Logs.InsertOnSubmit(i2);
                            try { db.SubmitChanges(); }
                            catch (Exception ex) { Log.WriteErrorLog(ex); continue; }
                            Configurations.last_change_interaction_files_id = i.id;
                            result++;
                        }
                    }
                    reader.Close();
                }
            }
            Configurations.last_change_interaction_files = max_times;
            return result;
        }

        public bool UpdateLocalActivitites()
        {
            List<SContext> contexts = server_api.GetContextsForSite(Configurations.GetSiteNameForServer());
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            if (contexts != null)
                if (contexts.Count > 0)
                    for (int counter = 0; counter < contexts.Count; counter++)
                        if (contexts[counter].kind == "Activity")
                        {
                            var activities = from a in db.Activities
                                             where a.name == contexts[counter].title
                                             select a;
                            if (activities.Count() == 0)
                            {
                                //create the activity
                                Activity a = new Activity(); a.name = contexts[counter].title; a.location_id = 0;
                                a.description = contexts[counter].description; a.creation_date = DateTime.UtcNow;
                                a.technical_info = contexts[counter].id.ToString();
                                a.avatar = contexts[counter].extras;
                                db.Activities.InsertOnSubmit(a);
                            }
                            else
                            {
                                //modify it
                                Activity a = activities.First<Activity>();
                                a.description = contexts[counter].description;
                                a.technical_info = contexts[counter].id.ToString();
                                a.avatar = contexts[counter].extras;
                            }
                            if (!SubmitChangesToLocalDB(db))
                                return false;
                        }
            return true;
        }

        private bool create_contribution(SNote note, SMedia media, DateTime note_date, DateTime note_mod_date, string note_content, string note_id, bool is_design_idea, int collection_id, string status, TableTopDataClassesDataContext db)
        {
            Contribution c = new Contribution();
            c.date = note_date;
            c.modified_date = note_mod_date;
            c.location_id = find_location_id(note);
            if (media != null)
                c.media_url = media.link;
            else
                c.media_url = "";

            c.note = note_content;
            c.status = status;
            if (is_design_idea)
                c.tags = "Design Idea";
            else
                if (media != null)
                    c.tags = media.kind;
            c.technical_info = note_id;
            c.web_username = note.webusername;
            db.Contributions.InsertOnSubmit(c);
            if (!SubmitChangesToLocalDB(db)) return false;
            //
            Collection_Contribution_Mapping map = new Collection_Contribution_Mapping();
            map.collection_id = collection_id;
            map.contribution_id = c.id;
            map.date = note_date;
            db.Collection_Contribution_Mappings.InsertOnSubmit(map);
            if (!SubmitChangesToLocalDB(db)) return false;
            return true;
        }

		private int get_or_create_collection(TableTopDataClassesDataContext db, string user_name, int activity_id, DateTime dt)
        {
            int user_id =0;
            var ru = from u in db.Users
                     where u.name == user_name
                     select u;
            if (ru.Count() == 0)
            {
                if (user_name.ToLower() != "default")
                    return 0;
            }
            else
                user_id = ru.First<User>().id;

            var r = from c in db.Collections
                    where ((c.user_id == user_id) && c.activity_id == activity_id)
                    orderby c.date descending
                    select c;
            if (r.Count() != 0)
            {
                foreach (Collection col in r)
                {
                    if (Configurations.GetDate_Formatted(col.date) == Configurations.GetDate_Formatted(dt))
                        return col.id;
                }
            }

            // create new collection
            Collection cl = new Collection();
            cl.activity_id = activity_id;
            cl.date = dt;
            cl.name = Configurations.GetDate_Formatted(dt);
            cl.user_id = user_id;
            db.Collections.InsertOnSubmit(cl);
            if (!SubmitChangesToLocalDB(db)) return -1;
            return cl.id;
        }

        private void update_user_for_contribution(TableTopDataClassesDataContext db, int contribution_id, int new_user_id, DateTime dt)
        {
            var mapping = from m in db.Collection_Contribution_Mappings
                          where m.contribution_id == contribution_id
                          select m;
            if (mapping.Count() == 0) return;
            Collection_Contribution_Mapping c1 = mapping.First<Collection_Contribution_Mapping>();
            if (mapping.Count() == 1)
            {
                var colls = from c in db.Collections
                            where c.id == c1.collection_id
                            select c;
                if (colls.Count() == 1)
                {
                    Collection c2 = colls.First<Collection>();
                    c2.user_id = new_user_id;
                    db.SubmitChanges();
                }
                //var mapping2 = from m in db.Collection_Contribution_Mappings
                //               where m.collection_id == c1.collection_id
                //               select m;
                //if (mapping2.Count() != 1)
                //    should_create_collection = true;
            }
        }

        private void update_or_create_collection(TableTopDataClassesDataContext db, int contribution_id, int new_activity_id, DateTime dt)
        {
            bool should_create_collection = false;

            var mapping = from m in db.Collection_Contribution_Mappings
                          where m.contribution_id == contribution_id
                          select m;
            if (mapping.Count() == 0) return;
            Collection_Contribution_Mapping c1 = mapping.First<Collection_Contribution_Mapping>();
            if (mapping.Count() != 1)
                should_create_collection = true;
            else
            {
                var colls = from c in db.Collections
                            where c.id == c1.collection_id
                            select c;
                if (colls.Count() == 1)
                {
                    Collection c2 = colls.First<Collection>();
                    c2.activity_id = new_activity_id;
                    db.SubmitChanges();
                }
                //var mapping2 = from m in db.Collection_Contribution_Mappings
                //               where m.collection_id == c1.collection_id
                //               select m;
                //if (mapping2.Count() != 1)
                //    should_create_collection = true;
            }
            //int user_id = c1.Collection.user_id;
            //if (should_create_collection)
            //{
            //    // create new collection
            //    Collection c2 = new Collection();
            //    c2.activity_id = new_activity_id;
            //    c2.date = dt;
            //    c2.name = Configurations.GetDate_Formatted(dt);
            //    c2.user_id = user_id;
            //    db.Collections.InsertOnSubmit(c2);
            //    db.SubmitChanges();
            //    var mapping_old = from m in db.Collection_Contribution_Mappings
            //                      where m.contribution_id == contribution_id
            //                      select m;
            //    db.Collection_Contribution_Mappings.DeleteAllOnSubmit(mapping_old);
            //    db.SubmitChanges();
            //    Collection_Contribution_Mapping mapping_new = new Collection_Contribution_Mapping();
            //    mapping_new.collection_id = c2.id;
            //    mapping_new.contribution_id = contribution_id;
            //    mapping_new.date = dt;
            //    db.SubmitChanges();
            //}
            //else
            //{
            //    c1.Collection.activity_id = new_activity_id;
            //    db.SubmitChanges();
            //}
        }

        private string find_account(int account_id)
        {
            List<SAccount> accounts = server_api.GetAccounts();
            for (int counter = 0; counter < accounts.Count; counter++)
                if (accounts[counter].id == account_id)
                    return accounts[counter].username;
            return "";
        }

        private string find_context(int context_id)
        {
            List<SContext> contexts = server_api.GetContextsForSite(Configurations.GetSiteNameForServer());
            for (int counter = 0; counter < contexts.Count; counter++)
                if (contexts[counter].id == context_id)
                    if (contexts[counter].kind == "Landmark")
                        return contexts[counter].name;
                    else
                        return contexts[counter].title;
            return "";
        }

        private int find_location_id(SNote note)
        {
            int location_id = 0;
            string landmark = find_landmark(note);
            if (landmark != "")
            {
                try { location_id = Convert.ToInt32(landmark.Substring(Configurations.GetSiteNameForServer().Length + "landmark".Length + 1)); }
                catch (Exception) { location_id = Configurations.FindLocationID(note.latitude, note.longitude); }
            }
            else
                location_id = Configurations.FindLocationID(note.latitude, note.longitude);
            return location_id;
        }

        private string find_landmark(SNote note)
        {
            if (note.feedbacks.Count > 0)
            {
                for (int counter = 0; counter < note.feedbacks.Count; counter++)
                    if (note.feedbacks[counter].kind == "Landmark")
                        return note.feedbacks[counter].content;
            }
            return "";
        }

        private bool SubmitChangesToLocalDB(TableTopDataClassesDataContext db)
        {
            try { db.SubmitChanges(); return true; }
            catch (Exception e) { Log.WriteErrorLog(e); return false; }
        }

    }
}
