using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WHConsult.Utils.Log4Net;
using WHConsult.Utils.Settings;
using WHConsult.Utils.Settings.IniFiles;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;

namespace FRA_IMP
{
    public partial class FormMain : Form
    {
        private Color cursorColor = Color.Black;
        private Color lineGridColor = Color.LightGray;
        private bool m_logFrequencyAxis;
        private bool mainFormActive = true;

        private ILogService logService;
        private FRAFileCollection m_Files;

        #region Initialization

        public FormMain()
        {
            logService = new FileLogService(typeof(FormMain));
            logService.Info("Starting FRAImpedance...");
            InitializeComponent();
            InitSettingsController();//must be initialized before settings are used.
            WindowState = FormWindowState.Maximized;
            this.Text = "FRA Impedance Viewer";
            m_Files = new FRAFileCollection();
            m_Files.CollectionChanged += m_Files_CollectionChanged;
            m_logFrequencyAxis = CurrentSettings.Instance.LogaritmicFrequencyAxis;
            InitCharts();
        }

        private void InitSettingsController()
        {
            // first time program is run, set default folders (default is different depending on operating system)
            if (Properties.Settings.Default.PathSettingsFile.Equals(""))
            {
                string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                Properties.Settings.Default.PathSettingsFile = Path.Combine(defaultPath, "FRA_IMP\\default.set");
                logService.Info("Path settings file set to: " + Properties.Settings.Default.PathSettingsFile);
                Properties.Settings.Default.Save();
            }

            if (File.Exists(Properties.Settings.Default.PathSettingsFile))
            {
                IniSettingsController.Instance.LoadFile("Settings", Properties.Settings.Default.PathSettingsFile);
                logService.Info("Settings file loaded: " + Properties.Settings.Default.PathSettingsFile);
            }
            else
            {
                IniSettingsController.Instance.CreateFile("Settings", Properties.Settings.Default.PathSettingsFile);
                logService.Info("Settings file created: " + Properties.Settings.Default.PathSettingsFile);
            }
        }

        private void m_Files_CollectionChanged(object sender, FRAFileEventArgs e)
        {
            // open is only allowed is a file is already present.
            if (m_Files.Count != 0) openToolStripMenuItem.Enabled = true;
            else openToolStripMenuItem.Enabled = false;

            UpdateCharts(e.File, e.Change);
        }

        private void InitCharts()
        {
            logService.Debug("Starting with initialization of charts");
            InitChartGainPhase();
            InitChartImpedance();
            InitChartCapacitance();
            InitChartInductance();
            logService.Debug("Initialization of charts finished");
        }

        private void SetChartDefaults(Chart chart)
        {
            //chart.Palette = ChartColorPalette.Bright;

            chart.Palette = ChartColorPalette.None; // group colours by 2, so both primary and secondarry axis are in same colour (secondary has markers and is also dashed)
            chart.PaletteCustomColors = new Color[] {  Color.Blue,Color.Blue,Color.Red, Color.Red, Color.DarkGreen, Color.DarkGreen, Color.DarkViolet, Color.DarkViolet,
                Color.DarkOrange, Color.DarkOrange, Color.DarkCyan,Color.DarkCyan, Color.DarkGoldenrod, Color.DarkGoldenrod,Color.DeepPink, Color.DeepPink,Color.Fuchsia, Color.Fuchsia,
            Color.LightBlue,Color.LightBlue, Color.LightPink,Color.LightPink,Color.LightGreen,Color.LightGreen,Color.LightCoral, Color.LightCoral};

            // Legend
            chart.Legends.Add("Default");
            SetChartLegendPosition(chart);

            // Cursors
            chart.ChartAreas[0].CursorX.Interval = 0;
            chart.ChartAreas[0].CursorX.LineColor = Color.Black;
            chart.ChartAreas[0].CursorX.LineDashStyle = ChartDashStyle.Dot;

            chart.ChartAreas[0].CursorY.Interval = 0;
            chart.ChartAreas[0].CursorY.LineColor = Color.Black;
            chart.ChartAreas[0].CursorY.LineDashStyle = ChartDashStyle.Dot;
        }

        private void SetChartLegendPosition(Chart chart)
        {
            if (CurrentSettings.Instance.DisplayLegendInChart)
            {
                chart.Legends[0].IsDockedInsideChartArea = true;
                chart.Legends[0].DockedToChartArea = "Default";
                chart.Legends[0].Docking = Docking.Left;
                chart.Legends[0].TableStyle = LegendTableStyle.Tall;
            }
            else
            {
                chart.Legends[0].IsDockedInsideChartArea = false;
                chart.Legends[0].DockedToChartArea = "Default";
                chart.Legends[0].Docking = Docking.Right;
                chart.Legends[0].TableStyle = LegendTableStyle.Auto;
            }
        }

        private void InitChartGainPhase()
        {
            logService.Debug("InitChartGainPhase");
            chartGainPhase.ChartAreas.Add(new ChartArea("Default"));
            SetChartDefaults(chartGainPhase);

            // Events
            chartGainPhase.MouseWheel += ChartGainPhase_MouseWheel;
            chartGainPhase.MouseMove += ChartGainPhase_MouseMove;
            chartGainPhase.MouseDown += ChartGainPhase_MouseDown;
            chartGainPhase.MouseEnter += ChartGainPhase_MouseEnter;
            chartGainPhase.KeyDown += ChartGainPhase_KeyDown;
        }

        private void InitChartImpedance()
        {
            logService.Debug("InitChartImpedance");
            chartImpedance.ChartAreas.Add(new ChartArea("Default"));
            SetChartDefaults(chartImpedance);

            // Events
            chartImpedance.MouseWheel += ChartImpedance_MouseWheel;
            chartImpedance.MouseMove += ChartImpedance_MouseMove;
            chartImpedance.MouseDown += ChartImpedance_MouseDown;
            chartImpedance.MouseEnter += ChartImpedance_MouseEnter;
            chartImpedance.KeyDown += ChartImpedance_KeyDown;
        }

        private void InitChartCapacitance()
        {
            logService.Debug("InitChartCapacitance");
            chartCapacitance.ChartAreas.Add(new ChartArea("Default"));
            SetChartDefaults(chartCapacitance);

            // Events
            chartCapacitance.MouseWheel += ChartCapacitance_MouseWheel;
            chartCapacitance.MouseMove += ChartCapacitance_MouseMove;
            chartCapacitance.MouseDown += ChartCapacitance_MouseDown;
            chartCapacitance.MouseEnter += ChartCapacitance_MouseEnter;
            chartCapacitance.KeyDown += ChartCapacitance_KeyDown;
        }

        private void InitChartInductance()
        {
            logService.Debug("InitChartInductance");
            chartInductance.ChartAreas.Add(new ChartArea("Default"));
            SetChartDefaults(chartInductance);

            // Events
            chartInductance.MouseWheel += ChartInductance_MouseWheel;
            chartInductance.MouseMove += ChartInductance_MouseMove;
            chartInductance.MouseDown += ChartInductance_MouseDown;
            chartInductance.MouseEnter += ChartInductance_MouseEnter;
            chartInductance.KeyDown += ChartInductance_KeyDown;
        }

        #endregion

        #region Toolstrip

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyImageToClipboard();
        }
        private void copyImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyImageToClipboard();
        }

        private void copyDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyDataTableToClipboard();
        }
        private void copyDataToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CopyDataTableToClipboard();
        }

        private void saveAsImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveImageAs();
        }
        private void saveImageAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveImageAs();
        }

        private void saveDataAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDataTableAs();
        }
        private void saveDataAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveDataTableAs();
        }

        private void fRA4PicoScopeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFRA_File(FRAFileType.FRA4PicoScope);
        }

        private void fRA4PicoScopeFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddFRA_File(FRAFileType.FRA4PicoScope);
        }

        private void keysightFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFRA_File(FRAFileType.Keysight);
        }

        private void keysightFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddFRA_File(FRAFileType.Keysight);
        }

        private void rhodeSwToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFRA_File(FRAFileType.RhodeSchwarz);
        }

        private void rhodeSchwarzFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFRA_File(FRAFileType.RhodeSchwarz);
        }

        private void picoscopeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MeasurePicoscope form = new MeasurePicoscope(m_Files);
            mainFormActive = false;
            form.Show();
        }

        private void shortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form infoShortCuts = new InfoShortCuts();
            infoShortCuts.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form infoInfoAbout = new InfoAbout();
            infoInfoAbout.Show();
        }

        private void testSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form infoTestSetup = new InfoTestSetup();
            infoTestSetup.Show();
        }

        private void logaritmicFrequencyAxisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_logFrequencyAxis)
            {
                logService.Info("Switch to linear frequency axis");
                m_logFrequencyAxis = false;
            }
            else
            {
                logService.Info("Switch to log frequency axis");
                m_logFrequencyAxis = true;
            }
            UpdateFrequencyAxi();
        }

        private void legendInChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentSettings.Instance.DisplayLegendInChart) CurrentSettings.Instance.DisplayLegendInChart = false;
            else CurrentSettings.Instance.DisplayLegendInChart = true;
            UpdateLegendPosition();
        }

        private void printChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintChart(GetDisplayedChart());
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintChart(GetDisplayedChart());
        }

        ColorDialog colourDialog;
        private void changeColourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colourDialog == null) colourDialog = new ColorDialog();
            colourDialog.AllowFullOpen = true;
            colourDialog.Color = GetDisplayedChart().Series[m_LegendItem.SeriesName].Color;
            if (colourDialog.ShowDialog() == DialogResult.OK && m_LegendItem != null)
            {
                logService.Info("Change colour of " + m_LegendItem.SeriesName);
                GetDisplayedChart().Series[m_LegendItem.SeriesName].Color = colourDialog.Color;
            }
        }

        private void changeLineWidhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string lineWidthInput = GetDisplayedChart().Series[m_LegendItem.SeriesName].BorderWidth.ToString();
            Int16 lineWidth;
            do
            {
                ShowInputDialog(ref lineWidthInput, "Please specify the line width" + Environment.NewLine + "(1 to 10 integer value)", "Line Width for Serie");
            } while ((!Int16.TryParse(lineWidthInput, out lineWidth)) || (lineWidth < 1) || (lineWidth > 10));

            if (m_LegendItem != null)
            {
                logService.Info("Change linewidth of " + m_LegendItem.SeriesName);
                GetDisplayedChart().Series[m_LegendItem.SeriesName].BorderWidth = lineWidth;
                GetDisplayedChart().Series[m_LegendItem.SeriesName].MarkerSize = lineWidth * 4;
            }
        }

        private void removeSeriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_LegendItem!= null)
            {
                Series serie = GetDisplayedChart().Series[m_LegendItem.SeriesName];
                FRAFile file = (FRAFile) serie.Tag;
                DialogResult dialogResult = DialogResult.Yes;
                if (!file.IsSavedFile) dialogResult = MessageBox.Show("Unsaved Measurements will be lost, do you want to continue", "Unsaved Measurements!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    logService.Info("Remove series from chart: " + m_LegendItem.SeriesName);
                    m_Files.Remove((FRAFile)serie.Tag);
                    return;
                }
                else return;
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_LegendItem != null)
            {
                logService.Debug("Display file info: " + m_LegendItem.SeriesName);
                Series serie = GetDisplayedChart().Series[m_LegendItem.SeriesName];
                InfoFile formInfoFile = new InfoFile((FRAFile)serie.Tag);
                formInfoFile.Show();
            }
        }

        private void ShowRightClickContextMenu(int posX, int posY)
        {
            logService.Debug("Show right click context menu on position X:" + posX + " Y:" + posY);
            logaritmicFrequencyAxisToolStripMenuItem.Checked = m_logFrequencyAxis;
            legendInChartToolStripMenuItem.Checked = CurrentSettings.Instance.DisplayLegendInChart;
            contextMenuStripRightClick.Show(this, new Point(posX, posY));//places the menu         
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formPreferences = new FormPreferences();
            formPreferences.Show();
        }

        #endregion

        #region Dialogs & interaction

        private static DialogResult ShowInputDialog(ref string input, string prompt, string title)
        {
            System.Drawing.Size size = new System.Drawing.Size(250, 120);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = title;

            System.Windows.Forms.Label label = new Label();
            label.Size = new System.Drawing.Size(size.Width - 10, 40);
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Location = new System.Drawing.Point(5, 5);
            label.Text = prompt;
            inputBox.Controls.Add(label);

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.TextAlign = HorizontalAlignment.Right;
            textBox.Location = new System.Drawing.Point(5, 50);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 80);
            inputBox.Controls.Add(okButton);

            inputBox.AcceptButton = okButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }

        private void NewFRA_File(FRAFileType fileType)
        {
            if (m_Files.ContrainsUnsavedFiles)
            {
                DialogResult dialogResult = MessageBox.Show("Unsaved Measurements will be lost, do you want to continue", "Unsaved Measurements!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    OpenFRA_File(true, fileType);
                    return;
                }
                else return;
            }
            OpenFRA_File(true, fileType);
        }

        private void AddFRA_File(FRAFileType fileType)
        {
            OpenFRA_File(false, fileType);
        }

        private void OpenFRA_File(bool newChart, FRAFileType fileType)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = CurrentSettings.Instance.PathLastMeasurementFile;
            dialog.Filter = "FRA File (*.csv)|*.csv";
            if (fileType == FRAFileType.FRA4PicoScope) dialog.Title = "Load FRA4PicoScope Export";
            if (fileType == FRAFileType.Keysight) dialog.Title = "Load Keysight Bode Plot Export";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                CurrentSettings.Instance.PathLastMeasurementFile = dialog.FileName;
                // request reference resitor value
                string refResistorInput = CurrentSettings.Instance.ReferenceResistor.ToString();
                double referenceResistor;
                do
                {
                    ShowInputDialog(ref refResistorInput, "Please specify the used reference resistor value" + Environment.NewLine + "(ohms)", "Reference Resistor Used?");
                } while ((!Double.TryParse(refResistorInput, out referenceResistor)) || (referenceResistor < 0));

                CurrentSettings.Instance.ReferenceResistor = referenceResistor; // remember ref resistor for next time
                this.Cursor = Cursors.WaitCursor; // file loading takes a while, show wait cursos while loading
                logService.Info("Add new file to plot: " + dialog.FileName + " with reference resistor:" + referenceResistor);
                if (newChart) m_Files.NewFile(dialog.FileName, fileType, referenceResistor);
                else
                {
                    if (!m_Files.ContainsFile(dialog.FileName)) m_Files.OpenFile(dialog.FileName, fileType, referenceResistor);
                    else MessageBox.Show("File Already Loaded in chart!", dialog.FileName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                this.Cursor = Cursors.Default;
            }
        }

        private void CopyImageToClipboard()
        {
            Chart chart = GetDisplayedChart();
            if (chart != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    logService.Debug("Copy chart to clipboard");
                    chart.SaveImage(ms, ChartImageFormat.Bmp);
                    Bitmap bm = new Bitmap(ms);
                    Clipboard.SetImage(bm);
                }
            }
        }

        private void SaveImageAs()
        {
            Chart chart = GetDisplayedChart();
            if (chart != null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.InitialDirectory = CurrentSettings.Instance.PathLastImageFile;
                dialog.Filter = "Image (*.png)|*.png";
                dialog.Title = "Save chart as";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    CurrentSettings.Instance.PathLastImageFile = dialog.FileName;
                    logService.Info("Saving image file: " + dialog.FileName);
                    chart.SaveImage(dialog.FileName, ChartImageFormat.Png);
                }
            }
        }

        private void CopyDataTableToClipboard()
        {
            logService.Debug("Copy dataTable to clipboard");
            Clipboard.SetText(m_Files.DataTableExcel);
        }

        private void SaveDataTableAs()
        {
            Chart chart = GetDisplayedChart();
            if (chart != null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.InitialDirectory = CurrentSettings.Instance.PathLastDataTableFile;
                dialog.Filter = "Textfile (*.txt)|*.txt";
                dialog.Title = "Save chart as";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    CurrentSettings.Instance.PathLastDataTableFile = dialog.FileName;
                    logService.Info("Saving datatable file: " + dialog.FileName);
                    StreamWriter file = new StreamWriter(dialog.FileName);
                    file.Write(m_Files.DataTableExcel);
                }
            }
        }

        private void PrintChart(Chart chart)
        {
            chart.Printing.PrintDocument.DefaultPageSettings.Landscape = true;
            chart.Printing.Print(true);
        }

        private void HandleKeyboardShortcuts(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Modifiers == (Keys.Control)) CopyImageToClipboard();
            if (e.KeyCode == Keys.C && e.Modifiers == (Keys.Control | Keys.Shift)) CopyDataTableToClipboard();
            if (e.KeyCode == Keys.S && e.Modifiers == (Keys.Control)) SaveImageAs();
            if (e.KeyCode == Keys.S && e.Modifiers == (Keys.Control | Keys.Shift)) SaveDataTableAs();
        }

        #endregion

        #region Chart Updates

        private Chart GetDisplayedChart()
        {
            if (tabControl1.SelectedIndex == 0) return chartGainPhase;
            if (tabControl1.SelectedIndex == 1) return chartImpedance;
            if (tabControl1.SelectedIndex == 2) return chartCapacitance;
            if (tabControl1.SelectedIndex == 3) return chartInductance;
            return null;
        }

        private void UpdateCharts(FRAFile file, FRAFileChange change)
        {
            logService.Debug("starting updating charts");
            UpdateGainPhaseChart(file, change);
            UpdateImpedanceChart(file, change);
            UpdateCapacitanceChart(file, change);
            UpdateInductanceChart(file, change);
            logService.Debug("updating charts finished");
        }

        private void UpdateGainPhaseChart(FRAFile file, FRAFileChange change)
        {
            if (change == FRAFileChange.FileAdded)
            {
                logService.Debug("File added to gain phase chart:" + file.FileName);
                chartGainPhase.Series.Add(file.GainDBSeries);
                chartGainPhase.Series.Add(file.PhaseDegreesSeries);
            }

            if (change == FRAFileChange.FileDeteted)
            {
                logService.Debug("File deleted from gain phase chart:" + file.FileName);
                chartGainPhase.Series.Remove(file.GainDBSeries);
                chartGainPhase.Series.Remove(file.PhaseDegreesSeries);
            }

            if (chartGainPhase.Series.Count != 0)
            {
                //Update frequency Axis
                UpdateFrequencyAxis(chartGainPhase, m_logFrequencyAxis);

                chartGainPhase.ChartAreas[0].AxisY.IsLogarithmic = false;
                chartGainPhase.ChartAreas[0].AxisY.LabelStyle.Format = FRAResult.GetGainFormat(m_Files.MaxGainDB);
                chartGainPhase.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
                chartGainPhase.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                chartGainPhase.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                chartGainPhase.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                chartGainPhase.ChartAreas[0].AxisY.MajorGrid.LineColor = lineGridColor;
                chartGainPhase.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;

                chartGainPhase.ChartAreas[0].AxisY2.IsLogarithmic = false;
                chartGainPhase.ChartAreas[0].AxisY2.LabelStyle.Format = FRAResult.GetPhaseFormat(m_Files.MaxPhaseDegrees);
                chartGainPhase.ChartAreas[0].AxisY2.IntervalAutoMode = IntervalAutoMode.VariableCount;

                chartGainPhase.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                chartGainPhase.ChartAreas[0].AxisY2.ScaleView.Zoomable = true;
                chartGainPhase.ChartAreas[0].AxisY2.MajorGrid.Enabled = false; // TODO: find a solution to make grids match 
                chartGainPhase.ChartAreas[0].AxisY2.MajorGrid.LineColor = lineGridColor;
                chartGainPhase.ChartAreas[0].AxisY2.MajorGrid.LineWidth = 1;

                try
                {
                    chartGainPhase.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                    chartGainPhase.ChartAreas[0].AxisY.ScaleView.ZoomReset();
                    chartGainPhase.ChartAreas[0].AxisY2.ScaleView.ZoomReset();
                    chartGainPhase.ChartAreas[0].RecalculateAxesScale();
                }
                catch (Exception ex)
                {
                    logService.Error("Failed to Reset Scaleview or Recalculate axis:" + ex.Message);
                }
            }
            else
            {
                chartGainPhase.ChartAreas[0].AxisX.IsLogarithmic = false; // when no series are added, the chart component crashes with a log axis
                chartGainPhase.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
                chartGainPhase.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;
                chartGainPhase.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
            }
        }

        private void UpdateImpedanceChart(FRAFile file, FRAFileChange change)
        {
            if (change == FRAFileChange.FileAdded && file.ReferenceResistorOhms != 0) // files without ref resistor do not have usefull RLC properties
            {
                logService.Debug("File added to impedance chart:" + file.FileName);
                chartImpedance.Series.Add(file.DUTImpedanceMilliOhmSeries);
                chartImpedance.Series.Add(file.DUTPhaseDegreesSeries);
            }

            if (change == FRAFileChange.FileDeteted)
            {
                logService.Debug("File deleted from impendance chart:" + file.FileName);
                chartImpedance.Series.Remove(file.DUTImpedanceMilliOhmSeries);
                chartImpedance.Series.Remove(file.DUTPhaseDegreesSeries);
            }

            if (chartImpedance.Series.Count != 0)
            {
                //Update frequency Axis
                UpdateFrequencyAxis(chartImpedance, m_logFrequencyAxis);

                chartImpedance.ChartAreas[0].AxisY.IsLogarithmic = false;
                chartImpedance.ChartAreas[0].AxisY.LabelStyle.Format = FRAResult.GetImpedanceFormat(m_Files.AverageDUTImpedanceMilliOhms);
                chartImpedance.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
                chartImpedance.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                chartImpedance.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                chartImpedance.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                chartImpedance.ChartAreas[0].AxisY.MajorGrid.LineColor = lineGridColor;
                chartImpedance.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;

                chartImpedance.ChartAreas[0].AxisY2.IsLogarithmic = false;
                chartImpedance.ChartAreas[0].AxisY2.LabelStyle.Format = FRAResult.GetPhaseFormat(m_Files.MaxPhaseDegrees);
                chartImpedance.ChartAreas[0].AxisY2.IntervalAutoMode = IntervalAutoMode.VariableCount;
                chartImpedance.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                chartImpedance.ChartAreas[0].AxisY2.ScaleView.Zoomable = true;
                chartImpedance.ChartAreas[0].AxisY2.MajorGrid.Enabled = false; // TODO: find a solution to make grids match 
                chartImpedance.ChartAreas[0].AxisY2.MajorGrid.LineColor = lineGridColor;
                chartImpedance.ChartAreas[0].AxisY2.MajorGrid.LineWidth = 1;
                try
                {
                    chartImpedance.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                    chartImpedance.ChartAreas[0].AxisY.ScaleView.ZoomReset();
                    chartImpedance.ChartAreas[0].AxisY2.ScaleView.ZoomReset();
                    chartImpedance.ChartAreas[0].RecalculateAxesScale();
                }
                catch (Exception ex)
                {
                    logService.Error("Failed to Reset Scaleview or Recalculate axis:" + ex.Message);
                }
            }
            else
            {
                chartImpedance.ChartAreas[0].AxisX.IsLogarithmic = false; // when no series are added, the chart component crashes with a log axis
                chartImpedance.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
                chartImpedance.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;
                chartImpedance.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;

            }
        }

        private void UpdateCapacitanceChart(FRAFile file, FRAFileChange change)
        {
            if (change == FRAFileChange.FileAdded && file.ReferenceResistorOhms != 0) // files without ref resistor do not have usefull RLC properties
            {
                logService.Debug("File added to capacitance chart:" + file.FileName);
                chartCapacitance.Series.Add(file.DUTCapacitancePifoFaradSeries);
                chartCapacitance.Series.Add(file.DUT_ESRMilliOhmSeries);
            }

            if (change == FRAFileChange.FileDeteted)
            {
                logService.Debug("File deleted from capacintance chart:" + file.FileName);
                chartCapacitance.Series.Remove(file.DUTCapacitancePifoFaradSeries);
                chartCapacitance.Series.Remove(file.DUT_ESRMilliOhmSeries);
            }

            if (chartCapacitance.Series.Count != 0)
            {
                //Update frequency Axis
                UpdateFrequencyAxis(chartCapacitance, m_logFrequencyAxis);

                chartCapacitance.ChartAreas[0].AxisY.IsLogarithmic = false;
                chartCapacitance.ChartAreas[0].AxisY.LabelStyle.Format = FRAResult.GetCapacitanceFormat(m_Files.AverageDUTCapacitancePicoFarads);
                chartCapacitance.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
                chartCapacitance.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                chartCapacitance.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                chartCapacitance.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                chartCapacitance.ChartAreas[0].AxisY.MajorGrid.LineColor = lineGridColor;
                chartCapacitance.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;

                chartCapacitance.ChartAreas[0].AxisY2.IsLogarithmic = false;
                chartCapacitance.ChartAreas[0].AxisY2.LabelStyle.Format = FRAResult.GetImpedanceFormat(m_Files.AverageDUT_ESR_MilliOhms);
                chartCapacitance.ChartAreas[0].AxisY2.IntervalAutoMode = IntervalAutoMode.VariableCount;
                chartCapacitance.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                chartCapacitance.ChartAreas[0].AxisY2.ScaleView.Zoomable = true;
                chartCapacitance.ChartAreas[0].AxisY2.MajorGrid.Enabled = false; // TODO: find a solution to make grids match 
                chartCapacitance.ChartAreas[0].AxisY2.MajorGrid.LineColor = lineGridColor;
                chartCapacitance.ChartAreas[0].AxisY2.MajorGrid.LineWidth = 1;

                chartCapacitance.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                chartCapacitance.ChartAreas[0].AxisY.ScaleView.ZoomReset();
                chartCapacitance.ChartAreas[0].AxisY2.ScaleView.ZoomReset();
                chartCapacitance.ChartAreas[0].RecalculateAxesScale();
            }
            else
            {
                try
                {
                    chartCapacitance.ChartAreas[0].AxisX.IsLogarithmic = false; // when no series are added, the chart component crashes with a log axis
                    chartCapacitance.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
                    chartCapacitance.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;
                    chartCapacitance.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
                }
                catch (Exception ex)
                {
                    logService.Error("Failed to Reset Scaleview or Recalculate axis:" + ex.Message);
                }
            }
        }

        private void UpdateInductanceChart(FRAFile file, FRAFileChange change)
        {
            if (change == FRAFileChange.FileAdded && file.ReferenceResistorOhms != 0) // files without ref resistor do not have usefull RLC properties
            {
                logService.Info("File added to inductance chart:" + file.FileName);
                chartInductance.Series.Add(file.DUTInductanceNanoHenrySeries);
                chartInductance.Series.Add(file.DUT_ESRMilliOhmSeries);
            }

            if (change == FRAFileChange.FileDeteted)
            {
                logService.Debug("File deleted from inductance chart:" + file.FileName);
                chartInductance.Series.Remove(file.DUTInductanceNanoHenrySeries);
                chartInductance.Series.Remove(file.DUT_ESRMilliOhmSeries);
            }

            if (chartInductance.Series.Count != 0)
            {
                //Update frequency Axis
                UpdateFrequencyAxis(chartInductance, m_logFrequencyAxis);

                chartInductance.ChartAreas[0].AxisY.IsLogarithmic = false;
                chartInductance.ChartAreas[0].AxisY.LabelStyle.Format = FRAResult.GetInductanceFormat(m_Files.AverageDUTInductanceNanoHenry);
                chartInductance.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
                chartInductance.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                chartInductance.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                chartInductance.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                chartInductance.ChartAreas[0].AxisY.MajorGrid.LineColor = lineGridColor;
                chartInductance.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;

                chartInductance.ChartAreas[0].AxisY2.IsLogarithmic = false;
                chartInductance.ChartAreas[0].AxisY2.LabelStyle.Format = FRAResult.GetImpedanceFormat(m_Files.AverageDUT_ESR_MilliOhms);
                chartInductance.ChartAreas[0].AxisY2.IntervalAutoMode = IntervalAutoMode.VariableCount;
                chartInductance.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                chartInductance.ChartAreas[0].AxisY2.ScaleView.Zoomable = true;
                chartInductance.ChartAreas[0].AxisY2.MajorGrid.Enabled = false; // TODO: find a solution to make grids match 
                chartInductance.ChartAreas[0].AxisY2.MajorGrid.LineColor = lineGridColor;
                chartInductance.ChartAreas[0].AxisY2.MajorGrid.LineWidth = 1;

                try
                {
                    chartInductance.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                    chartInductance.ChartAreas[0].AxisY.ScaleView.ZoomReset();
                    chartInductance.ChartAreas[0].AxisY2.ScaleView.ZoomReset();
                    chartInductance.ChartAreas[0].RecalculateAxesScale();
                }
                catch (Exception ex)
                {
                    logService.Error("Failed to Reset Scaleview or Recalculate axis:" + ex.Message);
                }
            }
            else
            {
                chartInductance.ChartAreas[0].AxisX.IsLogarithmic = false; // when no series are added, the chart component crashes with a log axis
                chartInductance.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
                chartInductance.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;
                chartInductance.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
            }
        }

        private void UpdateFrequencyAxi()
        {
            UpdateFrequencyAxis(chartGainPhase, m_logFrequencyAxis);
            UpdateFrequencyAxis(chartImpedance, m_logFrequencyAxis);
            UpdateFrequencyAxis(chartCapacitance, m_logFrequencyAxis);
            UpdateFrequencyAxis(chartInductance, m_logFrequencyAxis);
        }

        private void UpdateLegendPosition()
        {
            SetChartLegendPosition(chartGainPhase);
            SetChartLegendPosition(chartImpedance);
            SetChartLegendPosition(chartCapacitance);
            SetChartLegendPosition(chartInductance);
        }

        private void UpdateFrequencyAxis(Chart chart, bool logaritmic)
        {
            chart.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;
            if (chart.Series.Count != 0)
            {
                if (logaritmic)
                {
                    chart.ChartAreas[0].AxisX.IsLogarithmic = true;
                    chart.ChartAreas[0].AxisX.LogarithmBase = 10;
                    // must also be set here because labels are otherwise incorrectly calculated
                    chart.ChartAreas[0].AxisX.LabelStyle.Format = FRAResult.GetFrequencyFormat(m_Files.MinFrequencyHz, logaritmic);
                    chart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
                    chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                    chart.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
                    chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 2;
                    chart.ChartAreas[0].AxisX.MajorGrid.LineColor = lineGridColor;
                    chart.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
                    chart.ChartAreas[0].AxisX.MinorGrid.Interval = 1;
                    chart.ChartAreas[0].AxisX.MinorGrid.LineWidth = 1;
                    chart.ChartAreas[0].AxisX.MinorGrid.LineColor = lineGridColor;
                    chart.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
                    chart.ChartAreas[0].AxisX.Minimum = m_Files.MinFrequencyHzRounded;
                    chart.ChartAreas[0].AxisX.Maximum = m_Files.MaxFrequencyHzRounded;
                }
                else
                {
                    chart.ChartAreas[0].AxisX.IsLogarithmic = false;
                    chart.ChartAreas[0].AxisX.LogarithmBase = 10;
                    // must also be set here because labels are otherwise incorrectly calculated
                    chart.ChartAreas[0].AxisX.LabelStyle.Format = FRAResult.GetFrequencyFormat(m_Files.MinFrequencyHz, logaritmic);
                    chart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                    chart.ChartAreas[0].AxisX.MajorGrid.Interval = m_Files.MaxFrequencyHzRounded / 20;
                    chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 2;
                    chart.ChartAreas[0].AxisX.MajorGrid.LineColor = lineGridColor;
                    chart.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
                    chart.ChartAreas[0].AxisX.MinorGrid.Interval = m_Files.MaxFrequencyHzRounded / 100;
                    chart.ChartAreas[0].AxisX.MinorGrid.LineWidth = 1;
                    chart.ChartAreas[0].AxisX.MinorGrid.LineColor = lineGridColor;
                    chart.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
                    chart.ChartAreas[0].AxisX.Minimum = 0;
                    chart.ChartAreas[0].AxisX.Maximum = m_Files.MaxFrequencyHzRounded;
                }
            }
        }

        LegendItem m_LegendItem;
        private void LegendClick(Chart chart, object sender, MouseEventArgs e)
        {
            HitTestResult result = chart.HitTest(e.X, e.Y);
            if (result != null && result.Object != null)
            {
                // When user hits the LegendItem
                if (result.Object is LegendItem)
                {
                    // Legend item result
                    m_LegendItem = (LegendItem)result.Object;
                    contextMenuStripLegendClick.Show(this, new Point(e.X, e.Y));
                }
            }
        }

        #endregion

        #region charts events & zoom

        private int zoomXGainPhase = 0, zoomYGainPhase = 0, zoomY2GainPhase = 0;
        private void ChartGainPhase_MouseWheel(object sender, MouseEventArgs e)
        {
            ChartZoom((Chart)sender, e, ref zoomXGainPhase, ref zoomYGainPhase, ref zoomY2GainPhase);
        }
        private int zoomXImpedance = 0, zoomYImpedance = 0, zoomY2Impedance = 0;
        private void ChartImpedance_MouseWheel(object sender, MouseEventArgs e)
        {
            ChartZoom((Chart)sender, e, ref zoomXImpedance, ref zoomYImpedance, ref zoomY2Impedance);
        }
        private int zoomXCapacitance = 0, zoomYCapacitance = 0, zoomY2Capacitance = 0;
        private void ChartCapacitance_MouseWheel(object sender, MouseEventArgs e)
        {
            ChartZoom((Chart)sender, e, ref zoomXCapacitance, ref zoomYCapacitance, ref zoomY2Capacitance);
        }
        private int zoomXInductance = 0, zoomYInductance = 0, zoomY2Inductance = 0;
        private void ChartInductance_MouseWheel(object sender, MouseEventArgs e)
        {
            ChartZoom((Chart)sender, e, ref zoomXInductance, ref zoomYInductance, ref zoomY2Inductance);
        }
        private void ChartZoom(Chart chart, MouseEventArgs e, ref int zoomX, ref int zoomY, ref int zoomY2)
        {
            if (e.Location.X >= 0 && e.Location.Y >= 0)
            {
                double zoomFactor = 0.1;   //0 to 1 = 0% to 100% Every Wheel Tick.

                Axis xAxis = chart.ChartAreas[0].AxisX;
                Axis yAxis = chart.ChartAreas[0].AxisY;
                Axis y2Axis = chart.ChartAreas[0].AxisY2;

                // reset zoom count when user has reset zoom on scroll bars
                if (!xAxis.ScaleView.IsZoomed) zoomX = 0;
                if (!yAxis.ScaleView.IsZoomed) zoomY = 0;
                if (!y2Axis.ScaleView.IsZoomed) zoomY2 = 0;

                double mousePointerPosX = xAxis.PixelPositionToValue(e.Location.X);
                double mousePointerPosY = yAxis.PixelPositionToValue(e.Location.Y);
                double mousePointerPosY2 = y2Axis.PixelPositionToValue(e.Location.Y);

                double currentXMin = xAxis.ScaleView.ViewMinimum;
                double currentXMax = xAxis.ScaleView.ViewMaximum;
                double currentYMin = yAxis.ScaleView.ViewMinimum;
                double currentYMax = yAxis.ScaleView.ViewMaximum;
                double currentY2Min = y2Axis.ScaleView.ViewMinimum;
                double currentY2Max = y2Axis.ScaleView.ViewMaximum;

                try
                {
                    if (e.Delta < 0) // => ZOOM IN
                    {
                        if (Control.ModifierKeys != Keys.Shift && Control.ModifierKeys != Keys.Control) // zoom out X-axis
                        {
                            if (zoomX > 1)
                            {
                                double newXMin = currentXMin - ((mousePointerPosX - currentXMin) * zoomFactor);
                                double newXMax = currentXMax + ((currentXMax - mousePointerPosX) * zoomFactor);
                                xAxis.ScaleView.Zoom(newXMin, newXMax);
                                zoomX -= 1;
                            }
                            else // reset zoom to remove scroll bars
                            {
                                xAxis.ScaleView.ZoomReset();
                                zoomX = 0;
                            }
                        }

                        if (Control.ModifierKeys != Keys.Shift && Control.ModifierKeys == Keys.Control) // zoom out Y1
                        {
                            if (zoomY >= 1) // zoom out Y-axis
                            {
                                double newYMin = currentYMin - ((mousePointerPosY - currentYMin) * zoomFactor);
                                double newYMax = currentYMax + ((currentYMax - mousePointerPosY) * zoomFactor);
                                yAxis.ScaleView.Zoom(newYMin, newYMax);
                                zoomY -= 1;
                            }
                            else // reset zoom to remove scroll bars
                            {
                                yAxis.ScaleView.ZoomReset();
                                zoomY = 0;
                            }
                        }

                        if (Control.ModifierKeys == Keys.Shift && Control.ModifierKeys != Keys.Control) // zoom out Y2-axis
                        {
                            if (zoomY2 >= 1) // zoom out Y2-axis
                            {
                                double newY2Min = currentY2Min - ((mousePointerPosY2 - currentY2Min) * zoomFactor);
                                double newY2Max = currentY2Max + ((currentY2Max - mousePointerPosY2) * zoomFactor);
                                y2Axis.ScaleView.Zoom(newY2Min, newY2Max);
                                zoomY2 -= 1;
                            }
                            else // reset zoom to remove scroll bars
                            {
                                y2Axis.ScaleView.ZoomReset();
                                zoomY2 = 0;
                            }
                        }
                    }

                    if (e.Delta > 0) // => ZOOM OUT
                    {
                        if (Control.ModifierKeys != Keys.Shift && Control.ModifierKeys != Keys.Control) // zoom in X-axis
                        {
                            double newXMin = currentXMin + ((mousePointerPosX - currentXMin) * zoomFactor);
                            double newXMax = currentXMax - ((currentXMax - mousePointerPosX) * zoomFactor);
                            xAxis.ScaleView.Zoom(newXMin, newXMax);
                            zoomX += 1;
                        }

                        if (Control.ModifierKeys != Keys.Shift && Control.ModifierKeys == Keys.Control) // zoom in Y-axis
                        {
                            double newYMin = currentYMin + ((mousePointerPosY - currentYMin) * zoomFactor);
                            double newYMax = currentYMax - ((currentYMax - mousePointerPosY) * zoomFactor);
                            yAxis.ScaleView.Zoom(newYMin, newYMax);
                            zoomY += 1;
                        }

                        if (Control.ModifierKeys == Keys.Shift && Control.ModifierKeys != Keys.Control) // zoom in Y2-axis
                        {
                            double newY2Min = currentY2Min + ((mousePointerPosY2 - currentY2Min) * zoomFactor);
                            double newY2Max = currentY2Max - ((currentY2Max - mousePointerPosY2) * zoomFactor);
                            y2Axis.ScaleView.Zoom(newY2Min, newY2Max);
                            zoomY2 += 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logService.Fatal("zooming failed:" + ex.ToString());
                }
            }
        }

        private void ChartGainPhase_MouseDown(object sender, MouseEventArgs e)
        {
            if (chartGainPhase.Series.Count != 0)
            {
                LegendClick(chartGainPhase, sender, e);

                if (e.Button == MouseButtons.Right) ShowRightClickContextMenu(e.X, e.Y);

                if (e.Button == MouseButtons.Left) ChartGainPhase_MouseMove(sender, e); // to show X cursor 
            }
        }
        private void ChartImpedance_MouseDown(object sender, MouseEventArgs e)
        {
            if (chartImpedance.Series.Count != 0)
            {
                LegendClick(chartImpedance, sender, e);

                if (e.Button == MouseButtons.Right) ShowRightClickContextMenu(e.X, e.Y);

                if (e.Button == MouseButtons.Left) ChartImpedance_MouseMove(sender, e);
            }
        }
        private void ChartCapacitance_MouseDown(object sender, MouseEventArgs e)
        {
            if (chartCapacitance.Series.Count != 0)
            {
                LegendClick(chartCapacitance, sender, e);

                if (e.Button == MouseButtons.Right) ShowRightClickContextMenu(e.X, e.Y);

                if (e.Button == MouseButtons.Left) ChartCapacitance_MouseMove(sender, e);
            }
        }
        private void ChartInductance_MouseDown(object sender, MouseEventArgs e)
        {
            if (chartInductance.Series.Count != 0)
            {
                LegendClick(chartInductance, sender, e);

                if (e.Button == MouseButtons.Right) ShowRightClickContextMenu(e.X, e.Y);

                if (e.Button == MouseButtons.Left) ChartInductance_MouseMove(sender, e);
            }
        }

        private void ChartGainPhase_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePoint = new Point(e.X, e.Y);
            if (chartGainPhase.Series.Count != 0 && chartGainPhase.Focused)
            {
                // always show frequency cursor
                chartGainPhase.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, false);

                double frequency;
                if (m_logFrequencyAxis) frequency = Math.Pow(10, chartGainPhase.ChartAreas[0].AxisX.PixelPositionToValue(mousePoint.X));    // freqeuncy is returned as log value because of log axis
                else frequency = chartGainPhase.ChartAreas[0].AxisX.PixelPositionToValue(mousePoint.X);

                chartGainPhase.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 10, FontStyle.Bold);
                if (e.Button != MouseButtons.Left) chartGainPhase.ChartAreas[0].AxisX.Title = m_Files.GetGainPhaseInfo(frequency, m_logFrequencyAxis); // show info based on frequency and series
                else
                {
                    //show info based on cursor
                    using (StringWriter writer = new StringWriter())
                    {
                        writer.Write("Frequency: " + frequency.ToString(FRAResult.GetFrequencyFormat(frequency, m_logFrequencyAxis)));
                        double gain = chartGainPhase.ChartAreas[0].AxisY.PixelPositionToValue(mousePoint.Y);
                        writer.Write("        Gain: " + gain.ToString(FRAResult.GetGainFormat(gain)));
                        double phase = chartGainPhase.ChartAreas[0].AxisY2.PixelPositionToValue(mousePoint.Y);
                        writer.Write("        Phase: " + phase.ToString(FRAResult.GetPhaseFormat(phase)));
                        writer.Write("      (CURSOR)");
                        for (int i = 0; i < m_Files.Count; i++) writer.WriteLine(); // same number of lines as normally shown
                        chartGainPhase.ChartAreas[0].AxisX.Title = writer.ToString();
                    }
                }
            }
            else
            {
                // Hide cursors
                chartGainPhase.ChartAreas[0].CursorX.SetCursorPosition(double.MaxValue);
                chartGainPhase.ChartAreas[0].CursorY.SetCursorPosition(double.MaxValue);
            }

            //show Y-cursor when mouse button is pressed
            if (chartGainPhase.Series.Count != 0 && e.Button == MouseButtons.Left) chartGainPhase.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, false);
            else { chartGainPhase.ChartAreas[0].CursorY.SetCursorPosition(double.MaxValue); } // hide cursor

        }
        private void ChartImpedance_MouseMove(object sender, MouseEventArgs e)
        {

            Point mousePoint = new Point(e.X, e.Y);
            if (chartImpedance.Series.Count != 0 && chartImpedance.Focused)
            {
                // always show frequency cursor
                chartImpedance.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, false);

                double frequency;
                if (m_logFrequencyAxis) frequency = Math.Pow(10, chartImpedance.ChartAreas[0].AxisX.PixelPositionToValue(mousePoint.X));    // freqeuncy is returned as log value because of log axis
                else frequency = chartImpedance.ChartAreas[0].AxisX.PixelPositionToValue(mousePoint.X);

                chartImpedance.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 10, FontStyle.Bold);
                if (e.Button != MouseButtons.Left) chartImpedance.ChartAreas[0].AxisX.Title = m_Files.GetImpedanceInfo(frequency, m_logFrequencyAxis); // show info based on frequency and series
                else
                {
                    //show info based on cursor
                    using (StringWriter writer = new StringWriter())
                    {
                        writer.Write("Frequency: " + frequency.ToString(FRAResult.GetFrequencyFormat(frequency, m_logFrequencyAxis)));
                        double impedance = chartImpedance.ChartAreas[0].AxisY.PixelPositionToValue(mousePoint.Y);
                        writer.Write("        Impedance: " + impedance.ToString(FRAResult.GetImpedanceFormat(impedance)));
                        double phase = chartImpedance.ChartAreas[0].AxisY2.PixelPositionToValue(mousePoint.Y);
                        writer.Write("        Phase: " + phase.ToString(FRAResult.GetPhaseFormat(phase)));
                        writer.Write("      (CURSOR)");
                        for (int i = 0; i < m_Files.Count; i++) writer.WriteLine(); // same number of lines as normally shown
                        chartImpedance.ChartAreas[0].AxisX.Title = writer.ToString();
                    }
                }
            }
            else
            {
                // Hide cursors
                chartImpedance.ChartAreas[0].CursorX.SetCursorPosition(double.MaxValue);
                chartImpedance.ChartAreas[0].CursorY.SetCursorPosition(double.MaxValue);
            }

            //show Y-cursor when mouse button is pressed
            if (chartImpedance.Series.Count != 0 && e.Button == MouseButtons.Left) chartImpedance.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, false);
            else { chartImpedance.ChartAreas[0].CursorY.SetCursorPosition(double.MaxValue); } // hide cursor

        }
        private void ChartCapacitance_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePoint = new Point(e.X, e.Y);
            if (chartCapacitance.Series.Count != 0 && chartCapacitance.Focused)
            {
                // always show frequency cursor
                chartCapacitance.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, false);

                double frequency;
                if (m_logFrequencyAxis) frequency = Math.Pow(10, chartCapacitance.ChartAreas[0].AxisX.PixelPositionToValue(mousePoint.X));    // freqeuncy is returned as log value because of log axis
                else frequency = chartCapacitance.ChartAreas[0].AxisX.PixelPositionToValue(mousePoint.X);

                chartCapacitance.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 10, FontStyle.Bold);
                if (e.Button != MouseButtons.Left) chartCapacitance.ChartAreas[0].AxisX.Title = m_Files.GetCapacitanceInfo(frequency, m_logFrequencyAxis); // show info based on frequency and series
                else
                {
                    //show info based on cursor
                    using (StringWriter writer = new StringWriter())
                    {
                        writer.Write("Frequency: " + frequency.ToString(FRAResult.GetFrequencyFormat(frequency, m_logFrequencyAxis)));
                        double capacitance = chartCapacitance.ChartAreas[0].AxisY.PixelPositionToValue(mousePoint.Y);
                        writer.Write("        Capacitance: " + capacitance.ToString(FRAResult.GetCapacitanceFormat(capacitance)));
                        double ESR = chartCapacitance.ChartAreas[0].AxisY2.PixelPositionToValue(mousePoint.Y);
                        writer.Write("        Phase: " + ESR.ToString(FRAResult.GetImpedanceFormat(ESR)));
                        writer.Write("      (CURSOR)");
                        for (int i = 0; i < m_Files.Count; i++) writer.WriteLine(); // same number of lines as normally shown
                        chartCapacitance.ChartAreas[0].AxisX.Title = writer.ToString();
                    }
                }
            }
            else
            {
                // Hide cursors
                chartCapacitance.ChartAreas[0].CursorX.SetCursorPosition(double.MaxValue);
                chartCapacitance.ChartAreas[0].CursorY.SetCursorPosition(double.MaxValue);
            }

            //show Y-cursor when mouse button is pressed
            if (chartCapacitance.Series.Count != 0 && e.Button == MouseButtons.Left) chartCapacitance.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, false);
            else { chartCapacitance.ChartAreas[0].CursorY.SetCursorPosition(double.MaxValue); } // hide cursor

        }
        private void ChartInductance_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePoint = new Point(e.X, e.Y);
            if (chartInductance.Series.Count != 0 && chartInductance.Focused)
            {
                // always show frequency cursor
                chartInductance.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, false);

                double frequency;
                if (m_logFrequencyAxis) frequency = Math.Pow(10, chartInductance.ChartAreas[0].AxisX.PixelPositionToValue(mousePoint.X));    // freqeuncy is returned as log value because of log axis
                else frequency = chartInductance.ChartAreas[0].AxisX.PixelPositionToValue(mousePoint.X);

                chartInductance.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 10, FontStyle.Bold);
                if (e.Button != MouseButtons.Left) chartInductance.ChartAreas[0].AxisX.Title = m_Files.GetInductanceInfo(frequency, m_logFrequencyAxis); // show info based on frequency and series
                else
                {
                    //show info based on cursor
                    using (StringWriter writer = new StringWriter())
                    {
                        writer.Write("Frequency: " + frequency.ToString(FRAResult.GetFrequencyFormat(frequency, m_logFrequencyAxis)));
                        double inductance = chartInductance.ChartAreas[0].AxisY.PixelPositionToValue(mousePoint.Y);
                        writer.Write("        Inductance: " + inductance.ToString(FRAResult.GetInductanceFormat(inductance)));
                        double ESR = chartInductance.ChartAreas[0].AxisY2.PixelPositionToValue(mousePoint.Y);
                        writer.Write("        Phase: " + ESR.ToString(FRAResult.GetImpedanceFormat(ESR)));
                        writer.Write("      (CURSOR)");
                        for (int i = 0; i < m_Files.Count; i++) writer.WriteLine(); // same number of lines as normally shown
                        chartInductance.ChartAreas[0].AxisX.Title = writer.ToString();
                    }
                }
            }
            else
            {
                // Hide cursors
                chartInductance.ChartAreas[0].CursorX.SetCursorPosition(double.MaxValue);
                chartInductance.ChartAreas[0].CursorY.SetCursorPosition(double.MaxValue);
            }

            //show Y-cursor when mouse button is pressed
            if (e.Button == MouseButtons.Left) chartInductance.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, false);
            else { chartInductance.ChartAreas[0].CursorY.SetCursorPosition(double.MaxValue); } // hide cursor
        }

        private void ChartGainPhase_MouseEnter(object sender, EventArgs e) // chart needs to be focussed for the Mousewheel events to fire
        {
            if (mainFormActive && !chartGainPhase.Focused)
                chartGainPhase.Focus();
        }

        private void ChartImpedance_MouseEnter(object sender, EventArgs e) // chart needs to be focussed for the Mousewheel events to fire
        {
            if (mainFormActive && !chartImpedance.Focused)
                chartImpedance.Focus();
        }

        private void ChartCapacitance_MouseEnter(object sender, EventArgs e) // chart needs to be focussed for the Mousewheel events to fire
        {
            if (mainFormActive && !chartCapacitance.Focused)
                chartCapacitance.Focus();
        }

        private void ChartInductance_MouseEnter(object sender, EventArgs e) // chart needs to be focussed for the Mousewheel events to fire
        {
            if (mainFormActive && !chartInductance.Focused)
                chartInductance.Focus();
        }

        private void FormMain_Activated(object sender, EventArgs e)
        {
            mainFormActive = true;
        }

        private void FormMain_Deactivate(object sender, EventArgs e)
        {
            mainFormActive = false;
        }

        private void saveFRAFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_LegendItem != null)
            {
                logService.Debug("Save file as: " + m_LegendItem.SeriesName);
                Series serie = GetDisplayedChart().Series[m_LegendItem.SeriesName];
                FRAFile file = (FRAFile)serie.Tag;

                logService.Debug("Save file as: " + m_LegendItem.SeriesName + "  (" + file.FRAFileType.ToString() + ")");

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.InitialDirectory = CurrentSettings.Instance.PathLastMeasurementFile;
                dialog.Filter = "CSV File (*.csv)|*.csv";
                dialog.Title = "Save file as";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    CurrentSettings.Instance.PathLastMeasurementFile = dialog.FileName;
                    logService.Info("Saving file: " + dialog.FileName);
                    file.SaveAs(FRAFileType.FRA4PicoScope, dialog.FileName);
                }
            }
        }

        private void ChartGainPhase_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyboardShortcuts(sender, e);
        }
        private void ChartImpedance_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyboardShortcuts(sender, e);
        }
        private void ChartCapacitance_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyboardShortcuts(sender, e);
        }
        private void ChartInductance_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyboardShortcuts(sender, e);
        }

        #endregion

        #region exit app

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_Files.ContrainsUnsavedFiles)
            {
                DialogResult dialogResult = MessageBox.Show("Unsaved Measurements will be lost, do you want to continue", "Unsaved Measurements!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveSettingsOnExit();
                    return;
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            SaveSettingsOnExit();
        }

        private void SaveSettingsOnExit()
        {
            // becasue this setting is continously read to update the info at the bottom of the chart,
            // a local variable is used to prevent unnecesary file acces. For other settings this is not necesarry
            CurrentSettings.Instance.LogaritmicFrequencyAxis = m_logFrequencyAxis;
            if (CurrentSettings.Instance.SaveSettingsOnExit) IniSettingsController.Instance.SaveFile("Settings");
        }

        #endregion

    }
}
