﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace NetForms.DSGTest.Version
{
    public abstract class AbsVersion : IVersion
    {
        protected virtual string Title { get; set; }

        public virtual Form FrmMain()
        {
            return new Frms.FrmMain { Text = Title };
        }
    }
}
