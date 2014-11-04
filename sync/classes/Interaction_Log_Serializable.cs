using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace sync.classes
{
    [Serializable()]
    public class Interaction_Log_Serializable: ISerializable
    {
        public long id;
        public DateTime date;
        public int touch_id;
        public double touch_x;
        public double touch_y;
        public int type;
        public string details;
        public string technical_info;

        public Interaction_Log_Serializable()
        {

        }

        public Interaction_Log_Serializable(SerializationInfo info, StreamingContext ct)
        {
            id = (long)info.GetValue("id", typeof(long));
            touch_id = (int)info.GetValue("touch_id", typeof(int));
            touch_x = (double)info.GetValue("touch_x", typeof(double));
            touch_y = (double)info.GetValue("touch_y", typeof(double));
            type = (int)info.GetValue("type", typeof(int));
            date = DateTime.Parse((string)info.GetValue("date", typeof(string)));
            details = (string)info.GetValue("details", typeof(string));
            technical_info = (string)info.GetValue("technical_info", typeof(string));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", id);
            info.AddValue("touch_id", touch_id);
            info.AddValue("touch_x", touch_x);
            info.AddValue("touch_y", touch_y);
            info.AddValue("type", type);
            info.AddValue("date", date);
            info.AddValue("details", details);
            info.AddValue("technical_info", technical_info);
        }
    }
}
