using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Net;
using sync.classes;

namespace sync
{
    class Configurations
    {
		static string config_file = "config.ini";
        static string log_file = "log.txt";
        static string log_cmd_file = "log_cmd.txt";
        
        public static string interaction_log_absolute_file_path = "logs\\";
        public static string client_url = "http://naturenet.herokuapp.com/";
		public static string site_name = "aces";
        public static DateTime unix_epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long last_change_tabletop = 0;
        public static long last_interaction_id = 0;
        public static int max_interaction_size = 10; // number of records
        public static DateTime last_change_server_users = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime last_change_server_webusers = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime last_change_server_contributions = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime last_change_server_feedbacks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime last_change_server_interactions = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime last_change_interaction_files = new DateTime(1970, 1, 1, 0, 0, 0);
        public static long last_change_interaction_files_id = 0;

        public static int local_to_server_refresh_rate = 40000;
        public static int server_to_local_refresh_rate = 40000;

        public static int local_to_server_step = 200;
        public static int server_to_local_step = 200;

        public static Dictionary<int, float> latitudes = new Dictionary<int, float>();
        public static Dictionary<int, float> longitudes = new Dictionary<int, float>();
        public static float latitude_error = 0.0001f;
        public static float longitude_error = 0.0001f;

        public static List<string> avatar_colors = new List<string>() { "green", "orange", "purple", "red" };
        public static List<string> avatar_animals = new List<string>() { "bear", "bison", "caribou", "frog", "gator", "hare", "horse", "snake", "squirrel", "tortoise", "weasel", "wolf" };
        public static string avatar_prefix = "nn_";
        public static string avatar_extension = ".png";

        public static int console_buffer_length = 4 * 1024;
        static Random rand = new Random(Program.SEED());

        public static bool store_cmd_log_in_file = true;
        public static bool show_other_tabs = false;
        public static bool start_auto = false;
        public static bool sync_interactions_to_dropbox = false;
        public static bool sync_interactions_from_dropbox = false;
        public static bool sync_interactions_to_server = false;
        public static bool sync_interactions_from_server = false;
        public static bool send_notifications = true;

        public static int max_submit_changes = 10000;

		public static string GetAbsolutePath()
        {
            string c = Assembly.GetExecutingAssembly().CodeBase;
            string a = Path.GetFullPath(c.Substring(8));
            string b = (a).Substring(0, a.Length - 8);
            return b;
        }

        public static string GetAbsoluteConfigFilePath()
        {
            string a = Configurations.GetAbsolutePath();
            return Configurations.GetAbsolutePath() + Configurations.config_file;
        }

        public static string GetAbsoluteLogFilePath()
        {
            return Configurations.GetAbsolutePath() + Configurations.log_file;
        }

        public static string GetAbsoluteLogCmdFilePath()
        {
            return Configurations.GetAbsolutePath() + Configurations.log_cmd_file;
        }

        public static string GetAbsoluteInteractionLogFilePath()
        {
            return Configurations.interaction_log_absolute_file_path;
        }

        public static string GetRandomAvatar()
        {
            string color = avatar_colors[rand.Next(avatar_colors.Count)];
            string animal = avatar_animals[rand.Next(avatar_animals.Count)];
            string avatar = avatar_prefix + animal + color + avatar_extension;
            return avatar;
        }
		
		public static void SaveSettings()
		{
			INIParser parser = new INIParser(GetAbsoluteConfigFilePath(), Encoding.UTF8);
            parser.SetValue("Parameters", "last_change_tabletop", last_change_tabletop);
            parser.SetValue("Parameters", "last_interaction_id", last_interaction_id);
            parser.SetValue("Parameters", "last_change_server_users", last_change_server_users);
            parser.SetValue("Parameters", "last_change_server_webusers", last_change_server_webusers);
            parser.SetValue("Parameters", "last_change_server_contributions", last_change_server_contributions);
            parser.SetValue("Parameters", "last_change_server_feedbacks", last_change_server_feedbacks);
            parser.SetValue("Parameters", "last_change_server_interactions", last_change_server_interactions);
            parser.SetValue("Parameters", "last_change_interaction_files", last_change_interaction_files);
            parser.SetValue("Parameters", "last_change_interaction_files_id", last_change_interaction_files_id);
            parser.Save(GetAbsoluteConfigFilePath(), Encoding.UTF8);
		}
		
		public static void LoadSettings()
		{
			INIParser parser = new INIParser(GetAbsoluteConfigFilePath(), Encoding.UTF8);
            client_url = parser.GetValue("Parameters", "client_url", client_url);
            interaction_log_absolute_file_path = parser.GetValue("Parameters", "interaction_log_absolute_file_path", GetAbsolutePath() + interaction_log_absolute_file_path);
			last_change_tabletop = parser.GetValue("Parameters", "last_change_tabletop", last_change_tabletop);
            last_interaction_id = parser.GetValue("Parameters", "last_interaction_id", last_interaction_id);
            last_change_server_users = parser.GetValue("Parameters", "last_change_server_users", last_change_server_users);
            last_change_server_webusers = parser.GetValue("Parameters", "last_change_server_webusers", last_change_server_webusers);
            last_change_server_contributions = parser.GetValue("Parameters", "last_change_server_contributions", last_change_server_contributions);
            last_change_server_feedbacks = parser.GetValue("Parameters", "last_change_server_feedbacks", last_change_server_feedbacks);
            last_change_server_interactions = parser.GetValue("Parameters", "last_change_server_interactions", last_change_server_interactions);
            last_change_interaction_files = parser.GetValue("Parameters", "last_change_interaction_files", last_change_interaction_files);
            last_change_interaction_files_id = parser.GetValue("Parameters", "last_change_interaction_files_id", last_change_interaction_files_id);

            latitudes.Clear(); longitudes.Clear();
            for (int counter = 1; counter < 12; counter++)
            {
                latitudes.Add(counter, parser.GetValue("Locations", "P" + counter.ToString() + "Lat", 0f));
                longitudes.Add(counter, parser.GetValue("Locations", "P" + counter.ToString() + "Long", 0f));
            }
            latitude_error = parser.GetValue("Locations", "latitude_error", latitude_error);
            longitude_error = parser.GetValue("Locations", "longitude_error", longitude_error);

			site_name = parser.GetValue("Parameters", "site_name", site_name);
            server_to_local_refresh_rate = parser.GetValue("Parameters", "server_to_local_refresh_rate", server_to_local_refresh_rate);
            local_to_server_refresh_rate = parser.GetValue("Parameters", "local_to_server_refresh_rate", local_to_server_refresh_rate);
            store_cmd_log_in_file = parser.GetValue("Parameters", "store_cmd_log_in_file", store_cmd_log_in_file);
            show_other_tabs = parser.GetValue("Parameters", "show_other_tabs", show_other_tabs);
            start_auto = parser.GetValue("Parameters", "start_auto", start_auto);
            max_interaction_size = parser.GetValue("Parameters", "max_interaction_size", max_interaction_size);
            sync_interactions_to_server = parser.GetValue("Parameters", "sync_interactions_to_server", sync_interactions_to_server);
            sync_interactions_from_server = parser.GetValue("Parameters", "sync_interactions_from_server", sync_interactions_from_server);
            sync_interactions_to_dropbox = parser.GetValue("Parameters", "sync_interactions_to_dropbox", sync_interactions_to_dropbox);
            sync_interactions_from_dropbox = parser.GetValue("Parameters", "sync_interactions_from_dropbox", sync_interactions_from_dropbox);
            send_notifications = parser.GetValue("Parameters", "send_notifications", send_notifications);
		}

        public static string GetSiteNameForServer()
        {
            if (site_name == "dev")
                return "aces";
            else
                return site_name;
        }
		
		public static string GetDate_Formatted(DateTime dt)
        {
            string r = dt.Day.ToString() + " " + GetMonthName(dt.Month) + " " + dt.Year.ToString();
            return r;
        }

        public static String GetMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "Jan";
                case 2:
                    return "Feb";
                case 3:
                    return "Mar";
                case 4:
                    return "Apr";
                case 5:
                    return "May";
                case 6:
                    return "Jun";
                case 7:
                    return "Jul";
                case 8:
                    return "Aug";
                case 9:
                    return "Sep";
                case 10:
                    return "Oct";
                case 11:
                    return "Nov";
                case 12:
                    return "Dec";
                default:
                    return "Unknown";
            }
        }

        public static int FindLocationID(double latitude, double longitude)
        {
            for (int counter = 1; counter < 12; counter++)
                if (Math.Abs(latitudes[counter] - latitude) < latitude_error && Math.Abs(longitudes[counter] - longitude) < longitude_error)
                    return counter;
            return 0;
        }

        //public static long GetCurrentUnixTimestampMillis()
        //{
            //return (long)(DateTime.UtcNow - unix_epoch).TotalMilliseconds;
        //}

        public static long GetUnixTimestampMillis(DateTime t)
        {
            return (long)(t - unix_epoch).TotalMilliseconds;
        }

        public static DateTime DateTimeFromUnixTimestampMillis(long millis)
        {
            return unix_epoch.AddMilliseconds(millis);
        }

        //public static long GetCurrentUnixTimestampSeconds()
        //{
        //    return (long)(DateTime.UtcNow - unix_epoch).TotalSeconds;
        //}

        //public static DateTime DateTimeFromUnixTimestampSeconds(long seconds)
        //{
        //    return unix_epoch.AddSeconds(seconds);
        //}

        public static bool download_file(string file_name_to_download, string file_name_to_save)
        {
            try
            {
                System.IO.FileStream file_stream = new System.IO.FileStream(Configurations.GetAbsoluteInteractionLogFilePath() + "d_" + file_name_to_save, System.IO.FileMode.CreateNew);
                file_stream.Close();
                WebClient client = new WebClient();
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                client.DownloadFile(file_name_to_download, Configurations.GetAbsoluteInteractionLogFilePath() + "d_" + file_name_to_save);
                return true;
            }
            catch (Exception e)
            {
                Log.WriteErrorLog(e);
                return false;
            }
        }

        public static TableTopDataClassesDataContext GetTableTopDB()
        {
            if (Configurations.site_name == "aces")
                return new TableTopDataClassesDataContext(sync.Properties.Settings.Default.nature_netConnectionString_aces);
            if (Configurations.site_name == "umd")
                return new TableTopDataClassesDataContext(sync.Properties.Settings.Default.nature_netConnectionString_umd);
            if (Configurations.site_name == "uncc")
                return new TableTopDataClassesDataContext(sync.Properties.Settings.Default.nature_netConnectionString_uncc);
            if (Configurations.site_name == "dev")
                return new TableTopDataClassesDataContext(sync.Properties.Settings.Default.nature_netConnectionString_dev);
            return new TableTopDataClassesDataContext();
        }
    }
}
