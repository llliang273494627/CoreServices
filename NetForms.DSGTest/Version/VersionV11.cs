using System;
using System.Collections.Generic;
using System.Text;

namespace NetForms.DSGTest.Version
{
    public class VersionV11 : AbsVersion
    {
        private const string _title = "测试";

        public const string Version = "V.1.1.0.0";

        protected override string Title { get => $"{_title} {Version}"; }
    }
}
