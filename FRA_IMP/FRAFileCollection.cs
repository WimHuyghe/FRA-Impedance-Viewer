using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using WHConsult.Utils.Log4Net;

namespace FRA_IMP
{
    public class FRAFileCollection : ICollection<FRAFile>
    {
        private ILogService logService;
        private List<FRAFile> m_Files;

        public FRAFileCollection()
        {
            logService = new FileLogService(typeof(FRAFileCollection));
            m_Files = new List<FRAFile>();
        }

        #region ICollection Interface

        public int Count
        {
            get { return m_Files.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Add(FRAFile item)
        {
            if (!ContainsFile(item.FilePath))
            {
                m_Files.Add(item);
                NotifyCollectionChanged(item, FRAFileChange.FileAdded);
            }
            else throw new Exception("Files with identical Series Path is already added in the collection");         
        }

        public void Clear()
        {
            while(m_Files.Count!=0) this.Remove(m_Files[0]);        
        }

        public bool Contains(FRAFile item)
        {
            return m_Files.Contains(item);
        }

        public void CopyTo(FRAFile[] array, int arrayIndex)
        {
            foreach (FRAFile file in m_Files) array.SetValue(file, arrayIndex);
        }

        public IEnumerator<FRAFile> GetEnumerator()
        {
            return ((IEnumerable<FRAFile>)m_Files).GetEnumerator();
        }

        public bool Remove(FRAFile item)
        {
            if (m_Files.Contains(item))
            {            
                m_Files.Remove(item);
                NotifyCollectionChanged(item, FRAFileChange.FileDeteted);
                return true;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Files.GetEnumerator();
        }

        #endregion

        #region Graphics

        public IList<Series> GetGainPhasePlotSeries()
        {
            List<Series> series = new List<Series>();
            foreach (FRAFile file in m_Files)
            {
                series.Add(file.GainDBSeries);
                series.Add(file.PhaseDegreesSeries);
            }
            return series;
        }

        public IList<Series> GetImpedancePlotSeries()
        {
            List<Series> series = new List<Series>();
            foreach (FRAFile file in m_Files)
            {
                series.Add(file.DUTImpedanceMilliOhmSeries);
                series.Add(file.DUTPhaseDegreesSeries);
            }
            return series;
        }

        public IList<Series> GetCapacitancePlotSeries()
        {
            List<Series> series = new List<Series>();
            foreach (FRAFile file in m_Files)
            {
                series.Add(file.DUTCapacitancePifoFaradSeries);
                series.Add(file.DUT_ESRMilliOhmSeries);
            }
            return series;
        }

        public IList<Series> GetInductancePlotSeries()
        {
            List<Series> series = new List<Series>();
            foreach (FRAFile file in m_Files)
            {
                series.Add(file.DUTCapacitancePifoFaradSeries);
                series.Add(file.DUT_ESRMilliOhmSeries);
            }
            return series;
        }
        #endregion

        #region Properties & Functions on entire collection

        public double MinFrequencyHz
        {
            get
            {
                double result = double.MaxValue;
                foreach (FRAFile file in m_Files) if (file.MinFrequencyHz < result) result = file.MinFrequencyHz;
                return result;
            }
        }
        public double MaxFrequencyHz
        {
            get
            {
                double result = double.MinValue;
                foreach (FRAFile file in m_Files) if (file.MaxFrequencyHz > result) result = file.MaxFrequencyHz;
                return result;
            }
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
            get
            {
                double result = double.MaxValue;
                foreach (FRAFile file in m_Files) if (file.MinGainDB < result) result = file.MinGainDB;
                return result;
            }
        }
        public double MaxGainDB
        {
            get
            {
                double result = double.MinValue;
                foreach (FRAFile file in m_Files) if (file.MaxGainDB > result) result = file.MaxGainDB;
                return result;
            }
        }

        public double MinPhaseDegrees
        {
            get
            {
                double result = double.MaxValue;
                foreach (FRAFile file in m_Files) if (file.MinPhaseDegrees < result) result = file.MinPhaseDegrees;
                return result;
            }
        }
        public double MaxPhaseDegrees
        {
            get
            {
                double result = double.MinValue;
                foreach (FRAFile file in m_Files) if (file.MaxPhaseDegrees > result) result = file.MaxPhaseDegrees;
                return result;
            }
        }

        public double MinDUTImpedanceOhms
        {
            get
            {
                double result = double.MaxValue;
                foreach (FRAFile file in m_Files) if (file.MinDUTImpedanceOhms < result) result = file.MinDUTImpedanceOhms;
                return result;
            }
        }
        public double MaxDUTImpedanceOhms
        {
            get
            {
                double result = double.MinValue;
                foreach (FRAFile file in m_Files) if (file.MaxDUTImpedanceOhms > result) result = file.MaxDUTImpedanceOhms;
                return result;
            }
        }

        public double MinDUTPhaseDegrees
        {
            get
            {
                double result = double.MaxValue;
                foreach (FRAFile file in m_Files) if (file.MinDUTPhaseDegrees < result) result = file.MinDUTPhaseDegrees;
                return result;
            }
        }
        public double MaxDUTPhaseDegrees
        {
            get
            {
                double result = double.MinValue;
                foreach (FRAFile file in m_Files) if (file.MaxDUTPhaseDegrees > result) result = file.MaxDUTPhaseDegrees;
                return result;
            }
        }

        public double MinDUTCapacitancePicoFarad
        {
            get
            {
                double result = double.MaxValue;
                foreach (FRAFile file in m_Files) if (file.MinDUTCapacitancePicoFarad < result) result = file.MinDUTCapacitancePicoFarad;
                return result;
            }
        }
        public double MaxDUTCapacitancePicoFarad
        {
            get
            {
                double result = double.MinValue;
                foreach (FRAFile file in m_Files) if (file.MaxDUTCapacitancePicoFarad > result) result = file.MaxDUTCapacitancePicoFarad;
                return result;
            }
        }

        public double MinDUT_ESR_Ohms
        {
            get
            {
                double result = double.MaxValue;
                foreach (FRAFile file in m_Files) if (file.MinDUT_ESR_Ohms < result) result = file.MinDUT_ESR_Ohms;
                return result;
            }
        }
        public double MaxDUT_ESR_Ohms
        {
            get
            {
                double result = double.MinValue;
                foreach (FRAFile file in m_Files) if (file.MaxDUT_ESR_Ohms > result) result = file.MaxDUT_ESR_Ohms;
                return result;
            }
        }

        public double MinDUTInductanceNanoHenry
        {
            get
            {
                double result = double.MaxValue;
                foreach (FRAFile file in m_Files) if (file.MinDUTInductanceNanoHenry < result) result = file.MinDUTInductanceNanoHenry;
                return result;
            }
        }
        public double MaxDUTInductanceNanoHenry
        {
            get
            {
                double result = double.MinValue;
                foreach (FRAFile file in m_Files) if (file.MaxDUTInductanceNanoHenry > result) result = file.MaxDUTInductanceNanoHenry;
                return result;
            }
        }

        public double AverageGainDB
        {
            get
            {
                int count = 0;
                double sum = 0;
                foreach (FRAFile result in m_Files)
                {
                    sum += result.AverageGainDB;
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
                foreach (FRAFile result in m_Files)
                {
                    sum += result.AverageGain;
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
                foreach (FRAFile result in m_Files)
                {
                    sum += result.AveragePhaseDegrees;
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
                foreach (FRAFile result in m_Files)
                {
                    sum += result.AveragePhaseRadians;
                    count++;
                }
                if (count != 0) return sum / count;
                else return 0;
            }
        }

        public double AverageDUTImpedanceMilliOhms
        {
            get
            {
                int count = 0;
                double sum = 0;
                foreach (FRAFile result in m_Files)
                {
                    sum += result.AverageDUTImpedanceOhms;
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
                foreach (FRAFile result in m_Files)
                {
                    sum += result.AverageDUTPhaseDegrees;
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
                foreach (FRAFile result in m_Files)
                {
                    sum += result.AverageDUTCapacitancePicoFarads;
                    count++;
                }
                if (count != 0) return sum / count;
                else return 0;
            }
        }

        public double AverageDUT_ESR_MilliOhms
        {
            get
            {
                int count = 0;
                double sum = 0;
                foreach (FRAFile result in m_Files)
                {
                    sum += result.AverageDUT_ESR_Ohms;
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
                foreach (FRAFile result in m_Files)
                {
                    sum += result.AverageDUTInductanceNanoHenry;
                    count++;
                }
                if (count != 0) return sum / count;
                else return 0;
            }
        }

        public string DataTables
        {
            get
            {
                StringWriter writer = new StringWriter();
                foreach (FRAFile file in m_Files) writer.WriteLine(file.GetDataTable());
                return writer.ToString();
            }
        }

        public string GetGainPhaseInfo(double frequency, bool logFrequencyAxis)
        {
            StringWriter writer = new StringWriter();
            foreach (FRAFile file in m_Files) writer.WriteLine(file.GetGainPhaseInfo(frequency, logFrequencyAxis) + "     (" + file.FileName + ")");
            return writer.ToString();
        }

        public string GetImpedanceInfo(double frequency, bool logFrequencyAxis)
        {
            StringWriter writer = new StringWriter();
            foreach (FRAFile file in m_Files) if (file.ReferenceResistorOhms != 0) writer.WriteLine(file.GetImpedanceInfo(frequency, logFrequencyAxis) + "     (" + file.FileName + ")");
            return writer.ToString();
        }

        public string GetCapacitanceInfo(double frequency, bool logFrequencyAxis)
        {
            StringWriter writer = new StringWriter();
            foreach (FRAFile file in m_Files) if (file.ReferenceResistorOhms != 0) writer.WriteLine(file.GetCapacitanceInfo(frequency, logFrequencyAxis) + "       (" + file.FileName + ")");
            return writer.ToString();
        }

        public string GetInductanceInfo(double frequency, bool logFrequencyAxis)
        {
            StringWriter writer = new StringWriter();
            foreach (FRAFile file in m_Files) if(file.ReferenceResistorOhms!=0)writer.WriteLine(file.GetInducductanceInfo(frequency, logFrequencyAxis) + "     (" + file.FileName + ")");
            return writer.ToString();
        }

        #endregion

        #region File Management

        public void NewFile(string path, FRAFileType fileType, double referenceResistor)
        {
            logService.Info("New file: " + path);
            this.Clear();
            this.Add(new FRAFile(path, fileType, referenceResistor));
        }

        public bool OpenFile(string path, FRAFileType fileType, double referenceResistor)
        {
            logService.Info("Open file: " + path);
            if (!ContainsFile(path))
            {
                this.Add(new FRAFile(path, fileType, referenceResistor));
                return true;
            }
            else return false; 
        }

        public event EventHandler<FRAFileEventArgs> CollectionChanged;
        private void NotifyCollectionChanged(FRAFile file, FRAFileChange change)
        {
            FRAFileEventArgs eventArgs = new FRAFileEventArgs(file, change);
            if (CollectionChanged != null) CollectionChanged.Invoke(this, eventArgs);
        } 

        public bool ContainsFile(string path)
        {
            bool result = false;
            foreach (FRAFile file in m_Files) if (file.FilePath.Equals(path)) result = true;
            return result;
        }

        #endregion
    }
}


