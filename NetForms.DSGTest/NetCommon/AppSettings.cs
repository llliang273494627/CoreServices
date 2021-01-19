using System;
using System.Collections.Generic;
using System.Text;

namespace NetForms.DSGTest.NetCommon
{
    public class AppSettings : BackServices.Common.Helper.HelperAppsettings
    {
        private static AppSettings _settings = null;
        public static AppSettings Init
        {
            get
            {
                if (_settings == null)
                    _settings = new AppSettings();
                return _settings;
            }
        }

        public static string Version { get { return Init.GetNodeValue("Version"); } }
    }
}
