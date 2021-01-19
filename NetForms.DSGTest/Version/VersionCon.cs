using System;
using System.Collections.Generic;
using System.Text;

namespace NetForms.DSGTest.Version
{
   public  class VersionCon
    {
        public static IVersion Version()
        {
            IVersion version = null;
            switch (NetCommon.AppSettings.Version)
            {
                case VersionV11.Version:
                    version = new VersionV11();
                    break;
                default:
                    version = new VersionV11();
                    break;
            }
            return version;
        }
    }
}
