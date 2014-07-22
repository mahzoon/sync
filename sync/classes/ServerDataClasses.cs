using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sync.classes
{
    class ServerDataClasses
    {
        // API Endpoints

        // Account
        public SAccount GetAccount(string account_username)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("username", account_username);
            SBasic<SAccount> response = RESTService.MakeAndExecuteGetRequest<SAccount>("/api/account/{username}", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public List<SAccount> GetAccounts()
        {
            SBasic<List<SAccount>> response = RESTService.MakeAndExecuteGetRequest<List<SAccount>>("/api/accounts", null);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public SAccount CreateAccount(string account_username, string account_name, string account_password, string account_email, string account_consent)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            Dictionary<string, object> post_params = new Dictionary<string, object>();
            get_params.Add("username", account_username);
            post_params.Add("name", account_name); post_params.Add("password", account_password);
            post_params.Add("email", account_email); post_params.Add("consent", account_consent);
            SBasic<SAccount> response = RESTService.MakeAndExecutePostRequest<SAccount>("/api/account/new/{username}", get_params, post_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public List<SNote> GetNotes(string account_username)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("username", account_username);
            SBasic<List<SNote>> response = RESTService.MakeAndExecuteGetRequest<List<SNote>>("/api/account/{username}/notes", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public List<SFeedback> GetFeedbacksForUser(string account_username)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("username", account_username);
            SBasic<List<SFeedback>> response = RESTService.MakeAndExecuteGetRequest<List<SFeedback>>("/api/account/{username}/feedbacks", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }

        // Note
        public SNote GetNote(string note_id)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("id", note_id);
            SBasic<SNote> response = RESTService.MakeAndExecuteGetRequest<SNote>("/api/note/{id}", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public SNote CreateNote(string account_username, string note_kind, string note_content, string context_name)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            Dictionary<string, object> post_params = new Dictionary<string, object>();
            get_params.Add("username", account_username);
            post_params.Add("kind", note_kind); post_params.Add("content", note_content);
            post_params.Add("context", context_name);
            SBasic<SNote> response = RESTService.MakeAndExecutePostRequest<SNote>("/api/note/new/{username}", get_params, post_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public SMedia AddMedia(string note_id, string note_title, object note_file)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            Dictionary<string, object> post_params = new Dictionary<string, object>();
            get_params.Add("id", note_id);
            post_params.Add("title", note_title); post_params.Add("file", note_file);
            SBasic<SMedia> response = RESTService.MakeAndExecutePostRequest<SMedia>("/api/note/{id}/new/photo", get_params, post_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        //public SBasic UpdateNote(string note_type, string note_description)
        //{

        //}
        public List<SFeedback> GetFeedbacksForNote(string note_id)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("id", note_id);
            SBasic<List<SFeedback>> response = RESTService.MakeAndExecuteGetRequest<List<SFeedback>>("/api/note/{id}/feedbacks", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }


        // Context
        public SContext GetContext(string context_id)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("id", context_id);
            SBasic<SContext> response = RESTService.MakeAndExecuteGetRequest<SContext>("/api/context/{id}", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public List<SContext> GetContexts(string context_type)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("type", context_type);
            SBasic<List<SContext>> response = RESTService.MakeAndExecuteGetRequest<List<SContext>>("/api/context/{type}", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public List<SNote> GetNotes(string context_id, string note_type)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("id", context_id); get_params.Add("type", note_type);
            SBasic<List<SNote>> response = RESTService.MakeAndExecuteGetRequest<List<SNote>>("/api/context/{id}/notes/{type}", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }

        // Site
        public SSite GetSite(string site_name)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("name", site_name);
            SBasic<SSite> response = RESTService.MakeAndExecuteGetRequest<SSite>("/api/site/{name}", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public List<SSite> GetSites()
        {
            SBasic<List<SSite>> response = RESTService.MakeAndExecuteGetRequest<List<SSite>>("/api/sites", null);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public List<SContext> GetContextsForSite(string site_name)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("name", site_name);
            SBasic<List<SContext>> response = RESTService.MakeAndExecuteGetRequest<List<SContext>>("/api/site/{name}/contexts", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }

        // Feedback
        public SFeedback GetFeedback(string feedback_id)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("id", feedback_id);
            SBasic<SFeedback> response = RESTService.MakeAndExecuteGetRequest<SFeedback>("/api/feedback/{id}", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public SFeedback CreateFeedback(string kind, string model, string object_id, string account_username, object feedback_content)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            Dictionary<string, object> post_params = new Dictionary<string, object>();
            get_params.Add("kind", kind); get_params.Add("model", model); get_params.Add("id", object_id); get_params.Add("username", account_username); 
            post_params.Add("content", feedback_content);
            SBasic<SFeedback> response = RESTService.MakeAndExecutePostRequest<SFeedback>("/api/feedback/new/{kind}/for/{model}/{id}/by/{username}", get_params, post_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }

        //Interaction Log
        public string CreateInteractionFile(long start_date, long end_date, string file_name, string file_path)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            Dictionary<string, object> post_params = new Dictionary<string, object>();
            get_params.Add("kind", "tabletop"); get_params.Add("site", Configurations.site_name);
            post_params.Add("start", start_date); post_params.Add("end", end_date);
            string response = RESTService.MakeAndExecutePostRequestWithFile("/api/log/new/{kind}/at/{site}", get_params, post_params, file_name, file_path);
            return response;
        }
        public List<string> GetInteractionLogFiles(long from, long to)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("datetime_from", from.ToString()); get_params.Add("datetime_to", to.ToString()); get_params.Add("site", Configurations.site_name);
            SBasic<List<String>> response;
            response = RESTService.MakeAndExecuteGetRequest<List<String>>("/api/log/from/{datetime_from}/to/{datetime_to}/at/{site}" + Configurations.site_name, get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }

        // Sync
        public List<SAccount> GetAccountsCreatedSince(string year, string month, string day, string hour, string minute, bool use_site_name)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("year", year); get_params.Add("month", month); get_params.Add("day", day);
            get_params.Add("hour", hour); get_params.Add("minute", minute);
            SBasic<List<SAccount>> response;
            if (use_site_name)
                response = RESTService.MakeAndExecuteGetRequest<List<SAccount>>("/api/sync/accounts/created/since/{year}/{month}/{day}/{hour}/{minute}/at/" + Configurations.site_name, get_params);
            else
                response = RESTService.MakeAndExecuteGetRequest<List<SAccount>>("/api/sync/accounts/created/since/{year}/{month}/{day}/{hour}/{minute}", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public List<SNote> GetNotesCreatedSince(string year, string month, string day, string hour, string minute, bool use_site_name)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("year", year); get_params.Add("month", month); get_params.Add("day", day);
            get_params.Add("hour", hour); get_params.Add("minute", minute);
            SBasic<List<SNote>> response;
            if (use_site_name)
                response = RESTService.MakeAndExecuteGetRequest<List<SNote>>("/api/sync/notes/created/since/{year}/{month}/{day}/{hour}/{minute}/at/" + Configurations.site_name, get_params);
            else
                response = RESTService.MakeAndExecuteGetRequest<List<SNote>>("/api/sync/notes/created/since/{year}/{month}/{day}/{hour}/{minute}", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public List<SFeedback> GetFeedbacksCreatedSince(string year, string month, string day, string hour, string minute, bool use_site_name)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("year", year); get_params.Add("month", month); get_params.Add("day", day);
            get_params.Add("hour", hour); get_params.Add("minute", minute);
            SBasic<List<SFeedback>> response;
            if (use_site_name)
                response = RESTService.MakeAndExecuteGetRequest<List<SFeedback>>("/api/sync/feedbacks/created/since/{year}/{month}/{day}/{hour}/{minute}/at/" + Configurations.site_name, get_params);
            else
                response = RESTService.MakeAndExecuteGetRequest<List<SFeedback>>("/api/sync/feedbacks/created/since/{year}/{month}/{day}/{hour}/{minute}", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public List<SAccount> GetAccountsMostRecent(int n)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("n", n.ToString());
            SBasic<List<SAccount>> response = RESTService.MakeAndExecuteGetRequest<List<SAccount>>("/api/sync/accounts/created/recent/{n}", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
        public List<SNote> GetNotesMostRecent(int n)
        {
            Dictionary<string, string> get_params = new Dictionary<string, string>();
            get_params.Add("n", n.ToString());
            SBasic<List<SNote>> response = RESTService.MakeAndExecuteGetRequest<List<SNote>>("/api/sync/recent/{n}", get_params);
            if (response != null)
                if (response.data != null)
                    return response.data;
            return null;
        }
    }

    class SFeedback
    {
        public SAccount account { get; set; }
        public string content { get; set; }
        public long created_at { get; set; }
        public int id { get; set; }
        public string kind { get; set; }
        public long modified_at { get; set; }
        public STarget target { get; set; }
    }

    class SContext
    {
        public string _model_ { get; set; }
        public string description { get; set; }
        public string extras { get; set; }
        public int id { get; set; }
        public string kind { get; set; }
        public string name { get; set; }
        public SSite site { get; set; }
        public string title { get; set; }
    }

    class SMedia
    {
        public string _model_ { get; set; }
        public long created_at { get; set; }
        public int id { get; set; }
        public string kind { get; set; }
        public string link { get; set; }
        public string title { get; set; }
    }

    class SNote
    {
        public string _model_ { get; set; }
        public SAccount account { get; set; }
        public string content { get; set; }
        public SContext context { get; set; }
        public long created_at { get; set; }
        public List<SLandmarkFeedback> feedbacks { get; set; }
        public int id { get; set; }
        public string kind { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public List<SMedia> medias { get; set; }
        public long modified_at { get; set; }
    }

    class SAccount
    {
        public string _model_ { get; set; }
        public string consent { get; set; }
        public long created_at { get; set; }
        public string email { get; set; }
        public string icon_url { get; set; }
        public int id { get; set; }
        public long modified_at { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string username { get; set; }
    }

    class STarget
    {
        public object data { get; set; }
        public int id { get; set; }
        public string model { get; set; }
    }

    class SSite
    {
        public string _model_ { get; set; }
        public string description { get; set; }
        public int id { get; set; }
        public string image_url { get; set; }
        public string name { get; set; }
    }

    class SLandmarkFeedback
    {
        public SAccount account { get; set; }
        public string content { get; set; }
        public long created_at { get; set; }
        public int id { get; set; }
        public string kind { get; set; }
        public long modified_at { get; set; }
    }

    class SBasic<T>
    {
        public T data { get; set; }
        public int status_code { get; set; }
        public string status_txt { get; set; }
    }
}
