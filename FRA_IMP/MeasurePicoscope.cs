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
            m_FileCollection = fileCollection;
            m_PicoscopeTimer = new Timer();
            m_PicoscopeTimer.Interval = 1000;
            m_PicoscopeTimer.Tick += M_PicoscopeTimer_Tick;
        }

        private void StartMeasurement()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                logService.Debug("Measurement started by user...");
                FRA4Picoscope.Instance.StartMeasurement(100, 1000000, 50);
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

        private void CheckMeasurementFinished()
        {
            FRA_STATUS_T status;
            string messageLog;
            try
            {
                logService.Debug("Checking measurement finished..");
                status = FRA4Picoscope.Instance.GetStatus();
                messageLog = FRA4Picoscope.Instance.GetMeasruementMessageLog();
            }
            catch (Exception ex)
            {
                EndMeasurement();
                MessageBox.Show(ex.Message, "Measurement Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            richTextBoxMessageLog.AppendText(messageLog);
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
