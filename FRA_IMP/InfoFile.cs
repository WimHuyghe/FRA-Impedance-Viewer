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
    public partial class InfoFile : Form
    {
        public InfoFile(FRAFile file)
        {
            InitializeComponent();
            this.TopMost = true;
            textBoxFileName.Text = file.FileName;
            textBoxPath.Text = file.FilePath;
            textBoxFileType.Text = file.FRAFileType.ToString();
            textBoxReferenceResistor.Text = file.ReferenceResistorOhms.ToString(FRAResult.GetImpedanceFormat(file.ReferenceResistorOhms*1000));
            textBoxMinFrequency.Text = file.MinFrequencyHz.ToString(FRAResult.GetFrequencyFormat(file.MinFrequencyHz,true));
            textBoxMaxFrequency.Text = file.MaxFrequencyHz.ToString(FRAResult.GetFrequencyFormat(file.MaxFrequencyHz, true));
            textBoxNrOfDataPoints.Text = file.Count.ToString();
            textBoxAverageGain.Text = file.AverageGainDB.ToString(FRAResult.GetGainFormat(file.AverageGainDB));   
            textBoxMaxGain.Text = file.MaxGainDB.ToString(FRAResult.GetGainFormat(file.MaxGainDB));
            textBoxMinGain.Text = file.MinGainDB.ToString(FRAResult.GetGainFormat(file.MinGainDB));
            textBoxMaxImpedance.Text = file.MaxDUTImpedanceOhms.ToString(FRAResult.GetImpedanceFormat(file.MaxDUTImpedanceOhms));
            textBoxMinImpedance.Text = file.MinDUTImpedanceOhms.ToString(FRAResult.GetImpedanceFormat(file.MinDUTImpedanceOhms));
            textBoxMaxCapacitance.Text = file.MaxDUTCapacitancePicoFarad.ToString(FRAResult.GetCapacitanceFormat(file.MaxDUTCapacitancePicoFarad));
            textBoxMinCapacitance.Text = file.MinDUTCapacitancePicoFarad.ToString(FRAResult.GetCapacitanceFormat(file.MinDUTCapacitancePicoFarad));
            textBoxMaxInductance.Text = file.MaxDUTInductanceNanoHenry.ToString(FRAResult.GetInductanceFormat(file.MaxDUTInductanceNanoHenry));
            textBoxMinInductance.Text = file.MinDUTInductanceNanoHenry.ToString(FRAResult.GetInductanceFormat(file.MinDUTInductanceNanoHenry));
            textBoxMax_ESR.Text = file.MaxDUT_ESR_Ohms.ToString(FRAResult.GetImpedanceFormat(file.MaxDUT_ESR_Ohms));
            textBoxMin_ESR.Text = file.MinDUT_ESR_Ohms.ToString(FRAResult.GetImpedanceFormat(file.MinDUT_ESR_Ohms));
        }
    }
}
