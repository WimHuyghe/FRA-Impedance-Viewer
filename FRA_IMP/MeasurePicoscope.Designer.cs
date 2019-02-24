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
            this.SuspendLayout();
            // 
            // buttonStartMeasurement
            // 
            this.buttonStartMeasurement.Location = new System.Drawing.Point(12, 12);
            this.buttonStartMeasurement.Name = "buttonStartMeasurement";
            this.buttonStartMeasurement.Size = new System.Drawing.Size(124, 23);
            this.buttonStartMeasurement.TabIndex = 0;
            this.buttonStartMeasurement.Text = "Start Measurement";
            this.buttonStartMeasurement.UseVisualStyleBackColor = true;
            this.buttonStartMeasurement.Click += new System.EventHandler(this.buttonStartMeasurement_Click);
            // 
            // buttonAbortMeasurement
            // 
            this.buttonAbortMeasurement.Location = new System.Drawing.Point(12, 41);
            this.buttonAbortMeasurement.Name = "buttonAbortMeasurement";
            this.buttonAbortMeasurement.Size = new System.Drawing.Size(124, 23);
            this.buttonAbortMeasurement.TabIndex = 1;
            this.buttonAbortMeasurement.Text = "Abort Measurement";
            this.buttonAbortMeasurement.UseVisualStyleBackColor = true;
            this.buttonAbortMeasurement.Click += new System.EventHandler(this.buttonAbortMeasurement_Click);
            // 
            // richTextBoxMessageLog
            // 
            this.richTextBoxMessageLog.Location = new System.Drawing.Point(12, 70);
            this.richTextBoxMessageLog.Name = "richTextBoxMessageLog";
            this.richTextBoxMessageLog.ReadOnly = true;
            this.richTextBoxMessageLog.Size = new System.Drawing.Size(362, 368);
            this.richTextBoxMessageLog.TabIndex = 2;
            this.richTextBoxMessageLog.Text = "";
            // 
            // MeasurePicoscope
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.richTextBoxMessageLog);
            this.Controls.Add(this.buttonAbortMeasurement);
            this.Controls.Add(this.buttonStartMeasurement);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MeasurePicoscope";
            this.Text = "Picoscope Control";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStartMeasurement;
        private System.Windows.Forms.Button buttonAbortMeasurement;
        private System.Windows.Forms.RichTextBox richTextBoxMessageLog;
    }
}