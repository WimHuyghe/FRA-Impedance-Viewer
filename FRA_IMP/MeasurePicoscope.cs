using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WHConsult.Utils.Log4Net;

namespace FRA_IMP
{
    public partial class MeasurePicoscope : Form
    {
        private FRAFileCollection m_FileCollection;
        private ILogService logService;
        private Timer m_PicoscopeTimer;
        private bool m_MeasurementStarted;

        public MeasurePicoscope(FRAFileCollection fileCollection)
        {
            InitializeComponent();
            logService = new FileLogService(typeof(MeasurePicoscope));
            InitForm();
            m_FileCollection = fileCollection;
            m_PicoscopeTimer = new Timer();
            m_PicoscopeTimer.Interval = 1000;
            m_PicoscopeTimer.Tick += M_PicoscopeTimer_Tick;
        }

        private void InitForm()
        {
            // Input DUT
            foreach (PS_CHANNEL channel in Enum.GetValues(typeof(PS_CHANNEL))) comboBoxInputChannel.Items.Add(channel);
            comboBoxInputChannel.DataBindings.Add("SelectedItem", FRA4Picoscope.Instance,"InputChannel");
            foreach (ATTEN_T atten in Enum.GetValues(typeof(ATTEN_T))) comboBoxInputAttenuation.Items.Add(atten);
            comboBoxInputAttenuation.DataBindings.Add("SelectedItem", FRA4Picoscope.Instance, "InputAttenuation");
            foreach (PS_COUPLING coupling in Enum.GetValues(typeof(PS_COUPLING))) comboBoxInputCoupling.Items.Add(coupling);
            comboBoxInputCoupling.DataBindings.Add("SelectedItem", FRA4Picoscope.Instance, "InputCoupling");
            textBoxInputOffset.DataBindings.Add("Text", FRA4Picoscope.Instance, "InputDCOffset");

            // Output DUT
            foreach (PS_CHANNEL channel in Enum.GetValues(typeof(PS_CHANNEL))) comboBoxOutputChannel.Items.Add(channel);
            comboBoxOutputChannel.DataBindings.Add("SelectedItem", FRA4Picoscope.Instance, "OutputChannel");
            foreach (ATTEN_T atten in Enum.GetValues(typeof(ATTEN_T))) comboBoxOutputAttenuation.Items.Add(atten);
            comboBoxOutputAttenuation.DataBindings.Add("SelectedItem", FRA4Picoscope.Instance, "OutputAttenuation");
            foreach (PS_COUPLING coupling in Enum.GetValues(typeof(PS_COUPLING))) comboBoxOutputCoupling.Items.Add(coupling);
            comboBoxOutputCoupling.DataBindings.Add("SelectedItem", FRA4Picoscope.Instance, "OutputCoupling");
            textBoxOutputOffset.DataBindings.Add("Text", FRA4Picoscope.Instance, "OutputDCOffset");

            // Stimulus
            textBoxStimulusAmplitude.DataBindings.Add("Text", FRA4Picoscope.Instance, "InitialStimulus");
            textBoxStimulusOffeset.DataBindings.Add("Text", FRA4Picoscope.Instance, "StimulusOffset");

            // Frequency Sweep
            textBoxStartFrequency.DataBindings.Add("Text", FRA4Picoscope.Instance, "StartFrequencyHz");
            textBoxStopFrequency.DataBindings.Add("Text", FRA4Picoscope.Instance, "StopFrequencyHz");
            textBoxStepsPerDecade.DataBindings.Add("Text", FRA4Picoscope.Instance, "StepsPerDecade");

        }

        private void StartMeasurement()
        {
            string checkSettingsResult = FRA4Picoscope.Instance.CheckSettings();

            if (checkSettingsResult.Equals(""))
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    logService.Debug("Measurement started by user...");
                    FRA4Picoscope.Instance.StartMeasurement();
                }
                catch (Exception ex)
                {
                    EndMeasurement();
                    MessageBox.Show(ex.Message, "Measurement Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m_MeasurementStarted = true;
                m_PicoscopeTimer.Enabled = true;
            }
            else MessageBox.Show(checkSettingsResult, "Incorrect Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);         
        }

        private void CheckMeasurementFinished()
        {
            FRA_STATUS_T status;
            string messageLog;
            try
            {
                logService.Debug("Checking measurement finished..");
                status = FRA4Picoscope.Instance.GetStatus();
                if (checkBoxStatusEanbled.Checked)
                {
                    messageLog = FRA4Picoscope.Instance.GetMeasruementMessageLog();
                    richTextBoxMessageLog.AppendText(messageLog);
                }
            }
            catch (Exception ex)
            {
                EndMeasurement();
                MessageBox.Show(ex.Message, "Measurement Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }        
            if (status != FRA_STATUS_T.FRA_STATUS_IN_PROGRESS) ProcessMeasurementResults();  
        }

        private void ProcessMeasurementResults()
        {
            logService.Debug("Processing measurement results..");
            double[] frequenciesLogHz, gainsDb, phasesDegrees;
            try
            {
                FRA4Picoscope.Instance.GetResults(out frequenciesLogHz, out gainsDb, out phasesDegrees);
            }
            catch (Exception ex)
            {
                EndMeasurement();
                MessageBox.Show(ex.Message, "Measurement Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);             
                return;
            }
            EndMeasurement();
            m_FileCollection.Add(new FRAFile(FRAFileType.FRA4PicoScope, frequenciesLogHz, gainsDb, phasesDegrees, 1000));
            this.Cursor = Cursors.Default;
            this.Close();
        }

        private void AbortMeaurement()
        {
            logService.Debug("Measurement aborted by user...");
            FRA4Picoscope.Instance.AbortMeasurement();
            EndMeasurement();
        }

        private void EndMeasurement()
        {
            m_MeasurementStarted = false;
            m_PicoscopeTimer.Enabled = false;
            this.Cursor = Cursors.Default;
        }

        private void buttonStartMeasurement_Click(object sender, EventArgs e)
        {
            StartMeasurement();
        }

        private void buttonAbortMeasurement_Click(object sender, EventArgs e)
        {
            AbortMeaurement();
        }

        private void M_PicoscopeTimer_Tick(object sender, EventArgs e)
        {
            if (m_MeasurementStarted) CheckMeasurementFinished();
            else m_PicoscopeTimer.Enabled = false;
        }

    }
}
