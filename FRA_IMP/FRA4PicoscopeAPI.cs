using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FRA_IMP
{
    public class FRA4PicoscopeAPI
    {
        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern Byte SetScope(string sn);

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern double GetMinFrequency();

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern byte StartFRA(double startFreqHz, double stopFreqHz, int stepsPerDecade);

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern byte CancelFRA();

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern FRA_STATUS_T GetFraStatus();

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void SetFraSettings(SamplingMode_T samplingMode, byte adaptiveStimulusMode, double targetResponseAmplitude, byte sweepDescending,
            double phaseWrappingThreshold);

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void SetFraTuning(double purityLowerLimit, int extraSettlingTimeMs, byte autorangeTriesPerStep, double autorangeTolerance,
            double smallSignalResolutionTolerance, double maxAutorangeAmplitude, int inputStartRange, int outputStartRange, byte adaptiveStimulusTriesPerStep,
            double targetResponseAmplitudeTolerance, int minCyclesCaptured, double maxDftBw, int lowNoiseOversampling);

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern byte SetupChannels(PS_CHANNEL inputChannel, PS_COUPLING inputChannelCoupling, ATTEN_T inputChannelAttenuation, double inputDcOffset,
            PS_CHANNEL outputChannel, PS_COUPLING outputChannelCoupling, ATTEN_T outputChannelAttenuation, double outputDcOffset, double initialStimulusVpp,
            double maxStimulusVpp, double stimulusDcOffset);

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetNumSteps();

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void GetResults([In, Out] double[] freqsLogHz, [In, Out]  double[] gainsDb, [In, Out]  double[] phasesDeg, [In, Out]  double[] unwrappedPhasesDeg);

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void EnableDiagnostics(string baseDataPath);

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void DisableDiagnostics();

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void AutoClearMessageLog(bool bAutoClear);

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void EnableMessageLog(bool bEnable);

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void SetLogVerbosityFlag(LOG_MESSAGE_FLAGS_T flag, bool enable);

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void SetLogVerbosityFlags(LOG_MESSAGE_FLAGS_T flags);

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //[return: MarshalAs(UnmanagedType.LPWStr)]
        public static extern string GetMessageLog();

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void ClearMessageLog();

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern byte Initialize();

        [DllImport("FRA4PicoScope.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void Cleanup();
    }
}
