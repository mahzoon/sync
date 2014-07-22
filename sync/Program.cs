using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace sync
{
    static class Program
    {
        static Random RAND = new Random(Convert.ToInt32(DateTime.Now.Ticks / 10000000000));
        public static int SEED()
        {
            return RAND.Next();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
