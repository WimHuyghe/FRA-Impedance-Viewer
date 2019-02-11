using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FRA_IMP
{
    public partial class InfoAbout : Form
    {
        public InfoAbout()
        {
            InitializeComponent();
            InitForm();
            this.TopMost = true;
        }

        public void InitForm()
        {
            // Speaker Report Production Version
            Version version = new Version(Application.ProductVersion);
            labelMajor.Text = version.Major.ToString();
            labelMinor.Text = version.Minor.ToString();
            labelBuild.Text = version.Build.ToString();
            labelRevision.Text = version.Revision.ToString();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
