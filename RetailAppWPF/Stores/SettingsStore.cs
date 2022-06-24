using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailAppWPF.Stores
{
    public class SettingsStore
    {
        public static string AccessToken => Properties.Settings.Default.AccessToken;
        public static string Location => Properties.Settings.Default.Location;
        public static string Environment => Properties.Settings.Default.Environment;
        public static bool PrintEnable = Properties.Settings.Default.PrintEnable;

        public static bool IsReady()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.AccessToken))
                return false;
            if (string.IsNullOrEmpty(Properties.Settings.Default.Location))
                return false;
            if (string.IsNullOrEmpty(Properties.Settings.Default.Environment))
                return false;

            return true;
        }
    }
}
