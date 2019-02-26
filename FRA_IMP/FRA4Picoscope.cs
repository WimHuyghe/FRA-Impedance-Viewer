using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WHConsult.Utils.Log4Net;
using WHConsult.Utils.Settings;
using WHConsult.Utils.Settings.IniFiles;

namespace FRA_IMP
{
    public class FRA4Picoscope : IDisposable
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
            logService.Info("Connecting Picoscope...");
            byte result = FRA4PicoscopeAPI.Initialize();
            if (result != 1) HandlePicoException("Failed to initialize Picoscope API: " + result.ToString());
            else logService.Info("Picoscope API succesfully initialized");
            FRA4PicoscopeAPI.EnableMessageLog(true);
            FRA4PicoscopeAPI.AutoClearMessageLog(true);

            result = FRA4PicoscopeAPI.SetScope(""); // TODO: add SN to directly connect to previous scope (currently API does not support reading SN)
            if (result != 1) HandlePicoException("Failed to select Picoscope: " + result.ToString());
            else logService.Info("Picoscope succesfully selected");

            scopeConnected = true;
        }

        private void SetLogVerbosity()
        {
            logService.Debug("Setting Log Verbosity...");
            FRA4PicoscopeAPI.SetLogVerbosityFlag(LOG_MESSAGE_FLAGS_T.SCOPE_ACCESS_DIAGNOSTICS, LogScopeAccessDiagnostics);
            FRA4PicoscopeAPI.SetLogVerbosityFlag(LOG_MESSAGE_FLAGS_T.FRA_PROGRESS, LogFRAProgress);
            FRA4PicoscopeAPI.SetLogVerbosityFlag(LOG_MESSAGE_FLAGS_T.STEP_TRIAL_PROGRESS, LogStepTrailProgress);
            FRA4PicoscopeAPI.SetLogVerbosityFlag(LOG_MESSAGE_FLAGS_T.SIGNAL_GENERATOR_DIAGNOSTICS, LogSignalGeneratorDiagnostics);
            FRA4PicoscopeAPI.SetLogVerbosityFlag(LOG_MESSAGE_FLAGS_T.AUTORANGE_DIAGNOSTICS, LogAutoRangeDiagnostics);
            FRA4PicoscopeAPI.SetLogVerbosityFlag(LOG_MESSAGE_FLAGS_T.ADAPTIVE_STIMULUS_DIAGNOSTICS, LogAdapticeStimulusDiagnostics);
            FRA4PicoscopeAPI.SetLogVerbosityFlag(LOG_MESSAGE_FLAGS_T.SAMPLE_PROCESSING_DIAGNOSTICS, LogSampleProcessingDiagnostics);
            FRA4PicoscopeAPI.SetLogVerbosityFlag(LOG_MESSAGE_FLAGS_T.DFT_DIAGNOSTICS, LogDFTDiagnostics);
            FRA4PicoscopeAPI.SetLogVerbosityFlag(LOG_MESSAGE_FLAGS_T.SCOPE_POWER_EVENTS, LogScopePowerEvents);
            FRA4PicoscopeAPI.SetLogVerbosityFlag(LOG_MESSAGE_FLAGS_T.SAVE_EXPORT_STATUS, LogSaveExportStatus);
            FRA4PicoscopeAPI.SetLogVerbosityFlag(LOG_MESSAGE_FLAGS_T.FRA_WARNING, LogFRAWarnings);
        }

        public string CheckSettings()
        {
            StringWriter result = new StringWriter();
            if (!scopeConnected) ConnectPicoscope(); // must be connected to check the min frequency
            double minFrequency = FRA4PicoscopeAPI.GetMinFrequency();
            if (StartFrequencyHz < minFrequency) result.WriteLine("Start frequency must be higher than " + minFrequency + " Hz");
            if (StopFrequencyHz < minFrequency) result.WriteLine("Stop frequency must be higher than " + minFrequency + " Hz");
            if (StartFrequencyHz > StopFrequencyHz) result.WriteLine("Start frequency must be higher than Stop frequency !");
            if (InputChannel == OutputChannel) result.WriteLine("Input channel and output channel cannot be identical");
            if (InitialStimulus > MaxStimulus) result.WriteLine("Initial Stimulus must be smaller than max stimulus");
            if (StepsPerDecade < 1) result.WriteLine("Steps per decade must be greater than zero!");
            if (PhaseWrappingThreshold <= 0) result.WriteLine("Phase wrapping threshold must be greater than zero!");
            
            if (!result.ToString().Equals("")) logService.Warn("Settings incorrect:" + result.ToString());
            return result.ToString();
        }

        private void SetFRASettings()
        {
            logService.Debug("Setting FRA Settings...");
            byte stimulusMode = 0;
            if (AdaptiveStimulusMode) stimulusMode = 1;
            byte sweepDec = 0;
            if (SweepDescending) sweepDec = 1;
            FRA4PicoscopeAPI.SetFraSettings(SamplingMode, stimulusMode, TargetResponseAmplitude, sweepDec, PhaseWrappingThreshold);
        }

        private void SetFRATuning()
        {
            logService.Debug("Setting FRA Tuning...");
            FRA4PicoscopeAPI.SetFraTuning(PurityLowerLimit, ExtraSettlingTimeMs, AutoRangeTriesPerStep, AutoRangeTolerance, SmallSignalResolutionTolerance, MaxAutorangeAmplitude,
                InputStartRange, OutputStartRange, AdaptiveStimulusTriesPerStep, TargetResponseAmplitudeTolerance, MinCyclesCaptured, MaxDftBandWidth, LowNoiseOversampling);
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

        public void StartMeasurement()
        {
            if (!scopeConnected) ConnectPicoscope();
            string checkSettingsResult = CheckSettings();
            if (!checkSettingsResult.Equals("")) throw new ApplicationException(checkSettingsResult);
            SetLogVerbosity();
            SetUpChannels();
            SetFRASettings();
            SetFRATuning();
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

        public SamplingMode_T SamplingMode
        {
            get { return (SamplingMode_T)Enum.Parse(typeof(SamplingMode_T), (m_SettingsManager.GetStringValue("SamplingMode", SamplingMode_T.LOW_NOISE.ToString()))); }
            set { m_SettingsManager.SetStringValue("SamplingMode", value.ToString()); }
        }

        public bool AdaptiveStimulusMode
        {
            get { return m_SettingsManager.GetBooleanValue("AdaptiveStimulusMode", false); }
            set { m_SettingsManager.SetBooleanValue("AdaptiveStimulusMode", value); }
        }

        public bool SweepDescending
        {
            get { return m_SettingsManager.GetBooleanValue("SweepDescending", false); }
            set { m_SettingsManager.SetBooleanValue("SweepDescending", value); }
        }

        public double TargetResponseAmplitude
        {
            get { return m_SettingsManager.GetDoubleValue("TargetResponseAmplitude", 0.0); }
            set { m_SettingsManager.SetDoubleValue("TargetResponseAmplitude", value); }
        }

        public double PhaseWrappingThreshold
        {
            get { return m_SettingsManager.GetDoubleValue("PhaseWrappingThreshold", 180.0); }
            set { m_SettingsManager.SetDoubleValue("PhaseWrappingThreshold", value); }
        }

        public double PurityLowerLimit
        {
            get { return m_SettingsManager.GetDoubleValue("PurityLowerLimit", 0.80); }
            set { m_SettingsManager.SetDoubleValue("PurityLowerLimit", value); }
        }

        public int ExtraSettlingTimeMs
        {
            get { return m_SettingsManager.GetIntValue("ExtraSettlingTimeMs", 0); }
            set { m_SettingsManager.SetIntValue("ExtraSettlingTimeMs", value); }
        }

        public byte AutoRangeTriesPerStep
        {
            get { return m_SettingsManager.GetByteValue("AutoRangeTriesPerStep", 10); }
            set { m_SettingsManager.SetByteValue("AutoRangeTriesPerStep", value); }
        }

        public double AutoRangeTolerance
        {
            get { return m_SettingsManager.GetDoubleValue("AutoRangeTolerance", 0.10); }
            set { m_SettingsManager.SetDoubleValue("AutoRangeTolerance", value); }
        }

        public double SmallSignalResolutionTolerance
        {
            get { return m_SettingsManager.GetDoubleValue("SmallSignalResolutionTolerance", 0.0); }
            set { m_SettingsManager.SetDoubleValue("SmallSignalResolutionTolerance", value); }
        }

        public double MaxAutorangeAmplitude
        {
            get { return m_SettingsManager.GetDoubleValue("MaxAutorangeAmplitude", 1.0); }
            set { m_SettingsManager.SetDoubleValue("MaxAutorangeAmplitude", value); }
        }

        public int InputStartRange
        {
            get { return m_SettingsManager.GetIntValue("InputStartRange", -1); }
            set { m_SettingsManager.SetIntValue("InputStartRange", value); }
        }

        public int OutputStartRange
        {
            get { return m_SettingsManager.GetIntValue("OutputStartRange", 0); }
            set { m_SettingsManager.SetIntValue("OutputStartRange", value); }
        }

        public byte AdaptiveStimulusTriesPerStep
        {
            get { return m_SettingsManager.GetByteValue("AdaptiveStimulusTriesPerStep", 10); }
            set { m_SettingsManager.SetByteValue("AdaptiveStimulusTriesPerStep", value); }
        }

        public double TargetResponseAmplitudeTolerance
        {
            get { return m_SettingsManager.GetDoubleValue("TargetResponseAmplitudeTolerance", 0.1); }//=10%
            set { m_SettingsManager.SetDoubleValue("TargetResponseAmplitudeTolerance", value); }
        }

        public int MinCyclesCaptured
        {
            get { return m_SettingsManager.GetIntValue("MinCyclesCaptured", 16); }
            set { m_SettingsManager.SetIntValue("MinCyclesCaptured", value); }
        }

        public double MaxDftBandWidth
        {
            get { return m_SettingsManager.GetDoubleValue("MaxDftBandWidth", 100); }//=10%
            set { m_SettingsManager.SetDoubleValue("MaxDftBandWidth", value); }
        }

        public int LowNoiseOversampling
        {
            get { return m_SettingsManager.GetIntValue("LowNoiseOversampling", 64); }
            set { m_SettingsManager.SetIntValue("LowNoiseOversampling", value); }
        }

        public bool LogScopeAccessDiagnostics
        {
            get { return m_SettingsManager.GetBooleanValue("LogScopeAccessDiagnostics", false); }
            set { m_SettingsManager.SetBooleanValue("LogScopeAccessDiagnostics", value); }
        }

        public bool LogFRAProgress
        {
            get { return m_SettingsManager.GetBooleanValue("LogFRAProgress", true); }
            set { m_SettingsManager.SetBooleanValue("LogFRAProgress", value); }
        }

        public bool LogStepTrailProgress
        {
            get { return m_SettingsManager.GetBooleanValue("LogStepTrailProgress", true); }
            set { m_SettingsManager.SetBooleanValue("LogStepTrailProgress", value); }
        }

        public bool LogSignalGeneratorDiagnostics
        {
            get { return m_SettingsManager.GetBooleanValue("LogSignalGeneratorDiagnostics", false); }
            set { m_SettingsManager.SetBooleanValue("LogSignalGeneratorDiagnostics", value); }
        }

        public bool LogAutoRangeDiagnostics
        {
            get { return m_SettingsManager.GetBooleanValue("LogAutoRangeDiagnostics", false); }
            set { m_SettingsManager.SetBooleanValue("LogAutoRangeDiagnostics", value); }
        }

        public bool LogAdapticeStimulusDiagnostics
        {
            get { return m_SettingsManager.GetBooleanValue("LogAdapticeStimulusDiagnostics", false); }
            set { m_SettingsManager.SetBooleanValue("LogAdapticeStimulusDiagnostics", value); }
        }

        public bool LogSampleProcessingDiagnostics
        {
            get { return m_SettingsManager.GetBooleanValue("LogSampleProcessingDiagnostics", false); }
            set { m_SettingsManager.SetBooleanValue("LogSampleProcessingDiagnostics", value); }
        }

        public bool LogDFTDiagnostics
        {
            get { return m_SettingsManager.GetBooleanValue("LogDFTDiagnostics", false); }
            set { m_SettingsManager.SetBooleanValue("LogDFTDiagnostics", value); }
        }

        public bool LogScopePowerEvents
        {
            get { return m_SettingsManager.GetBooleanValue("LogScopePowerEvents", true); }
            set { m_SettingsManager.SetBooleanValue("LogScopePowerEvents", value); }
        }

        public bool LogSaveExportStatus
        {
            get { return m_SettingsManager.GetBooleanValue("LogSaveExportStatus", true); }
            set { m_SettingsManager.SetBooleanValue("LogSaveExportStatus", value); }
        }

        public bool LogFRAWarnings
        {
            get { return m_SettingsManager.GetBooleanValue("LogFRAWarnings", true); }
            set { m_SettingsManager.SetBooleanValue("LogFRAWarnings", value); }
        }

        // TODO: currently not forseen in enum, but is available in FRA4Picoscope
        public bool LogPicoscopeAPICalls
        {
            get { return m_SettingsManager.GetBooleanValue("LogPicoscopeAPICalls", false); }
            set { m_SettingsManager.SetBooleanValue("LogPicoscopeAPICalls", value); }
        }

        #endregion

        private void HandlePicoException(string message)
        {
            logService.Error(message);
            FRA4PicoscopeAPI.Cleanup();
            scopeConnected = false;
            throw new ApplicationException(message);
        }
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
