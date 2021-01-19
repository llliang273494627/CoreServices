using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NetForms.DSGTest
{
    public partial class FrmError : Form
    {
        public FrmError()
        {
            InitializeComponent();
        }

        private static FrmError _frmError = null;

        public static void ShowErrors(List<string> errors)
        {
            if (_frmError == null || !_frmError.Visible)
                _frmError = new FrmError();
            _frmError.lstError.Items.Clear();
            _frmError.lstError.Items.AddRange(errors.ToArray());
            if (!_frmError.Visible)
                _frmError.Show();
        }

        private void FrmError_Load(object sender, EventArgs e)
        {
            lstError.Items.Clear();
        }
    }
}
