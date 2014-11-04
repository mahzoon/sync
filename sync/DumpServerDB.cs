using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sync.classes;
using System.IO;

namespace sync
{
    class DumpServerDB
    {
        public ServerDataClasses server_api = new ServerDataClasses();

        public void DumpAccounts()
        {
            string users_text = "";
            List<SAccount> accounts = server_api.GetAccounts();
            foreach (SAccount u in accounts)
                if (u.id != 0)
                    users_text = users_text + get_user_info(u) + "\r\n";
            StreamWriter writer_u = new StreamWriter("accounts.txt");
            writer_u.WriteLine(users_text);
            writer_u.Close();
            users_text = "";
        }

        public void DumpFeedbacks()
        {
            string feedbacks_text = "";
            List<SFeedback> feedbacks = server_api.GetFeedbacks();
            foreach (SFeedback f in feedbacks)
                if (f.id != 0)
                    feedbacks_text = feedbacks_text + get_feedback_info(f) + "\r\n";
            StreamWriter writer_f = new StreamWriter("feedbacks.txt");
            writer_f.WriteLine(feedbacks_text);
            writer_f.Close();
            feedbacks_text = "";
        }

        public void DumpSites()
        {
            string sites_text = "";
            List<SSite> sites = server_api.GetSites();
            foreach (SSite s in sites)
                sites_text = sites_text + get_site_info(s) + "\r\n";
            StreamWriter writer_s = new StreamWriter("sites.txt");
            writer_s.WriteLine(sites_text);
            writer_s.Close();
            sites_text = "";
        }

        public void DumpContexts()
        {
            string contexts_text = "";
            List<SContext> contexts = server_api.GetContexts();
            foreach (SContext c in contexts)
                contexts_text = contexts_text + get_context_info(c) + "\r\n";
            StreamWriter writer_c = new StreamWriter("contexts.txt");
            writer_c.WriteLine(contexts_text);
            writer_c.Close();
            contexts_text = "";
        }

        public void DumpNotesAndMedias()
        {
            string notes_text = "";
            string medias_text = "";
            List<SAccount> accounts = server_api.GetAccounts();
            foreach (SAccount u in accounts)
                if (u.id != 0)
                {
                    List<SNote> notes = server_api.GetNotes(u.username);
                    foreach (SNote n in notes)
                    {
                        notes_text = notes_text + get_note_info(n) + "\r\n";
                        foreach (SMedia m in n.medias)
                            medias_text = medias_text + get_media_info(m, n.id) + "\r\n";
                    }
                }
            
            StreamWriter writer_n = new StreamWriter("notes.txt");
            writer_n.WriteLine(notes_text);
            writer_n.Close();
            notes_text = "";

            StreamWriter writer_m = new StreamWriter("medias.txt");
            writer_m.WriteLine(medias_text);
            writer_m.Close();
            medias_text = "";
        }

        //public void DumpInteractions()
        //{
        //    string interactions_text = "";
        //    TableTopDataClassesDataContext db = Configurations.GetTableTopDB();
        //    var interactions = from u in db.Interaction_Logs
        //                       select u;
        //    foreach (Interaction_Log i in interactions)
        //        interactions_text = interactions_text + get_interaction_info(i) + "\r\n";
        //    StreamWriter writer_i = new StreamWriter("interactions.txt");
        //    writer_i.WriteLine(interactions_text);
        //    writer_i.Close();
        //    interactions_text = "";
        //}

        private string get_user_info(SAccount u)
        {
            string affiliation = "";
            if (u.affiliation != null)
                affiliation = u.affiliation;
            string r = u.id + "," + u.username + "," + u.name + "," + 
                u.consent + "," + u.password + "," + u.email + "," +
                u.created_at + "," + u.modified_at + "," + u.icon_url + "," + affiliation;
            return r;
        }

        private string get_feedback_info(SFeedback f)
        {
            string r = f.id + "," + f.account.id + "," + f.kind.ToLower() + "," +
                f.content + "," + f.target.model.ToLower() + "," + f.target.id + "," +
                f.parent_id + "," + f.created_at + "," + f.modified_at;
            return r;
        }

        private string get_site_info(SSite s)
        {
            string r = s.id + "," + s.name + "," + s.image_url + "," + s.description;
            return r;
        }

        private string get_context_info(SContext c)
        {
            string r = c.id + "," + c.kind + "," + c.name + "," + c.title + "," +
                c.description + "," + c.extras + "," + c.site.id;
            return r;
        }

        private string get_note_info(SNote n)
        {
            string status = "";
            if (n.status != null)
                status = n.status;
            string r = n.id + "," + n.kind + "," + n.content + "," + n.created_at + "," +
                n.modified_at + "," + status + "," + n.longitude + "," + n.latitude + "," +
                n.account.id + "," + n.context.id;
            return r;
        }

        private string get_media_info(SMedia m, int note_id)
        {
            string link = "";
            string link_prefix = "http://res.cloudinary.com/university-of-colorado/image/upload/v1400187706/";
            if (m.link != null)
            {
                link = m.link;
                if (link.StartsWith(link_prefix))
                    link = link.Substring(link_prefix.Length);
            }
            string r = m.id + "," + m.kind + "," + link + "," + m.title + "," +
                m.created_at + "," + note_id;
            return r;
        }

        //private string get_interaction_info(Interaction_Log i)
        //{
        //    string r = i.id + "," + i.type + "," + i.date + "," + i.touch_id + "," +
        //        i.touch_x + "," + i.touch_y + "," + i.details;
        //    return r;
        //}
    }
}
