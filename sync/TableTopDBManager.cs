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
    class TableTopDBManager
    {
		public ServerDataClasses server_api;
		public List<sync.classes.Action> changes;
		public List<Exception> errors;
		
		public TableTopDBManager()
		{
			server_api = new ServerDataClasses();
			errors = new List<Exception>();
		}
		
        public void GetChanges()
        {
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var actions = from a in db.Actions
                          where a.id > Configurations.last_change_tabletop
                          select a;
            if (actions.Count() > 0)
				this.changes = actions.ToList<sync.classes.Action>();
			else
				this.changes = new List<sync.classes.Action>();
        }
		
		public void ProcessChanges()
		{
            for (int counter = 0; counter < changes.Count; counter++)
            {
                if (changes[counter].type_id == 1 && changes[counter].object_type == "nature_net.User")
                    this.ProcessInsertUser(changes[counter].object_id);
                if (changes[counter].type_id == 2 && changes[counter].object_type == "nature_net.User")
                    this.ProcessUpdateUser(changes[counter].object_id);
                if (changes[counter].type_id == 3 && changes[counter].object_type == "nature_net.User")
                    this.ProcessDeleteUser(changes[counter].technical_info);
                if (changes[counter].type_id == 1 && changes[counter].object_type == "nature_net.Contribution"
                    && changes[counter].technical_info == "Design Idea")
                    this.ProcessInsertDesignIdea(changes[counter].object_id);
                if (changes[counter].type_id == 2 && changes[counter].object_type == "nature_net.Contribution")
                    this.ProcessUpdateDesignIdea(changes[counter].object_id);
                if (changes[counter].type_id == 3 && changes[counter].object_type == "nature_net.Contribution")
                    this.ProcessDeleteDesignIdea(changes[counter].technical_info);
                if (changes[counter].type_id == 1 && changes[counter].object_type == "nature_net.Feedback")
                    this.ProcessInsertFeedback(changes[counter].object_id);
                if (changes[counter].type_id == 1 && changes[counter].object_type == "nature_net.Activity")
                    this.ProcessInsertActivity(changes[counter].object_id);
                if (changes[counter].type_id == 2 && changes[counter].object_type == "nature_net.Activity")
                    this.ProcessUpdateActivity(changes[counter].object_id);
                if (changes[counter].type_id == 3 && changes[counter].object_type == "nature_net.Activity")
                    this.ProcessDeleteActivity(changes[counter].technical_info);
                Configurations.last_change_tabletop = changes[counter].id;
                Configurations.SaveSettings();
            }
            //if (Configurations.sync_interactions_to_dropbox)
                //ProcessInsertInteractions();
		}
		
		private void ProcessInsertUser(int user_id)
		{
			TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var users = from u in db.Users
                          where u.id == user_id
                          select u;
            if (users.Count() == 1)
			{
				User us = users.Single<User>();
				server_api.CreateAccount(us.name, us.name, us.password, us.email, us.technical_info, us.affiliation.ToString(), us.avatar);
				if (RESTService.Last_Exception != null)
					this.errors.Add(RESTService.Last_Exception);
			}
		}

        private void ProcessUpdateUser(int user_id)
        {
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var users = from u in db.Users
                        where u.id == user_id
                        select u;
            if (users.Count() == 1)
            {
                User us = users.Single<User>();
                server_api.UpdateAccount(us.name, us.password, us.email, us.technical_info, us.affiliation.ToString(), us.avatar);
                if (RESTService.Last_Exception != null)
                    this.errors.Add(RESTService.Last_Exception);
            }
        }

        private void ProcessDeleteUser(string username)
        {
            server_api.DeleteAccount(username);
            if (RESTService.Last_Exception != null)
                this.errors.Add(RESTService.Last_Exception);
        }
		
		private void ProcessInsertDesignIdea(int id)
		{
			TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var designideas = from d in db.Design_Ideas
                          where d.id == id
                          select d;
            if (designideas.Count() == 1)
			{
                Design_Idea di = designideas.Single<Design_Idea>();
                SNote note = server_api.CreateNote(di.name, "DesignIdea", di.note, Configurations.GetSiteNameForServer() + "_design_idea", di.status.ToString());
                var contrib = from c in db.Contributions
                              where c.id == id
                              select c;
                Contribution c1 = contrib.First<Contribution>();
                c1.technical_info = note.id.ToString();
                db.SubmitChanges();
				if (RESTService.Last_Exception != null)
					this.errors.Add(RESTService.Last_Exception);
			}
		}

        private void ProcessUpdateDesignIdea(int id)
        {
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var designideas = from d in db.Design_Ideas
                              where d.id == id
                              select d;
            if (designideas.Count() == 1)
            {
                Design_Idea di = designideas.Single<Design_Idea>();
                SNote note = server_api.UpdateNote(di.technical_info, di.status.ToString());
                if (RESTService.Last_Exception != null)
                    this.errors.Add(RESTService.Last_Exception);
            }
        }

        private void ProcessDeleteDesignIdea(string note_id)
        {
            SNote note = server_api.DeleteNote(note_id);
            if (RESTService.Last_Exception != null)
                this.errors.Add(RESTService.Last_Exception);
        }

        public void ProcessInsertActivity(int id)
        {
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var activities = from d in db.Activities
                             where d.id == id
                             select d;
            if (activities.Count() == 1)
            {
                Activity di = activities.Single<Activity>();
                SContext context = server_api.AddContext(di.name, di.description);
                if (RESTService.Last_Exception != null)
                    this.errors.Add(RESTService.Last_Exception);
            }
        }

        private void ProcessUpdateActivity(int id)
        {
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var activities = from d in db.Activities
                             where d.id == id
                             select d;
            if (activities.Count() == 1)
            {
                Activity di = activities.Single<Activity>();
                SContext context = server_api.UpdateContext(di.technical_info, di.name, di.description);
                if (RESTService.Last_Exception != null)
                    this.errors.Add(RESTService.Last_Exception);
            }
        }

        private void ProcessDeleteActivity(string context_id)
        {
            SContext context = server_api.DeleteContext(context_id);
            if (RESTService.Last_Exception != null)
                this.errors.Add(RESTService.Last_Exception);
        }

		private void ProcessInsertFeedback(int feedback_id)
		{
			TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var feedbacks = from f in db.Feedbacks
                          where f.id == feedback_id
                          select f;
            if (feedbacks.Count() == 1)
			{
				Feedback fb = feedbacks.Single<Feedback>();
                SFeedback result = null;
                if (fb.object_type == "nature_net.Contribution")	// feedback on contribution (note)
                {
                    var contributions = from c in db.Contributions
                                        where c.id == fb.object_id
                                        select c;
                    if (contributions.Count() == 1)
                    {
                        Contribution cn = contributions.Single<Contribution>();
                        if (fb.parent_id == 0)
                        {
                            if (fb.type_id == 1)	// comment on contribution
                            {
                                result = server_api.CreateFeedback("comment", "note", cn.technical_info, fb.User.name, fb.note, "0");
                            }
                            if (fb.type_id == 2)	// like on contribution (currently design idea)
                            {
                                result = server_api.CreateFeedback("like", "note", cn.technical_info, "default", fb.note, "0");
                            }
                        }
                        else
                        {
                            var fbs = from f in db.Feedbacks
                                      where f.id == fb.parent_id
                                      select f;
                            if (fbs.Count() == 1)
                            {
                                Feedback parent = fbs.Single<Feedback>();
                                if (fb.type_id == 1)
                                    result = server_api.CreateFeedback("comment", "note", cn.technical_info, fb.User.name, fb.note, parent.technical_info);
                            }
                        }
                    }
                }
                if (fb.object_type == "nature_net.User")	// feedback on user (account)
                {
                    var users = from u in db.Users
                                where u.id == fb.object_id
                                select u;
                    if (users.Count() == 1)
                    {
                        User us = users.Single<User>();
                        int account_id = find_account(us.name);
                        if (account_id != -1)
                        {
                            if (fb.parent_id == 0)
                            {
                                if (fb.type_id == 1)    // comment on user
                                {
                                    result = server_api.CreateFeedback("comment", "account", account_id.ToString(), fb.User.name, fb.note, "0");
                                }
                            }
                            else
                            {
                                var fbs = from f in db.Feedbacks
                                          where f.id == fb.parent_id
                                          select f;
                                if (fbs.Count() == 1)
                                {
                                    Feedback parent = fbs.Single<Feedback>();
                                    if (fb.type_id == 1)
                                        result = server_api.CreateFeedback("comment", "account", account_id.ToString(), fb.User.name, fb.note, parent.technical_info);
                                }
                            }
                        }
                    }
                }
                if (fb.object_type == "nature_net.Activity")	// feedback on activity (context)
                {

                    var activities = from a in db.Activities
                                     where a.id == fb.object_id
                                     select a;
                    if (activities.Count() == 1)
                    {
                        Activity ac = activities.Single<Activity>();
                        int context_id = find_activity(ac.name);
                        if (context_id != -1)
                        {
                            if (fb.parent_id == 0)
                            {
                                if (fb.type_id == 1)    // comment on activity
                                {
                                    result = server_api.CreateFeedback("comment", "context", context_id.ToString(), fb.User.name, fb.note, "0");
                                }
                            }
                            else
                            {
                                var fbs = from f in db.Feedbacks
                                          where f.id == fb.parent_id
                                          select f;
                                if (fbs.Count() == 1)
                                {
                                    Feedback parent = fbs.Single<Feedback>();
                                    if (fb.type_id == 1)
                                        result = server_api.CreateFeedback("comment", "context", context_id.ToString(), fb.User.name, fb.note, parent.technical_info);
                                }
                            }
                        }
                    }
                }
                if (fb.object_type == "nature_net.Location")    // feedback on location (context)
                {
                    var locations = from l in db.Locations
                                    where l.id == fb.object_id
                                    select l;
                    if (locations.Count() == 1)
                    {
                        Location lc = locations.Single<Location>();
                        int context_id = find_location(lc.id.ToString());
                        if (context_id != -1)
                        {
                            if (fb.parent_id == 0)
                            {
                                if (fb.type_id == 1)    //comment on location
                                {
                                    result = server_api.CreateFeedback("comment", "context", context_id.ToString(), fb.User.name, fb.note, "0");
                                }
                            }
                            else
                            {
                                var fbs = from f in db.Feedbacks
                                          where f.id == fb.parent_id
                                          select f;
                                if (fbs.Count() == 1)
                                {
                                    Feedback parent = fbs.Single<Feedback>();
                                    if (fb.type_id == 1)
                                        result = server_api.CreateFeedback("comment", "context", context_id.ToString(), fb.User.name, fb.note, parent.technical_info);
                                }
                            }
                        }
                    }
                }

                if (result != null)
                {
                    fb.technical_info = result.id.ToString();
                    db.SubmitChanges();
                }
				if (RESTService.Last_Exception != null)
					this.errors.Add(RESTService.Last_Exception);
			}
		}

        public void ProcessInsertInteractions_Forced()
        {
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var interactions = from i in db.Interaction_Logs
                               where i.id > Configurations.last_interaction_id
                               select i;
            if (interactions.Count() == 0) return;
            CombineAndSendInteractions(interactions.ToList<Interaction_Log>(), db);
        }

        public int ProcessInsertInteractions()
        {
            int result = 0;
            TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
            var interactions = from i in db.Interaction_Logs
                               where i.id > Configurations.last_interaction_id
                               select i;
            if (interactions.Count() > Configurations.max_interaction_size)
            {
                int r = CombineAndSendInteractions(interactions.ToList<Interaction_Log>(), db);
                result = result + r;
            }
            return result;
        }

        private int CombineAndSendInteractions(List<Interaction_Log> logs, TableTopDataClassesDataContext db)
        {
            int r = 0;
            for (int counter = 0; counter < logs.Count; counter++)
            {
                if (Configurations.sync_interactions_to_server)
                {
                    SInteractionLog result = server_api.CreateInteractionRecord(logs[counter].type, logs[counter].details, logs[counter].touch_id,
                        logs[counter].touch_x, logs[counter].touch_y, logs[counter].date.ToString());
                    if (result == null)
                    {
                        if (RESTService.Last_Exception != null)
                        {
                            Log.WriteErrorLog(RESTService.Last_Exception);
                            RESTService.Last_Exception = null;
                        }
                    }
                    else
                    {
                        r++;
                        var existings = from i in db.Interaction_Logs
                                        where i.id == logs[counter].id
                                        select i;
                        Interaction_Log l = existings.First<Interaction_Log>();
                        l.technical_info = result.id.ToString();
                        db.SubmitChanges();
                    }
                }

                if (Configurations.sync_interactions_to_dropbox)
                {
                    //serialize in file
                    try
                    {
                        Interaction_Log_Serializable log = new Interaction_Log_Serializable();
                        log.id = logs[counter].id; log.touch_id = logs[counter].touch_id;
                        log.touch_x = logs[counter].touch_x; log.touch_y = logs[counter].touch_y;
                        log.date = logs[counter].date; log.details = logs[counter].details;
                        log.technical_info = logs[counter].technical_info; log.type = logs[counter].type;
                        MemoryStream str_mem = new MemoryStream();
                        BinaryFormatter bformatter = new BinaryFormatter();
                        bformatter.Serialize(str_mem, log);
                        string ilog = Convert.ToBase64String(str_mem.ToArray());
                        string fname = log.date.Date.ToString("yyMMdd");
                        StreamWriter writer = new StreamWriter(Configurations.GetAbsoluteInteractionLogFilePath() + fname, true);
                        writer.WriteLine(ilog);
                        writer.Close();
                    }
                    catch (Exception ex) { Log.WriteErrorLog(ex); continue; }
                    r++;
                }
            }

            // make a file
            //string fname = Configurations.GetAbsoluteInteractionLogFilePath() + "i_" + Configurations.GetUnixTimestampMillis(logs[0].date).ToString();
            //StreamWriter writer = new StreamWriter(fname);
            //for (int counter = 0; counter < logs.Count; counter++)
            //{
            //    Interaction_Log l = logs[counter];
            //    string ilog = l.date.ToString() + "\t" + l.Interaction_Type.type + "\t" + l.touch_id.ToString() + "\t" +
            //        l.touch_x.ToString() + "\t" + l.touch_y.ToString() + "\t" + l.details.Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ');
            //    writer.WriteLine(ilog);
            //}
            //writer.Close();

            // send the file
            //long start_date = Configurations.GetUnixTimestampMillis(logs[0].date); long end_date = Configurations.GetUnixTimestampMillis(logs[logs.Count - 1].date);
            //string result = server_api.CreateInteractionFile(start_date, end_date, "i_" + Configurations.GetUnixTimestampMillis(logs[0].date), fname);

            // set the last_interaction_id to last one in the list
            // if (result == "OK")

            Configurations.last_interaction_id = logs[logs.Count - 1].id;
            return r;
        }

        public string ProcessSendNotification()
        {
            string r = server_api.SendAliveMessage();
            if (RESTService.Last_Exception != null)
            {
                this.errors.Add(RESTService.Last_Exception);
                return "Unsuccessful.";
            }
            return r;
        }

        private int find_account(string username)
        {
            List<SAccount> accounts = server_api.GetAccounts();
            for (int counter = 0; counter < accounts.Count; counter++)
                if (accounts[counter].username == username)
                    return accounts[counter].id;
            return -1;
        }

        private int find_activity(string activity_name)
        {
            List<SContext> contexts = server_api.GetContextsForSite(Configurations.GetSiteNameForServer());
            for (int counter = 0; counter < contexts.Count; counter++)
                if (contexts[counter].title.ToLower() == activity_name.ToLower() && contexts[counter].kind == "Activity")
                    return contexts[counter].id;
            return -1;
        }

        private int find_location(string location_id)
        {
            List<SContext> contexts = server_api.GetContextsForSite(Configurations.GetSiteNameForServer());
            for (int counter = 0; counter < contexts.Count; counter++)
                if (contexts[counter].name.ToLower() == (Configurations.GetSiteNameForServer() + "_landmark" + location_id) && contexts[counter].kind == "Landmark")
                    return contexts[counter].id;
            return -1;
        }
    }
}
