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
    public partial class StatusVerbosity : Form
    {
        public StatusVerbosity()
        {
            InitializeComponent();

            checkBoxScopeAccessD.DataBindings.Add("Checked", FRA4Picoscope.Instance, "LogScopeAccessDiagnostics");
            checkBoxFRAProgress.DataBindings.Add("Checked", FRA4Picoscope.Instance, "LogFRAProgress");
            checkBoxStepTrialProgress.DataBindings.Add("Checked", FRA4Picoscope.Instance, "LogStepTrailProgress");
            checkBoxSignalGenD.DataBindings.Add("Checked", FRA4Picoscope.Instance, "LogSignalGeneratorDiagnostics");
            checkBoxAutorangeD.DataBindings.Add("Checked", FRA4Picoscope.Instance, "LogAutoRangeDiagnostics");
            checkBoxAdaptiveStimulD.DataBindings.Add("Checked", FRA4Picoscope.Instance, "LogAdapticeStimulusDiagnostics");
            checkBoxSampleProcessingD.DataBindings.Add("Checked", FRA4Picoscope.Instance, "LogSampleProcessingDiagnostics");
            checkBoxDFTD.DataBindings.Add("Checked", FRA4Picoscope.Instance, "LogDFTDiagnostics");
            checkBoxScopePowerEvents.DataBindings.Add("Checked", FRA4Picoscope.Instance, "LogScopePowerEvents");
            checkBoxSaveExportEvents.DataBindings.Add("Checked", FRA4Picoscope.Instance, "LogSaveExportStatus");
            checkBoxFRAWarning.DataBindings.Add("Checked", FRA4Picoscope.Instance, "LogFRAWarnings");
        }
    }
}
