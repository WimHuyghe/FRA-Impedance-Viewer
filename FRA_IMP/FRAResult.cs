using System;
using System.Globalization;

namespace FRA_IMP
{
    public class FRAResult : IComparable
    {
        #region String formats for Result

        // format string for frequency in log Hz
        private static string FormatFreqLogHz = "0.##### Hz";
        private static string FormatFreqLogKiloHz = "0,.##### kHz";
        private static string FormatFreqLogMegaHz = "0,,.##### MHz";

        // format string for frequency in log Hz
        private static string FormatFreqHz = "#,# Hz";

        private static string FormatGainDb = "0.#### dB";

        private static string FormatPhaseDegrees = "0.#### °";

        // format string for impedance in milli ohms
        private static string FormatImpedanceMilliOhms = "0.#### mOhms";
        private static string FormatImpedanceOhms = "0,.#### Ohms";
        private static string FormatImpedanceKiloOhms = "0,,.#### KOhms";
        private static string FormatImpedanceMegaOhms = "0,,,.#### MOhms";

        // format string for capacitance in pico farads
        private static string FormatCapacitanceFarad = "0,,,,.#### F";
        private static string FormatCapacitanceMilliFarad = "0,,,.#### mF";
        private static string FormatCapacitanceMicroFarad = "0,,.#### µF";
        private static string FormatCapacitanceNanoFarad = "0,.#### nF";
        private static string FormatCapacitancePicoFarad = "0.#### pF";

        // format string for inductance in nano henry
        private static string FormatInductanceHenry = "0,,,.#### H";
        private static string FormatInductanceMilliHenry = "0,,.#### mH";
        private static string FormatInductanceMicroHenry = "0,.#### µH";
        private static string FormatInductanceNanoHenry = "0.#### nH";

        private static string FormatQFactor = "0.####";

        public static string GetFrequencyFormat(double frequencyHz, bool logAxis)
        {
            if (logAxis)
            {
                double freq = Math.Abs(frequencyHz);
                if (freq > 1000000) return FormatFreqLogMegaHz;
                if (freq > 1000) return FormatFreqLogKiloHz;
                return FormatFreqLogHz;
            }
            else return FormatFreqHz;
        }
        public static string GetGainFormat(double gainDB)
        {
            return FormatGainDb;
        }
        public static string GetPhaseFormat(double phaseDegrees)
        {
            return FormatPhaseDegrees;
        }
        public static string GetImpedanceFormat(double resistanceMilliOhms)
        {
            double impAbs = Math.Abs(resistanceMilliOhms);
            if (impAbs > 10000000000) return FormatImpedanceMegaOhms;
            if (impAbs > 10000000) return FormatImpedanceKiloOhms;
            if (impAbs > 10000) return FormatImpedanceOhms;
            return FormatImpedanceMilliOhms;
        }
        public static string GetCapacitanceFormat(double capacitancePicoFarads)
        {
            double capAbs = Math.Abs(capacitancePicoFarads);
            if (capAbs > 10000000000000) return FormatCapacitanceFarad;
            if (capAbs > 10000000000) return FormatCapacitanceMilliFarad;
            if (capAbs > 10000000) return FormatCapacitanceMicroFarad;
            if (capAbs > 10000) return FormatCapacitanceNanoFarad;
            return FormatCapacitancePicoFarad;
        }
        public static string GetInductanceFormat(double InductanceNanoHenry)
        {
            double inducAbs = Math.Abs(InductanceNanoHenry);
            if (inducAbs > 10000000000) return FormatInductanceHenry;
            if (inducAbs > 10000000) return FormatInductanceMilliHenry;
            if (inducAbs > 10000) return FormatInductanceMicroHenry;
            return FormatInductanceNanoHenry;
        }
        public static string GetQFactorFormat(double qFactor)
        {
            return FormatQFactor;
        }

        #endregion

        public FRAResult(double frequencyHz, double gainDB, double phaseDegrees, double referenceResistorOhms)
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            m_FrequencyHz = frequencyHz;
            m_GainFRA_DB = gainDB;
            m_PhaseDegrees = phaseDegrees;
            m_ReferenceResistorOhms = referenceResistorOhms;
        }

        #region Properties

        private double m_ReferenceResistorOhms;
        public double ReferenceResistorOhms
        {
            get { return m_ReferenceResistorOhms; }
            set { m_ReferenceResistorOhms = value; }
        }

        private double m_FrequencyHz;
        public double FrequencyHz
        {
            get { return m_FrequencyHz; }
        }

        private double m_GainFRA_DB;
        public double GainFRA_DB
        {
            get { return m_GainFRA_DB; }
        }

        public double GainFRA
        {
            get { return Math.Pow(10.0, this.GainFRA_DB / 20); }
        }

        public double GainCorrectedDB
        {
            get
            {
                if (GainFRA_DB > 0) return 0; // cannot be greated than 0 dB for passive devices (but sometimes is a little due to noise), and this gives an error in the calculation of the impedance plots
                else return GainFRA_DB;
            }
        }

        public double GainCorrected
        {
            get { return Math.Pow(10.0, this.GainCorrectedDB / 20); }
        }

        private double m_PhaseDegrees;
        public double PhaseDegrees
        {
            get { return m_PhaseDegrees; }
        }

        public double PhaseRadians
        {
            get { return m_PhaseDegrees * Math.PI / 180; }
        }

        public double DUTImpedanceMilliOhms
        {
            get { return (GainCorrected * ReferenceResistorOhms) / Math.Sqrt(1 - (2 * GainCorrected * Math.Cos(PhaseRadians)) + (GainCorrected * GainCorrected)) * 1000; }
        }

        public double DUTPhaseRadians
        {
            get { return PhaseRadians - Math.Atan((GainCorrected * Math.Sin(PhaseRadians)) / (GainCorrected * Math.Cos(PhaseRadians) - 1)); }
        }

        public double DUTPhaseDegrees
        {
            get { return PhaseRadians * 180 / Math.PI; }
        }

        public double DUTCapacitancePicoFarad
        {
            get { return -1e12 / (2 * Math.PI * FrequencyHz * DUTImpedanceMilliOhms / 1000 * Math.Sin(DUTPhaseRadians)); }
        }

        public double DUT_ESR_MilliOhms
        {
            get { return DUTImpedanceMilliOhms * Math.Cos(DUTPhaseRadians); }
        }

        public double DUTInductanceNanoHenry
        {
            get { return DUTImpedanceMilliOhms / 1000 * Math.Sin(DUTPhaseRadians) / (2 * Math.PI * FrequencyHz) * 1e9; }
        }

        public double DUT_QCapacitor
        {
            get { return 1 / (2 * Math.PI * FrequencyHz * DUTCapacitancePicoFarad * 1e-12 * DUT_ESR_MilliOhms / 1000); }
        }

        public double DUT_QInductor
        {
            get { return (2 * Math.PI * FrequencyHz) * DUTInductanceNanoHenry * 1e-9 / (DUT_ESR_MilliOhms / 1000); }
        }

        #endregion

        // sort results on frequency
        public int CompareTo(object obj)
        {
            FRAResult comparer = (FRAResult)obj;
            return this.FrequencyHz.CompareTo(comparer.FrequencyHz);
        }

        public string ToStringValues()
        {
            return FrequencyHz.ToString() + "\t" + GainFRA_DB.ToString() + "\t" + PhaseDegrees.ToString() + "\t" +
                DUTImpedanceMilliOhms.ToString() + "\t" + DUTCapacitancePicoFarad.ToString() + "\t" +
                DUTInductanceNanoHenry.ToString() + "\t" + DUT_ESR_MilliOhms.ToString() + "\t" + DUT_QCapacitor.ToString() + "\t" + DUT_QInductor.ToString();
        }

        public static string ToStringTitles()
        {
            return "Frequency(Hz)" + "\t" + "Gain(dB)" + "\t" + "Phase(°)" + "\t" + "Impedance(MilliOhms)" + "\t" + 
                "Capacitance(pF)" + "\t" + "Inductance(nH)" + "\t" + "ESR(MilliOhms)" + "\t" + "Q(Capacitor)" + "\t" + "Q(Inductor)";
        }
    }
}
