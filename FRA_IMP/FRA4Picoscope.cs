using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WHConsult.Utils.Log4Net;
using WHConsult.Utils.Settings;
using WHConsult.Utils.Settings.IniFiles;

namespace FRA_IMP
{
    public class FRA4Picoscope: IDisposable
    {
        private ISettingsManager m_SettingsManager;
        private ILogService logService;
        private bool scopeConnected = false;



        #region Singleton

        static readonly FRA4Picoscope m_Instance = new FRA4Picoscope();

        public static FRA4Picoscope Instance
        {
            get { return m_Instance; }
        }

        private FRA4Picoscope()
        {
            logService = new FileLogService(typeof(FRA4Picoscope));
            logService.Info("Creating new Picoscope...");
            m_SettingsManager = new IniSettingsManager("Picoscope", "Settings");
        }

        #endregion

        private void ConnectPicoscope()
        {
            byte result = FRA4PicoscopeAPI.Initialize();
            if (result != 1) HandlePicoException("Failed to initialize Picoscope API: " + result.ToString());
            else logService.Info("Picoscope API succesfully initialized");
        
            FRA4PicoscopeAPI.SetLogVerbosityFlag(LOG_MESSAGE_FLAGS_T.FRA_PROGRESS, false);
            FRA4PicoscopeAPI.SetLogVerbosityFlags(LOG_MESSAGE_FLAGS_T.FRA_PROGRESS);
            FRA4PicoscopeAPI.EnableMessageLog(true);
            FRA4PicoscopeAPI.AutoClearMessageLog(false);

            result = FRA4PicoscopeAPI.SetScope("");
            if (result != 1) HandlePicoException("Failed to select Picoscope: " + result.ToString());
            else logService.Info("Picoscope succesfully selected");

            scopeConnected = true;
        }

        private void SetUpChannels()
        {
            logService.Info("Setting up Picoscope succesfully channels...");
            logService.Info("Inputchannel:" + InputChannel.ToString() + "   Coupling:" + InputCoupling.ToString() + "   Atten:" + InputAttenuation.ToString() + "   DC Offset:" + InputDCOffset.ToString());
            logService.Info("Outputchannel:" + OutputChannel.ToString() + "   Coupling:" + OutputCoupling.ToString() + "   Atten:" + OutputAttenuation.ToString() + "   DC Offset:" + OutputDCOffset.ToString());
            logService.Info("Initial stimulus:" + InitialStimulus.ToString() + "   Max Stimulis:" + MaxStimulus + "   Stimmulus Offset:" + StimulusOffset.ToString());
            byte result = FRA4PicoscopeAPI.SetupChannels(InputChannel, InputCoupling, InputAttenuation, InputDCOffset, OutputChannel, OutputCoupling, OutputAttenuation, OutputDCOffset,
                InitialStimulus, MaxStimulus, StimulusOffset);
            if (result != 1) HandlePicoException("Failed to setup channels and stimulus: " + result.ToString());
            else logService.Info("Picoscope channels and stimulus succesfully setup");

        }

        public void StartMeasurement(double startFrequencyHz, double stopFrequencyHz, int stepsPerDecade)
        {
            if (!scopeConnected)
            {
                logService.Info("Connecting Picoscope...");
                ConnectPicoscope();        
            }
            logService.Info("Setting up Picoscope channels...");
            SetUpChannels();

            //store settings
            this.StartFrequencyHz = startFrequencyHz;
            this.StopFrequencyHz = stopFrequencyHz;
            this.StepsPerDecade = stepsPerDecade;

            logService.Debug("Starting Picoscope measurement...");
            byte result = FRA4PicoscopeAPI.StartFRA(StartFrequencyHz, StopFrequencyHz, StepsPerDecade);
            if (result != 1) HandlePicoException("Failed to record plot: " + result.ToString());
            else logService.Info("Picoscope measurement succesfully started");
        }

        public string GetMeasruementMessageLog()
        {
            string log = FRA4PicoscopeAPI.GetMessageLog();
            logService.Debug("Message log:" + log);
            return log;
        }

        public void AbortMeasurement()
        {
            logService.Info("Aborting Picoscope measurement...");
            FRA4PicoscopeAPI.CancelFRA();
        }

        public FRA_STATUS_T GetStatus()
        { return FRA4PicoscopeAPI.GetFraStatus(); }

        public void GetResults(out double[] frequenciesLogHz, out double[] gainsDB, out double[] phasesDegrees)
        {
            frequenciesLogHz = null; gainsDB = null; phasesDegrees = null;

            FRA_STATUS_T status = GetStatus();
            while ((status = GetStatus()) == FRA_STATUS_T.FRA_STATUS_IN_PROGRESS)
            {
                System.Threading.Thread.Sleep(1000);
            }

            if (status == FRA_STATUS_T.FRA_STATUS_COMPLETE)
            {
                logService.Info("Picoscope measurement succesfully completed");

                int numOfSteps = FRA4PicoscopeAPI.GetNumSteps();
                logService.Info("Number of recorded steps:" + numOfSteps.ToString());
                if (numOfSteps <= 0) HandlePicoException("Number of recorded steps was equal or below zero!");


                frequenciesLogHz = new double[numOfSteps];
                gainsDB = new double[numOfSteps];
                double[] wrappedPhaseDegrees = new double[numOfSteps];
                phasesDegrees = new double[numOfSteps];

                FRA4PicoscopeAPI.GetResults(frequenciesLogHz, gainsDB, wrappedPhaseDegrees, phasesDegrees);
                logService.Info("Picoscope Results succesfully collected:" + numOfSteps.ToString());
               
            }
            else
            {
                HandlePicoException("Plot record was not completed succesfully: " + status.ToString());
            }        
        }

        #region IDisposable

        bool is_disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!is_disposed) // only dispose once!
            {
                if (disposing) // Not in destructor, OK to reference other objects
                {
                    FRA4PicoscopeAPI.Cleanup();
                    scopeConnected = false;
                }
                // perform cleanup for this object itself

            }
            this.is_disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FRA4Picoscope()
        {
            Dispose(false);

        } 

        #endregion

        #region Properties

        public PS_CHANNEL InputChannel
        {
            get { return (PS_CHANNEL)Enum.Parse(typeof(PS_CHANNEL), (m_SettingsManager.GetStringValue("InputChannel", PS_CHANNEL.PS_CHANNEL_A.ToString()))); }
            set { m_SettingsManager.SetStringValue("InputChannel", value.ToString()); }
        }

        public PS_COUPLING InputCoupling
        {
            get { return (PS_COUPLING)Enum.Parse(typeof(PS_COUPLING), (m_SettingsManager.GetStringValue("InputCoupling", PS_COUPLING.PS_DC.ToString()))); }
            set { m_SettingsManager.SetStringValue("InputCoupling", value.ToString()); }
        }

        public ATTEN_T InputAttenuation
        {
            get { return (ATTEN_T)Enum.Parse(typeof(ATTEN_T), (m_SettingsManager.GetStringValue("InputAttenuation", ATTEN_T.ATTEN_1X.ToString()))); }
            set { m_SettingsManager.SetStringValue("InputAttenuation", value.ToString()); }
        }

        public double InputDCOffset
        {
            get { return m_SettingsManager.GetDoubleValue("InputDCOffset", 0.0); }
            set { m_SettingsManager.SetDoubleValue("InputDCOffset", value); }
        }

        public PS_CHANNEL OutputChannel
        {
            get { return (PS_CHANNEL)Enum.Parse(typeof(PS_CHANNEL), (m_SettingsManager.GetStringValue("OutputChannel", PS_CHANNEL.PS_CHANNEL_B.ToString()))); }
            set { m_SettingsManager.SetStringValue("OutputChannel", value.ToString()); }
        }

        public PS_COUPLING OutputCoupling
        {
            get { return (PS_COUPLING)Enum.Parse(typeof(PS_COUPLING), (m_SettingsManager.GetStringValue("OutputCoupling", PS_COUPLING.PS_DC.ToString()))); }
            set { m_SettingsManager.SetStringValue("OutputCoupling", value.ToString()); }
        }

        public ATTEN_T OutputAttenuation
        {
            get { return (ATTEN_T)Enum.Parse(typeof(ATTEN_T), (m_SettingsManager.GetStringValue("OutputAttenuation", ATTEN_T.ATTEN_1X.ToString()))); }
            set { m_SettingsManager.SetStringValue("OutputAttenuation", value.ToString()); }
        }

        public double OutputDCOffset
        {
            get { return m_SettingsManager.GetDoubleValue("OutputDCOffset", 0.0); }
            set { m_SettingsManager.SetDoubleValue("OutputDCOffset", value); }
        }

        public double InitialStimulus
        {
            get { return m_SettingsManager.GetDoubleValue("InitialStimulus", 0.0); }
            set { m_SettingsManager.SetDoubleValue("InitialStimulus", value); }
        }

        public double MaxStimulus
        {
            get { return m_SettingsManager.GetDoubleValue("MaxStimulus", 1.0); }
            set { m_SettingsManager.SetDoubleValue("MaxStimulus", value); }
        }

        public double StimulusOffset
        {
            get { return m_SettingsManager.GetDoubleValue("StimulusOffset", 0.0); }
            set { m_SettingsManager.SetDoubleValue("StimulusOffset", value); }
        }

        public double StartFrequencyHz
        {
            get { return m_SettingsManager.GetDoubleValue("StartFrequencyHz", 100.0); }
            set { m_SettingsManager.SetDoubleValue("StartFrequencyHz", value); }
        }

        public double StopFrequencyHz
        {
            get { return m_SettingsManager.GetDoubleValue("StopFrequencyHz", 10000.0); }
            set { m_SettingsManager.SetDoubleValue("StopFrequencyHz", value); }
        }

        public int StepsPerDecade
        {
            get { return m_SettingsManager.GetIntValue("StepsPerDecade", 10); }
            set { m_SettingsManager.SetIntValue("StepsPerDecade", value); }
        }

        private void HandlePicoException(string message)
        {
            logService.Error(message);
            FRA4PicoscopeAPI.Cleanup();
            scopeConnected = false;
            throw new ApplicationException(message);
        }

        #endregion
    }

    #region Enuums

    public enum PS_CHANNEL
    {
        PS_CHANNEL_A,
        PS_CHANNEL_B,
        PS_CHANNEL_C,
        PS_CHANNEL_D,
        PS_CHANNEL_E,
        PS_CHANNEL_F,
        PS_CHANNEL_G,
        PS_CHANNEL_H,
        PS_CHANNEL_INVALID,
    }

    public enum PS_COUPLING
    {
        PS_AC,
        PS_DC,
        PS_DC_1M = PS_DC,
        PS_DC_50R,
    }

    public enum ATTEN_T
    {
        ATTEN_1X,
        ATTEN_10X,
        ATTEN_20X,
        ATTEN_100X,
        ATTEN_200X,
        ATTEN_1000X,
    }

    public enum SamplingMode_T
    {
        LOW_NOISE,
        HIGH_NOISE,
    }

    public enum FRA_STATUS_T
    {
        FRA_STATUS_IDLE,
        FRA_STATUS_IN_PROGRESS,
        FRA_STATUS_COMPLETE,
        FRA_STATUS_CANCELED,
        FRA_STATUS_AUTORANGE_LIMIT,
        FRA_STATUS_POWER_CHANGED,
        FRA_STATUS_FATAL_ERROR,
        FRA_STATUS_MESSAGE,
    }

    public enum LOG_MESSAGE_FLAGS_T
    {
        SCOPE_ACCESS_DIAGNOSTICS = 0x0001,
        FRA_PROGRESS = 0x0002,
        STEP_TRIAL_PROGRESS = 0x0004,
        SIGNAL_GENERATOR_DIAGNOSTICS = 0x0008,
        AUTORANGE_DIAGNOSTICS = 0x0010,
        ADAPTIVE_STIMULUS_DIAGNOSTICS = 0x0020,
        SAMPLE_PROCESSING_DIAGNOSTICS = 0x0040,
        DFT_DIAGNOSTICS = 0x0080,
        SCOPE_POWER_EVENTS = 0x0100,
        SAVE_EXPORT_STATUS = 0x0200,
        FRA_WARNING = 0x0400,
        FRA_ERROR = 0x8000,
    }

    #endregion
}
