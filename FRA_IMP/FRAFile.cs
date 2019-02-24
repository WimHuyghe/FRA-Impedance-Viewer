using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHConsult.Utils.Log4Net;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Globalization;

namespace FRA_IMP
{
    public class FRAFile
    {
        private ILogService logService;
        private List<FRAResult> m_FRAResults;
        private double m_MaxFrequencyHz = Double.MinValue, m_MinFrequencyHz = Double.MaxValue;
        private double m_MaxGainDB = Double.MinValue, m_MinGainDB = Double.MaxValue;
        private double m_MaxPhaseDegrees = Double.MinValue, m_MinPhaseDegrees = Double.MaxValue;
        private double m_MaxDUTImpedanceOhms = Double.MinValue, m_MinDUTImpedanceOhms = Double.MaxValue;
        private double m_MaxDUTPhaseDegrees = Double.MinValue, m_MinDUTPhaseDegrees = Double.MaxValue;
        private double m_MaxDUTCapacitancePicoFarad = Double.MinValue, m_MinDUTCapacitancePicoFarad = Double.MaxValue;
        private double m_MaxDUT_ESR_Ohms = Double.MinValue, m_MinDUT_ESR_Ohms = Double.MaxValue;
        private double m_MaxDUTInductanceNanoHenry = Double.MinValue, m_MinDUTInductanceNanoHenry = Double.MaxValue;

        public FRAFile(string path, FRAFileType fileType, double referenceResistor)
        {
            logService = new FileLogService(typeof(FRAFile));
            logService.Debug("Creating FRASeries from file " + path);
            m_FRAResults = new List<FRAResult>();
            m_FilePath = path;
            m_ReferenceResistor = referenceResistor;
            m_FRAFileType = fileType;
            m_DateTimeCreated = DateTime.Now;
            AddResults();
            CreateLineSeries();
        }

        public FRAFile(FRAFileType fileType, double[] frequenciesLogHz, double[] gainsDB, double[] phasesDegrees, double referenceResistorOhms)
        {
            logService = new FileLogService(typeof(FRAFile));
            logService.Debug("Creating FRASeries from arrays ");
            m_FRAResults = new List<FRAResult>();
            m_FilePath = "";
            m_ReferenceResistor = referenceResistorOhms;
            m_FRAFileType = fileType;
            m_DateTimeCreated = DateTime.Now;
            AddResults(frequenciesLogHz, gainsDB, phasesDegrees);
            CreateLineSeries();
        }

        #region Result Collection 

        private void AddResults(double[] frequencyHz, double[] gainDB, double[] phaseDegrees)
        {
            for (int i = 0; i < frequencyHz.Length; i++) AddResult(new FRAResult(Math.Pow(10, frequencyHz[i]), gainDB[i], phaseDegrees[i], ReferenceResistorOhms));
        }

        private void AddResults()
        {
            using (TextFieldParser parser = new TextFieldParser(FilePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Parse header 
                int firstRows = 0;
                if (FRAFileType == FRAFileType.FRA4PicoScope) firstRows = 1;
                if (FRAFileType == FRAFileType.Keysight) firstRows = 1;
                if (FRAFileType == FRAFileType.RhodeSchwarz) firstRows = 1;
                while (firstRows != 0)
                {
                    parser.ReadFields();
                    firstRows -= 1;
                }

                while (!parser.EndOfData)
                {
                    //Process row
                    string[] fields = parser.ReadFields();
                    logService.Debug("Opening file:" + FilePath);
                    logService.Debug("File:" + FRAFileType.ToString());
                    logService.Debug("LogFreq:" + fields[0] + " DbGain:" + fields[1] + " Phase:" + fields[2]);

                    CultureInfo culturFRA4PicoScope = CultureInfo.InvariantCulture;
                    CultureInfo culturKeysight = CultureInfo.InvariantCulture;
                    CultureInfo culturRhodeSwchwarz = CultureInfo.InvariantCulture;
                    double frequencyHz = 0;
                    if (FRAFileType == FRAFileType.FRA4PicoScope) frequencyHz = Math.Pow(10, Convert.ToDouble(fields[0], culturFRA4PicoScope));
                    if (FRAFileType == FRAFileType.Keysight) frequencyHz = Convert.ToDouble(fields[1], culturKeysight);
                    if (FRAFileType == FRAFileType.RhodeSchwarz) frequencyHz = Convert.ToDouble(fields[1], culturRhodeSwchwarz);
                    double gainDB = 0;
                    if (FRAFileType == FRAFileType.FRA4PicoScope) gainDB = Convert.ToDouble(fields[1], culturFRA4PicoScope);
                    if (FRAFileType == FRAFileType.Keysight) gainDB = Convert.ToDouble(fields[3], culturKeysight);
                    if (FRAFileType == FRAFileType.RhodeSchwarz) gainDB = Convert.ToDouble(fields[2], culturRhodeSwchwarz);
                    double phaseDegrees = 0;
                    if (FRAFileType == FRAFileType.FRA4PicoScope) phaseDegrees = Convert.ToDouble(fields[2], culturFRA4PicoScope);
                    if (FRAFileType == FRAFileType.Keysight) phaseDegrees = Convert.ToDouble(fields[4], culturKeysight);
                    if (FRAFileType == FRAFileType.RhodeSchwarz) phaseDegrees = Convert.ToDouble(fields[3], culturRhodeSwchwarz);

                    AddResult(new FRAResult(frequencyHz, gainDB, phaseDegrees, ReferenceResistorOhms));
                }
            }
            m_FRAResults.Sort();
        }

        private void AddResult(FRAResult result)
        {
            m_FRAResults.Add(result);
            if (result.FrequencyHz > m_MaxFrequencyHz) m_MaxFrequencyHz = result.FrequencyHz;
            if (result.FrequencyHz < m_MinFrequencyHz) m_MinFrequencyHz = result.FrequencyHz;
            if (result.GainFRA_DB > m_MaxGainDB) m_MaxGainDB = result.GainFRA_DB;
            if (result.GainFRA_DB < m_MinGainDB) m_MinGainDB = result.GainFRA_DB;
            if (result.PhaseDegrees > m_MaxPhaseDegrees) m_MaxPhaseDegrees = result.PhaseDegrees;
            if (result.PhaseDegrees < m_MinPhaseDegrees) m_MinPhaseDegrees = result.PhaseDegrees;
            if (result.DUTImpedanceMilliOhms > m_MaxDUTImpedanceOhms) m_MaxDUTImpedanceOhms = result.DUTImpedanceMilliOhms;
            if (result.DUTImpedanceMilliOhms < m_MinDUTImpedanceOhms) m_MinDUTImpedanceOhms = result.DUTImpedanceMilliOhms;
            if (result.DUTPhaseDegrees > m_MaxDUTPhaseDegrees) m_MaxDUTPhaseDegrees = result.DUTPhaseDegrees;
            if (result.DUTPhaseDegrees < m_MinDUTPhaseDegrees) m_MinDUTPhaseDegrees = result.DUTPhaseDegrees;
            if (result.DUTCapacitancePicoFarad > m_MaxDUTCapacitancePicoFarad) m_MaxDUTCapacitancePicoFarad = result.DUTCapacitancePicoFarad;
            if (result.DUTCapacitancePicoFarad < m_MinDUTCapacitancePicoFarad) m_MinDUTCapacitancePicoFarad = result.DUTCapacitancePicoFarad;
            if (result.DUT_ESR_MilliOhms > m_MaxDUT_ESR_Ohms) m_MaxDUT_ESR_Ohms = result.DUT_ESR_MilliOhms;
            if (result.DUT_ESR_MilliOhms < m_MinDUT_ESR_Ohms) m_MinDUT_ESR_Ohms = result.DUT_ESR_MilliOhms;
            if (result.DUTInductanceNanoHenry > m_MaxDUTInductanceNanoHenry) m_MaxDUTInductanceNanoHenry = result.DUTInductanceNanoHenry;
            if (result.DUTInductanceNanoHenry < m_MinDUTInductanceNanoHenry) m_MinDUTInductanceNanoHenry = result.DUTInductanceNanoHenry;
        }

        public IList<FRAResult> Results
        {
            get { return m_FRAResults.AsReadOnly(); }
        }

        public int Count
        {
            get { return m_FRAResults.Count; }
        }

        #endregion

        #region Result Queries & Info

        private int GetIndexResultBelowFrequency(double frequency)
        {
            int result = 0;
            m_FRAResults.Sort();
            for (int i = result; i < m_FRAResults.Count; i++) if (m_FRAResults[i].FrequencyHz <= frequency) result = i;
            return result;
        }
        private FRAResult GetResultBelowFrequency(double frequency)
        {
            return m_FRAResults[GetIndexResultBelowFrequency(frequency)];
        }
        private int GetIndexResultAboveFrequency(double frequency)
        {
            int result = m_FRAResults.Count - 1;
            m_FRAResults.Sort();
            for (int i = result; i >= 0; i--) if (m_FRAResults[i].FrequencyHz > frequency) result = i;
            return result;
        }
        private FRAResult GetResultAboveFrequency(double frequency)
        {
            return m_FRAResults[GetIndexResultAboveFrequency(frequency)];
        }

        public double GetGainDB(double frequency)
        {
            FRAResult resAbove = GetResultAboveFrequency(frequency);
            FRAResult resBelow = GetResultBelowFrequency(frequency);
            if (resAbove != resBelow)
            {
                double interpollFreqFactor = (frequency - resBelow.FrequencyHz) / (resAbove.FrequencyHz - resBelow.FrequencyHz);
                double gainDelta = resAbove.GainFRA_DB - resBelow.GainFRA_DB;
                return resBelow.GainFRA_DB + (interpollFreqFactor * gainDelta);
            }
            else return resBelow.GainFRA_DB;
        }

        public double GetPhaseDegrees(double frequency)
        {
            FRAResult resAbove = GetResultAboveFrequency(frequency);
            FRAResult resBelow = GetResultBelowFrequency(frequency);
            if (resAbove != resBelow)
            {
                double interpollFreqFactor = (frequency - resBelow.FrequencyHz) / (resAbove.FrequencyHz - resBelow.FrequencyHz);
                double phaseDelta = resAbove.PhaseDegrees - resBelow.PhaseDegrees;
                return resBelow.PhaseDegrees + (interpollFreqFactor * phaseDelta);
            }
            else return resBelow.PhaseDegrees;
        }

        public double GetDUTImpedanceMilliOmhs(double frequency)
        {
            FRAResult resAbove = GetResultAboveFrequency(frequency);
            FRAResult resBelow = GetResultBelowFrequency(frequency);
            if (resAbove != resBelow)
            {
                double interpollFreqFactor = (frequency - resBelow.FrequencyHz) / (resAbove.FrequencyHz - resBelow.FrequencyHz);
                double ImpedanceDelta = resAbove.DUTImpedanceMilliOhms - resBelow.DUTImpedanceMilliOhms;
                return resBelow.DUTImpedanceMilliOhms + (interpollFreqFactor * ImpedanceDelta);
            }
            else return resBelow.DUTImpedanceMilliOhms;
        }

        public double GetDUTPhaseDegrees(double frequency)
        {
            FRAResult resAbove = GetResultAboveFrequency(frequency);
            FRAResult resBelow = GetResultBelowFrequency(frequency);
            if (resAbove != resBelow)
            {
                double interpollFreqFactor = (frequency - resBelow.FrequencyHz) / (resAbove.FrequencyHz - resBelow.FrequencyHz);
                double dutPhaseDelta = resAbove.DUTPhaseDegrees - resBelow.DUTPhaseDegrees;
                return resBelow.DUTPhaseDegrees + (interpollFreqFactor * dutPhaseDelta);
            }
            else return resBelow.DUTPhaseDegrees;
        }

        public double GetDUTCapacitancePicoFarad(double frequency)
        {
            FRAResult resAbove = GetResultAboveFrequency(frequency);
            FRAResult resBelow = GetResultBelowFrequency(frequency);
            if (resAbove != resBelow)
            {
                double interpollFreqFactor = (frequency - resBelow.FrequencyHz) / (resAbove.FrequencyHz - resBelow.FrequencyHz);
                double CapacitanceDelta = resAbove.DUTCapacitancePicoFarad - resBelow.DUTCapacitancePicoFarad;
                return resBelow.DUTCapacitancePicoFarad + (interpollFreqFactor * CapacitanceDelta);
            }
            else return resBelow.DUTCapacitancePicoFarad;
        }

        public double GetDUT_ESR_MilliOhms(double frequency)
        {
            FRAResult resAbove = GetResultAboveFrequency(frequency);
            FRAResult resBelow = GetResultBelowFrequency(frequency);
            if (resAbove != resBelow)
            {
                double interpollFreqFactor = (frequency - resBelow.FrequencyHz) / (resAbove.FrequencyHz - resBelow.FrequencyHz);
                double ESR_Delta = resAbove.DUT_ESR_MilliOhms - resBelow.DUT_ESR_MilliOhms;
                return resBelow.DUT_ESR_MilliOhms + (interpollFreqFactor * ESR_Delta);
            }
            else return resBelow.DUT_ESR_MilliOhms;
        }

        public double GetDUTInductanceNanoHenry(double frequency)
        {
            FRAResult resAbove = GetResultAboveFrequency(frequency);
            FRAResult resBelow = GetResultBelowFrequency(frequency);
            if (resAbove != resBelow)
            {
                double interpollFreqFactor = (frequency - resBelow.FrequencyHz) / (resAbove.FrequencyHz - resBelow.FrequencyHz);
                double InductanceDelta = resAbove.DUTInductanceNanoHenry - resBelow.DUTInductanceNanoHenry;
                return resBelow.DUTInductanceNanoHenry + (interpollFreqFactor * InductanceDelta);
            }
            else return resBelow.DUTInductanceNanoHenry;
        }

        public double GetDUT_Q_Capacitor(double frequency)
        {
            FRAResult resAbove = GetResultAboveFrequency(frequency);
            FRAResult resBelow = GetResultBelowFrequency(frequency);
            if (resAbove != resBelow)
            {
                double interpollFreqFactor = (frequency - resBelow.FrequencyHz) / (resAbove.FrequencyHz - resBelow.FrequencyHz);
                double Q_Delta = resAbove.DUT_QCapacitor - resBelow.DUT_QCapacitor;
                return resBelow.DUT_QCapacitor + (interpollFreqFactor * Q_Delta);
            }
            else return resBelow.DUT_QCapacitor;
        }

        public double GetDUT_Q_Inductor(double frequency)
        {
            FRAResult resAbove = GetResultAboveFrequency(frequency);
            FRAResult resBelow = GetResultBelowFrequency(frequency);
            if (resAbove != resBelow)
            {
                double interpollFreqFactor = (frequency - resBelow.FrequencyHz) / (resAbove.FrequencyHz - resBelow.FrequencyHz);
                double Q_Delta = resAbove.DUT_QInductor - resBelow.DUT_QInductor;
                return resBelow.DUT_QInductor + (interpollFreqFactor * Q_Delta);
            }
            else return resBelow.DUT_QInductor;
        }

        public string GetGainPhaseInfo(double frequency, bool logFrequencyAxis)
        {
            string sFrequency = "Frequency: " + frequency.ToString(FRAResult.GetFrequencyFormat(frequency, logFrequencyAxis));
            double gain = GetGainDB(frequency);
            string sGain = "        Gain: " + gain.ToString(FRAResult.GetGainFormat(gain));
            double phase = GetPhaseDegrees(frequency);
            string sPhase = "        Phase: " + phase.ToString(FRAResult.GetPhaseFormat(phase));
            return sFrequency + sGain + sPhase;
        }
        public string GetImpedanceInfo(double frequency, bool logFrequencyAxis)
        {
            string sFrequency = "Frequency: " + frequency.ToString(FRAResult.GetFrequencyFormat(frequency, logFrequencyAxis));
            double impedance = GetDUTImpedanceMilliOmhs(frequency);
            string sImpedance = "        Impedance: " + impedance.ToString(FRAResult.GetImpedanceFormat(impedance));
            double phase = GetPhaseDegrees(frequency);
            string sPhase = "        Phase: " + phase.ToString(FRAResult.GetPhaseFormat(phase));
            return sFrequency + sImpedance + sPhase;
        }
        public string GetCapacitanceInfo(double frequency, bool logFrequencyAxis)
        {
            string sFrequency = "Frequency: " + frequency.ToString(FRAResult.GetFrequencyFormat(frequency, logFrequencyAxis));
            double capacitance = GetDUTCapacitancePicoFarad(frequency);
            string sCapacitance = "        Capacitance: " + capacitance.ToString(FRAResult.GetCapacitanceFormat(capacitance));
            double esr = GetDUT_ESR_MilliOhms(frequency);
            string sESR = "        ESR: " + esr.ToString(FRAResult.GetImpedanceFormat(esr));
            double qFactor = GetDUT_Q_Capacitor(frequency);
            string sQ = "       Q: " + qFactor.ToString(FRAResult.GetQFactorFormat(qFactor));
            return sFrequency + sCapacitance + sESR + sQ;
        }
        public string GetInducductanceInfo(double frequency, bool logFrequencyAxis)
        {
            string sFrequency = "Frequency: " + frequency.ToString(FRAResult.GetFrequencyFormat(frequency, logFrequencyAxis));
            double inductance = GetDUTInductanceNanoHenry(frequency);
            string sInductance = "        Inductance: " + inductance.ToString(FRAResult.GetInductanceFormat(inductance));
            double esr = GetDUT_ESR_MilliOhms(frequency);
            string sESR = "        ESR: " + esr.ToString(FRAResult.GetImpedanceFormat(esr));
            double qFactor = GetDUT_Q_Inductor(frequency);
            string sQ = "       Q: " + qFactor.ToString(FRAResult.GetQFactorFormat(qFactor));
            return sFrequency + sInductance + sESR + sQ;
        }

        #endregion

        #region Properties

        private double m_ReferenceResistor;
        public double ReferenceResistorOhms
        {
            get { return m_ReferenceResistor; }
        }

        private FRAFileType m_FRAFileType;
        public FRAFileType FRAFileType
        {
            get { return m_FRAFileType; }
        }

        private string m_FilePath;
        public string FilePath
        {
            get { return m_FilePath; }
        }

        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(FilePath)) return DateTime.Now.ToString();
                else return Path.GetFileNameWithoutExtension(FilePath);
            }
        }

        private DateTime m_DateTimeCreated;
        public DateTime DateTimeCreated
        {
            get { return m_DateTimeCreated; }
        }

        public void SaveAs(FRAFileType fileType, string path)
        {
            switch (fileType)
            {
                case FRAFileType.FRA4PicoScope:
                    WriteFile(path, FRATableFormat.CSV_FRA4PicoScope);
                    m_FilePath = path;
                    UpdateSerieNames();
                    break;
                case FRAFileType.Keysight:
                    WriteFile(path, FRATableFormat.CSV_Keysight);
                    m_FilePath = path;
                    UpdateSerieNames();
                    break;
                case FRAFileType.RhodeSchwarz:
                    WriteFile(path, FRATableFormat.CSV_RhodeSchwarz);
                    m_FilePath = path;
                    UpdateSerieNames();
                    break;
                default:
                    logService.Warn("No file could be created for fileType" + fileType.ToString());
                    break;
            }
        }

        private void WriteFile(string path, FRATableFormat tableFormat)
        {
            using (StreamWriter wr = new StreamWriter(path))
            {
                wr.AutoFlush = true;
                wr.WriteLine(FRAResult.ToStringTitles(tableFormat));
                foreach (FRAResult result in m_FRAResults) wr.WriteLine(result.ToStringValues(tableFormat));
            }
        }

        public double MinFrequencyHz
        {
            get { return m_MinFrequencyHz; }
        }
        public double MaxFrequencyHz
        {
            get { return m_MaxFrequencyHz; }
        }

        public double MinFrequencyHzRounded
        {
            get { return Math.Round(MinFrequencyHz, 0); }
        }
        public double MaxFrequencyHzRounded
        {
            get { return Math.Round(MaxFrequencyHz, 0); }
        }

        public double MinFrequencyHzLog
        {
            get
            {
                double log = Math.Log(MinFrequencyHz, 10);
                double logCeiling = Math.Ceiling(log) - 1;
                return Math.Pow(10, logCeiling);
            }
        }
        public double MaxFrequencyHzLog
        {
            get
            {
                double log = Math.Log(MaxFrequencyHz, 10);
                double logCeiling = Math.Ceiling(log);
                return Math.Pow(10, logCeiling);
            }
        }

        public double MinGainDB
        {
            get { return m_MinGainDB; }
        }
        public double MaxGainDB
        {
            get { return m_MaxGainDB; }
        }

        public double MinPhaseDegrees
        {
            get { return m_MinPhaseDegrees; }
        }
        public double MaxPhaseDegrees
        {
            get { return m_MaxPhaseDegrees; }
        }

        public double MinDUTImpedanceOhms
        {
            get { return m_MinDUTImpedanceOhms; }
        }
        public double MaxDUTImpedanceOhms
        {
            get { return m_MaxDUTImpedanceOhms; }
        }

        public double MinDUTPhaseDegrees
        {
            get { return m_MinDUTPhaseDegrees; }
        }
        public double MaxDUTPhaseDegrees
        {
            get { return m_MaxDUTPhaseDegrees; }
        }

        public double MinDUTCapacitancePicoFarad
        {
            get { return m_MinDUTCapacitancePicoFarad; }
        }
        public double MaxDUTCapacitancePicoFarad
        {
            get { return m_MaxDUTCapacitancePicoFarad; }
        }

        public double MinDUT_ESR_Ohms
        {
            get { return m_MinDUT_ESR_Ohms; }
        }
        public double MaxDUT_ESR_Ohms
        {
            get { return m_MaxDUT_ESR_Ohms; }
        }

        public double MinDUTInductanceNanoHenry
        {
            get { return m_MinDUTInductanceNanoHenry; }
        }
        public double MaxDUTInductanceNanoHenry
        {
            get { return m_MaxDUTInductanceNanoHenry; }
        }

        public double AverageGainDB
        {
            get
            {
                int count = 0;
                double sum = 0;
                foreach (FRAResult result in m_FRAResults)
                {
                    sum += result.GainFRA_DB;
                    count++;
                }
                if (count != 0) return sum / count;
                else return 0;
            }
        }

        public double AverageGain
        {
            get
            {
                int count = 0;
                double sum = 0;
                foreach (FRAResult result in m_FRAResults)
                {
                    sum += result.GainFRA;
                    count++;
                }
                if (count != 0) return sum / count;
                else return 0;
            }
        }

        public double AveragePhaseDegrees
        {
            get
            {
                int count = 0;
                double sum = 0;
                foreach (FRAResult result in m_FRAResults)
                {
                    sum += result.PhaseDegrees;
                    count++;
                }
                if (count != 0) return sum / count;
                else return 0;
            }
        }

        public double AveragePhaseRadians
        {
            get
            {
                int count = 0;
                double sum = 0;
                foreach (FRAResult result in m_FRAResults)
                {
                    sum += result.PhaseRadians;
                    count++;
                }
                if (count != 0) return sum / count;
                else return 0;
            }
        }

        public double AverageDUTImpedanceOhms
        {
            get
            {
                int count = 0;
                double sum = 0;
                foreach (FRAResult result in m_FRAResults)
                {
                    sum += result.DUTImpedanceMilliOhms;
                    count++;
                }
                if (count != 0) return sum / count;
                else return 0;
            }
        }

        public double AverageDUTPhaseDegrees
        {
            get
            {
                int count = 0;
                double sum = 0;
                foreach (FRAResult result in m_FRAResults)
                {
                    sum += result.DUTPhaseDegrees;
                    count++;
                }
                if (count != 0) return sum / count;
                else return 0;
            }
        }

        public double AverageDUTCapacitancePicoFarads
        {
            get
            {
                int count = 0;
                double sum = 0;
                foreach (FRAResult result in m_FRAResults)
                {
                    sum += result.DUTCapacitancePicoFarad;
                    count++;
                }
                if (count != 0) return sum / count;
                else return 0;
            }
        }

        public double AverageDUT_ESR_Ohms
        {
            get
            {
                int count = 0;
                double sum = 0;
                foreach (FRAResult result in m_FRAResults)
                {
                    sum += result.DUT_ESR_MilliOhms;
                    count++;
                }
                if (count != 0) return sum / count;
                else return 0;
            }
        }

        public double AverageDUTInductanceNanoHenry
        {
            get
            {
                int count = 0;
                double sum = 0;
                foreach (FRAResult result in m_FRAResults)
                {
                    sum += result.DUTInductanceNanoHenry;
                    count++;
                }
                if (count != 0) return sum / count;
                else return 0;
            }
        }

        #endregion

        #region Series & Series Info

        private Series m_GainDBSeries;
        public Series GainDBSeries
        {
            get { return m_GainDBSeries; }
        }

        private Series m_PhaseDegreesSeries;
        public Series PhaseDegreesSeries
        {
            get { return m_PhaseDegreesSeries; }
        }

        private Series m_DUTImpedanceMilliOhmSeries;
        public Series DUTImpedanceMilliOhmSeries
        {
            get { return m_DUTImpedanceMilliOhmSeries; }
        }

        private Series m_DUTPhaseDegreesSeries;
        public Series DUTPhaseDegreesSeries
        {
            get { return m_DUTPhaseDegreesSeries; }
        }

        private Series m_DUTCapacitancePicoFaredSeries;
        public Series DUTCapacitancePifoFaradSeries
        {
            get { return m_DUTCapacitancePicoFaredSeries; }
        }

        private Series m_DUT_ESRMilliOhmSeries;
        public Series DUT_ESRMilliOhmSeries
        {
            get { return m_DUT_ESRMilliOhmSeries; }
        }

        private Series m_DUTInductanceNanoHenrySeries;
        public Series DUTInductanceNanoHenrySeries
        {
            get { return m_DUTInductanceNanoHenrySeries; }
        }

        private void CreateLineSeries()
        {
            m_GainDBSeries = CreateLineSerie("Gain", AxisType.Primary);
            m_PhaseDegreesSeries = CreateLineSerie("Phase", AxisType.Secondary);
            m_DUTImpedanceMilliOhmSeries = CreateLineSerie("Impedance", AxisType.Primary);
            m_DUTPhaseDegreesSeries = CreateLineSerie("Phase", AxisType.Secondary);
            m_DUTCapacitancePicoFaredSeries = CreateLineSerie("Capacitance", AxisType.Primary);
            m_DUT_ESRMilliOhmSeries = CreateLineSerie("ESR", AxisType.Secondary);
            m_DUTInductanceNanoHenrySeries = CreateLineSerie("Inductance", AxisType.Primary);

            foreach (FRAResult result in m_FRAResults) // add results
            {
                m_GainDBSeries.Points.AddXY(result.FrequencyHz, result.GainFRA_DB);
                m_PhaseDegreesSeries.Points.AddXY(result.FrequencyHz, result.PhaseDegrees);
                m_DUTImpedanceMilliOhmSeries.Points.AddXY(result.FrequencyHz, result.DUTImpedanceMilliOhms);
                m_DUTPhaseDegreesSeries.Points.AddXY(result.FrequencyHz, result.DUTPhaseDegrees);
                m_DUTCapacitancePicoFaredSeries.Points.AddXY(result.FrequencyHz, result.DUTCapacitancePicoFarad);
                m_DUT_ESRMilliOhmSeries.Points.AddXY(result.FrequencyHz, result.DUT_ESR_MilliOhms);
                m_DUTInductanceNanoHenrySeries.Points.AddXY(result.FrequencyHz, result.DUTInductanceNanoHenry);
            }

            int numberOfMarkers = 50; // set marker step to ensure they are still visiable separately. +1 to ensure never zero (not allowed)
            m_GainDBSeries.MarkerStep = m_GainDBSeries.Points.Count / numberOfMarkers + 1;
            m_PhaseDegreesSeries.MarkerStep = m_PhaseDegreesSeries.Points.Count / numberOfMarkers + 1;
            m_DUTImpedanceMilliOhmSeries.MarkerStep = m_DUTImpedanceMilliOhmSeries.Points.Count / numberOfMarkers + 1;
            m_DUTPhaseDegreesSeries.MarkerStep = m_DUTPhaseDegreesSeries.Points.Count / numberOfMarkers + 1;
            m_DUTCapacitancePicoFaredSeries.MarkerStep = m_DUTCapacitancePicoFaredSeries.Points.Count / numberOfMarkers + 1;
            m_DUT_ESRMilliOhmSeries.MarkerStep = m_DUT_ESRMilliOhmSeries.Points.Count / numberOfMarkers + 1;
            m_DUTInductanceNanoHenrySeries.MarkerStep = m_DUTInductanceNanoHenrySeries.Points.Count / numberOfMarkers + 1;
        }

        private string GetSeriesName(string name)
        {
            return Guid.NewGuid().ToString();

            //if (string.IsNullOrEmpty(FilePath)) return  name + " : " + DateTimeCreated.ToString();
            //else return name + " : " + FilePath;
        }

        private string GetLegendName(string name)
        {
            return name + " : " + this.FileName;
        }

        private void UpdateSeriesNaming(string name, Series serie)
        {
            serie.Name = GetSeriesName(name);
            serie.LegendText = GetLegendName(name);
        }

        private void UpdateSerieNames()
        {
            UpdateSeriesNaming("Gain", m_GainDBSeries);
            UpdateSeriesNaming("Phase", m_PhaseDegreesSeries);
            UpdateSeriesNaming("Impedance", m_DUTImpedanceMilliOhmSeries);
            UpdateSeriesNaming("Phase", m_DUTPhaseDegreesSeries);
            UpdateSeriesNaming("Capacitance", m_DUTCapacitancePicoFaredSeries);
            UpdateSeriesNaming("ESR", m_DUT_ESRMilliOhmSeries);
            UpdateSeriesNaming("Inductance", m_DUTInductanceNanoHenrySeries);
            UpdateSeriesNaming("Gain", m_GainDBSeries);
        }

        private Series CreateLineSerie(string name, AxisType yAxisType)
        {
            Series serie = new Series(GetSeriesName(name));
            serie.LegendText = GetLegendName(name);
            serie.ChartType = SeriesChartType.Line;
            serie.YAxisType = yAxisType;
            if (yAxisType == AxisType.Primary)
            {
                serie.BorderDashStyle = ChartDashStyle.Solid;
                serie.MarkerStyle = MarkerStyle.None;
                serie.BorderWidth = 2;
            }
            else
            {
                serie.BorderDashStyle = ChartDashStyle.Dash;
                serie.MarkerStyle = MarkerStyle.Circle;
                serie.BorderWidth = 2;
                serie.MarkerSize = serie.BorderWidth * 4;
            }
            serie.Enabled = true;
            serie.XValueType = ChartValueType.Double;
            serie.YValueType = ChartValueType.Double;
            serie.IsVisibleInLegend = true;
            serie.ToolTip = FilePath;

            serie.Tag = this;
            return serie;
        }

        public string GetDataTable(FRATableFormat tableFormat)
        {
            using (StringWriter wr = new StringWriter())
            {
                wr.WriteLine(FRAResult.ToStringTitles(tableFormat));
                foreach (FRAResult result in m_FRAResults) wr.WriteLine(result.ToStringValues(tableFormat));
                return wr.ToString();
            }
        }

        #endregion
    }

    public enum FRAFileType
    {
        FRA4PicoScope,
        Keysight,
        RhodeSchwarz,
    }

    public enum FRATableFormat
    {
        CSV_FRA4PicoScope,
        CSV_Keysight,
        CSV_RhodeSchwarz,
        Excel_ALL

    }
}
