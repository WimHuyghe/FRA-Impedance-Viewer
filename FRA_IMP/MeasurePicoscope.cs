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
            m_PicoscopeTimer.Interval = 500;
            m_PicoscopeTimer.Tick += M_PicoscopeTimer_Tick;
            ConnectScope();
        }

        private void InitForm()
        {
            // Input DUT = Output generator
            foreach (PS_CHANNEL channel in Enum.GetValues(typeof(PS_CHANNEL))) comboBoxInputChannel.Items.Add(channel);
            comboBoxInputChannel.DataBindings.Add("SelectedItem", FRA4Picoscope.Instance, "InputDUTChannel");
            foreach (ATTEN_T atten in Enum.GetValues(typeof(ATTEN_T))) comboBoxInputAttenuation.Items.Add(atten);
            comboBoxInputAttenuation.DataBindings.Add("SelectedItem", FRA4Picoscope.Instance, "InputDUTAttenuation");
            foreach (PS_COUPLING coupling in Enum.GetValues(typeof(PS_COUPLING))) comboBoxInputCoupling.Items.Add(coupling);
            comboBoxInputCoupling.DataBindings.Add("SelectedItem", FRA4Picoscope.Instance, "InputDUTCoupling");
            textBoxInputOffset.DataBindings.Add("Text", FRA4Picoscope.Instance, "InputDUTDCOffset");

            // Output DUT
            foreach (PS_CHANNEL channel in Enum.GetValues(typeof(PS_CHANNEL))) comboBoxOutputChannel.Items.Add(channel);
            comboBoxOutputChannel.DataBindings.Add("SelectedItem", FRA4Picoscope.Instance, "OutputDUTChannel");
            foreach (ATTEN_T atten in Enum.GetValues(typeof(ATTEN_T))) comboBoxOutputAttenuation.Items.Add(atten);
            comboBoxOutputAttenuation.DataBindings.Add("SelectedItem", FRA4Picoscope.Instance, "OutputDUTAttenuation");
            foreach (PS_COUPLING coupling in Enum.GetValues(typeof(PS_COUPLING))) comboBoxOutputCoupling.Items.Add(coupling);
            comboBoxOutputCoupling.DataBindings.Add("SelectedItem", FRA4Picoscope.Instance, "OutputDUTCoupling");
            textBoxOutputOffset.DataBindings.Add("Text", FRA4Picoscope.Instance, "OutputDUTDCOffset");

            // Stimulus
            textBoxStimulusAmplitude.DataBindings.Add("Text", FRA4Picoscope.Instance, "InitialStimulus");
            textBoxStimulusOffeset.DataBindings.Add("Text", FRA4Picoscope.Instance, "StimulusOffset");
            textBoxExtraSettlingTime.DataBindings.Add("Text", FRA4Picoscope.Instance, "ExtraSettlingTimeMs");

            // Frequency Sweep
            textBoxStartFrequency.DataBindings.Add("Text", FRA4Picoscope.Instance, "StartFrequencyHz");
            textBoxStopFrequency.DataBindings.Add("Text", FRA4Picoscope.Instance, "StopFrequencyHz");
            textBoxStepsPerDecade.DataBindings.Add("Text", FRA4Picoscope.Instance, "StepsPerDecade");
            foreach (SweepDirection direction in Enum.GetValues(typeof(SweepDirection))) comboBoxSweepDirection.Items.Add(direction);
            comboBoxSweepDirection.DataBindings.Add("SelectedItem", FRA4Picoscope.Instance, "SweepDirection");

            // Test Jig
            textBoxReferenceResistor.DataBindings.Add("Text", FRA4Picoscope.Instance, "TestJigReferenceResistor");

            //  Other
            checkBoxCloseFormWhenFinished.DataBindings.Add("Checked", FRA4Picoscope.Instance, "AutoClosePicoFormWhenFinished");
        }

        private void ConnectScope()
        {
            try
            {
                logService.Debug("Trying to connect scope when form is first shown...");
                FRA4Picoscope.Instance.ConnectPicoscope();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Connecting scope failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartMeasurement()
        {
            this.UseWaitCursor = true;
            string checkSettingsResult = FRA4Picoscope.Instance.CheckSettings(); // check settings already connects to scope, so show wait cursor before
            if (checkSettingsResult.Equals(""))
            {
                try
                {
                    logService.Debug("Measurement started by user...");
                    richTextBoxMessageLog.Clear();
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
            else
            {
                this.UseWaitCursor = false;
                MessageBox.Show(checkSettingsResult, "Incorrect Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckMeasurementFinished()
        {
            FRA_STATUS_T status;            
            try
            {
                logService.Debug("Checking measurement finished..");
                status = FRA4Picoscope.Instance.GetStatus();
                UpdateMessageLog();
            }
            catch (Exception ex)
            {
                EndMeasurement();
                MessageBox.Show(ex.Message, "Measurement Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (status != FRA_STATUS_T.FRA_STATUS_IN_PROGRESS) ProcessMeasurementResults();
        }

        private void UpdateMessageLog()
        {
            string messageLog = FRA4Picoscope.Instance.GetMeasruementMessageLog();
            richTextBoxMessageLog.Focus(); //must be focussed to scroll down automatically
            richTextBoxMessageLog.AppendText(messageLog);
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
            FRAFile measurementResult = new FRAFile(FRAFileType.FRA4PicoScope, frequenciesLogHz, gainsDb, phasesDegrees, FRA4Picoscope.Instance.TestJigReferenceResistor);
            measurementResult.MeasurementConditions = FRA4Picoscope.Instance.MeasurementConditionsSummary();
            m_FileCollection.Add(measurementResult);
            this.Cursor = Cursors.Default;
            if (FRA4Picoscope.Instance.AutoClosePicoFormWhenFinished) this.Close();
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
            UpdateMessageLog();
            this.UseWaitCursor = false;
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

        private void buttonVerbosity_Click(object sender, EventArgs e)
        {
            Form statusVerbosity = new StatusVerbosity();
            statusVerbosity.Show();
        }
    }
}
