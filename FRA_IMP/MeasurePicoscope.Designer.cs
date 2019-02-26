namespace FRA_IMP
{
    partial class MeasurePicoscope
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeasurePicoscope));
            this.buttonStartMeasurement = new System.Windows.Forms.Button();
            this.buttonAbortMeasurement = new System.Windows.Forms.Button();
            this.richTextBoxMessageLog = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxInputOffset = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxInputChannel = new System.Windows.Forms.ComboBox();
            this.comboBoxInputAttenuation = new System.Windows.Forms.ComboBox();
            this.comboBoxInputCoupling = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxOutputOffset = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxOutputChannel = new System.Windows.Forms.ComboBox();
            this.comboBoxOutputAttenuation = new System.Windows.Forms.ComboBox();
            this.comboBoxOutputCoupling = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxStimulusOffeset = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxStimulusAmplitude = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxStepsPerDecade = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxStopFrequency = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxStartFrequency = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBoxStatusEanbled = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStartMeasurement
            // 
            this.buttonStartMeasurement.Location = new System.Drawing.Point(18, 317);
            this.buttonStartMeasurement.Name = "buttonStartMeasurement";
            this.buttonStartMeasurement.Size = new System.Drawing.Size(124, 23);
            this.buttonStartMeasurement.TabIndex = 0;
            this.buttonStartMeasurement.Text = "Start Measurement";
            this.buttonStartMeasurement.UseVisualStyleBackColor = true;
            this.buttonStartMeasurement.Click += new System.EventHandler(this.buttonStartMeasurement_Click);
            // 
            // buttonAbortMeasurement
            // 
            this.buttonAbortMeasurement.Location = new System.Drawing.Point(153, 317);
            this.buttonAbortMeasurement.Name = "buttonAbortMeasurement";
            this.buttonAbortMeasurement.Size = new System.Drawing.Size(124, 23);
            this.buttonAbortMeasurement.TabIndex = 1;
            this.buttonAbortMeasurement.Text = "Abort Measurement";
            this.buttonAbortMeasurement.UseVisualStyleBackColor = true;
            this.buttonAbortMeasurement.Click += new System.EventHandler(this.buttonAbortMeasurement_Click);
            // 
            // richTextBoxMessageLog
            // 
            this.richTextBoxMessageLog.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.richTextBoxMessageLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMessageLog.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxMessageLog.Name = "richTextBoxMessageLog";
            this.richTextBoxMessageLog.ReadOnly = true;
            this.richTextBoxMessageLog.Size = new System.Drawing.Size(532, 280);
            this.richTextBoxMessageLog.TabIndex = 2;
            this.richTextBoxMessageLog.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(546, 65);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input DUT";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxInputChannel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxInputAttenuation, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxInputCoupling, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(540, 46);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxInputOffset, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(408, 23);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(129, 20);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // textBoxInputOffset
            // 
            this.textBoxInputOffset.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxInputOffset.Location = new System.Drawing.Point(3, 3);
            this.textBoxInputOffset.Name = "textBoxInputOffset";
            this.textBoxInputOffset.Size = new System.Drawing.Size(93, 20);
            this.textBoxInputOffset.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(102, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "V";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(408, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Offset";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(273, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Coupling";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(138, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Attenuation";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Scope Channel";
            // 
            // comboBoxInputChannel
            // 
            this.comboBoxInputChannel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBoxInputChannel.FormattingEnabled = true;
            this.comboBoxInputChannel.Location = new System.Drawing.Point(3, 23);
            this.comboBoxInputChannel.Name = "comboBoxInputChannel";
            this.comboBoxInputChannel.Size = new System.Drawing.Size(121, 21);
            this.comboBoxInputChannel.TabIndex = 1;
            // 
            // comboBoxInputAttenuation
            // 
            this.comboBoxInputAttenuation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBoxInputAttenuation.FormattingEnabled = true;
            this.comboBoxInputAttenuation.Location = new System.Drawing.Point(138, 23);
            this.comboBoxInputAttenuation.Name = "comboBoxInputAttenuation";
            this.comboBoxInputAttenuation.Size = new System.Drawing.Size(116, 21);
            this.comboBoxInputAttenuation.TabIndex = 3;
            // 
            // comboBoxInputCoupling
            // 
            this.comboBoxInputCoupling.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBoxInputCoupling.FormattingEnabled = true;
            this.comboBoxInputCoupling.Location = new System.Drawing.Point(273, 23);
            this.comboBoxInputCoupling.Name = "comboBoxInputCoupling";
            this.comboBoxInputCoupling.Size = new System.Drawing.Size(116, 21);
            this.comboBoxInputCoupling.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Location = new System.Drawing.Point(12, 83);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(546, 65);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output DUT";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.label7, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.label8, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.label9, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.comboBoxOutputChannel, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.comboBoxOutputAttenuation, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.comboBoxOutputCoupling, 2, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(540, 46);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel4.Controls.Add(this.textBoxOutputOffset, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label6, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(408, 23);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(129, 20);
            this.tableLayoutPanel4.TabIndex = 4;
            // 
            // textBoxOutputOffset
            // 
            this.textBoxOutputOffset.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxOutputOffset.Location = new System.Drawing.Point(3, 3);
            this.textBoxOutputOffset.Name = "textBoxOutputOffset";
            this.textBoxOutputOffset.Size = new System.Drawing.Size(93, 20);
            this.textBoxOutputOffset.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(102, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "V";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(408, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Offset";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(273, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Coupling";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(138, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Attenuation";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Scope Channel";
            // 
            // comboBoxOutputChannel
            // 
            this.comboBoxOutputChannel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBoxOutputChannel.FormattingEnabled = true;
            this.comboBoxOutputChannel.Location = new System.Drawing.Point(3, 23);
            this.comboBoxOutputChannel.Name = "comboBoxOutputChannel";
            this.comboBoxOutputChannel.Size = new System.Drawing.Size(121, 21);
            this.comboBoxOutputChannel.TabIndex = 1;
            // 
            // comboBoxOutputAttenuation
            // 
            this.comboBoxOutputAttenuation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBoxOutputAttenuation.FormattingEnabled = true;
            this.comboBoxOutputAttenuation.Location = new System.Drawing.Point(138, 23);
            this.comboBoxOutputAttenuation.Name = "comboBoxOutputAttenuation";
            this.comboBoxOutputAttenuation.Size = new System.Drawing.Size(116, 21);
            this.comboBoxOutputAttenuation.TabIndex = 3;
            // 
            // comboBoxOutputCoupling
            // 
            this.comboBoxOutputCoupling.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBoxOutputCoupling.FormattingEnabled = true;
            this.comboBoxOutputCoupling.Location = new System.Drawing.Point(273, 23);
            this.comboBoxOutputCoupling.Name = "comboBoxOutputCoupling";
            this.comboBoxOutputCoupling.Size = new System.Drawing.Size(116, 21);
            this.comboBoxOutputCoupling.TabIndex = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel5);
            this.groupBox3.Location = new System.Drawing.Point(12, 175);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(546, 65);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Stimulus Signal";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel7, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.label14, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.label15, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(540, 46);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel7.Controls.Add(this.textBoxStimulusOffeset, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.label16, 1, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(138, 23);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(129, 20);
            this.tableLayoutPanel7.TabIndex = 8;
            // 
            // textBoxStimulusOffeset
            // 
            this.textBoxStimulusOffeset.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxStimulusOffeset.Location = new System.Drawing.Point(3, 3);
            this.textBoxStimulusOffeset.Name = "textBoxStimulusOffeset";
            this.textBoxStimulusOffeset.Size = new System.Drawing.Size(93, 20);
            this.textBoxStimulusOffeset.TabIndex = 0;
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(102, 3);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(14, 13);
            this.label16.TabIndex = 1;
            this.label16.Text = "V";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.Controls.Add(this.textBoxStimulusAmplitude, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.label11, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(129, 20);
            this.tableLayoutPanel6.TabIndex = 7;
            // 
            // textBoxStimulusAmplitude
            // 
            this.textBoxStimulusAmplitude.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxStimulusAmplitude.Location = new System.Drawing.Point(3, 3);
            this.textBoxStimulusAmplitude.Name = "textBoxStimulusAmplitude";
            this.textBoxStimulusAmplitude.Size = new System.Drawing.Size(93, 20);
            this.textBoxStimulusAmplitude.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(102, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(14, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "V";
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(138, 3);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(35, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Offset";
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 3);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Amplitude";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tableLayoutPanel8);
            this.groupBox4.Location = new System.Drawing.Point(12, 246);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(546, 65);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Frequency Sweep";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 4;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.Controls.Add(this.textBoxStepsPerDecade, 2, 1);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel9, 1, 1);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel10, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.label17, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.label18, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.label19, 2, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(540, 46);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // textBoxStepsPerDecade
            // 
            this.textBoxStepsPerDecade.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxStepsPerDecade.Location = new System.Drawing.Point(273, 23);
            this.textBoxStepsPerDecade.Name = "textBoxStepsPerDecade";
            this.textBoxStepsPerDecade.Size = new System.Drawing.Size(93, 20);
            this.textBoxStepsPerDecade.TabIndex = 7;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel9.Controls.Add(this.textBoxStopFrequency, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.label12, 1, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(138, 23);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(129, 20);
            this.tableLayoutPanel9.TabIndex = 8;
            // 
            // textBoxStopFrequency
            // 
            this.textBoxStopFrequency.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxStopFrequency.Location = new System.Drawing.Point(3, 3);
            this.textBoxStopFrequency.Name = "textBoxStopFrequency";
            this.textBoxStopFrequency.Size = new System.Drawing.Size(93, 20);
            this.textBoxStopFrequency.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(102, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(20, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Hz";
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel10.Controls.Add(this.textBoxStartFrequency, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.label13, 1, 0);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(129, 20);
            this.tableLayoutPanel10.TabIndex = 7;
            // 
            // textBoxStartFrequency
            // 
            this.textBoxStartFrequency.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxStartFrequency.Location = new System.Drawing.Point(3, 3);
            this.textBoxStartFrequency.Name = "textBoxStartFrequency";
            this.textBoxStartFrequency.Size = new System.Drawing.Size(93, 20);
            this.textBoxStartFrequency.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(102, 3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(20, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "Hz";
            // 
            // label17
            // 
            this.label17.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(138, 3);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(82, 13);
            this.label17.TabIndex = 2;
            this.label17.Text = "Stop Frequency";
            // 
            // label18
            // 
            this.label18.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(3, 3);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(82, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "Start Frequency";
            // 
            // label19
            // 
            this.label19.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(273, 3);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(77, 13);
            this.label19.TabIndex = 9;
            this.label19.Text = "Steps/Decade";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.richTextBoxMessageLog);
            this.groupBox5.Location = new System.Drawing.Point(564, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(538, 299);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Status";
            // 
            // checkBoxStatusEanbled
            // 
            this.checkBoxStatusEanbled.AutoSize = true;
            this.checkBoxStatusEanbled.Location = new System.Drawing.Point(567, 321);
            this.checkBoxStatusEanbled.Name = "checkBoxStatusEanbled";
            this.checkBoxStatusEanbled.Size = new System.Drawing.Size(107, 17);
            this.checkBoxStatusEanbled.TabIndex = 8;
            this.checkBoxStatusEanbled.Text = "EnableStatusLog";
            this.checkBoxStatusEanbled.UseVisualStyleBackColor = true;
            // 
            // MeasurePicoscope
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 346);
            this.Controls.Add(this.checkBoxStatusEanbled);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonAbortMeasurement);
            this.Controls.Add(this.buttonStartMeasurement);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MeasurePicoscope";
            this.Text = "Picoscope Control";
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStartMeasurement;
        private System.Windows.Forms.Button buttonAbortMeasurement;
        private System.Windows.Forms.RichTextBox richTextBoxMessageLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textBoxInputOffset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxInputChannel;
        private System.Windows.Forms.ComboBox comboBoxInputAttenuation;
        private System.Windows.Forms.ComboBox comboBoxInputCoupling;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TextBox textBoxOutputOffset;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBoxOutputChannel;
        private System.Windows.Forms.ComboBox comboBoxOutputAttenuation;
        private System.Windows.Forms.ComboBox comboBoxOutputCoupling;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.TextBox textBoxStimulusOffeset;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TextBox textBoxStimulusAmplitude;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TextBox textBoxStepsPerDecade;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TextBox textBoxStopFrequency;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.TextBox textBoxStartFrequency;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBoxStatusEanbled;
    }
}