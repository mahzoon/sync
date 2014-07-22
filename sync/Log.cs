using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace sync
{
    class Log
    {
        public static void WriteErrorLog(Exception e)
        {
            try
            {
                StreamReader reader = new StreamReader(Configurations.GetAbsoluteLogFilePath());
                string whole = reader.ReadToEnd();
                reader.Close();

                StreamWriter writer = new StreamWriter(Configurations.GetAbsoluteLogFilePath());
                string error = "--- START ON: " + DateTime.Now.ToString() + " ---\r\nMessage: " + e.Message +
                    "\r\n" + e.StackTrace + "--- END (" + DateTime.Now.ToString() + ") ---\r\n" + whole;
                writer.WriteLine(error);
                writer.Close();
            }
            catch (Exception) { }
        }

        public static void WriteCmdLog(string cmd)
        {
            string time = DateTime.Now.ToString();

            StreamWriter writer = new StreamWriter(Configurations.GetAbsoluteLogCmdFilePath(), true);
            writer.WriteLine("---START: " + time + "---");
            writer.WriteLine(cmd);
            writer.WriteLine("---END: " + time + "---");
            writer.Close();
        }

    }
}
