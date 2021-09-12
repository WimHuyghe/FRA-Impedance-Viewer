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
    public partial class FormPreferences : Form
    {
        public FormPreferences()
        {
            InitializeComponent();
            checkBoxAbsoluteReactance.DataBindings.Add("Checked", CurrentSettings.Instance, "PlotAbsoluteReactanceValues");
            checkBoxLimitGainMaxZeroDb.DataBindings.Add("Checked", CurrentSettings.Instance, "LimitGainMaxZeroDb");
        }
    }
}
