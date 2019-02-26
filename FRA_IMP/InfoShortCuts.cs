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
    public partial class InfoShortCuts : Form
    {
        public InfoShortCuts()
        {
            InitializeComponent();
            this.TopMost = true;

            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            richTextBox1.AppendText("Mouse scroll:");
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
            richTextBox1.AppendText("Zoom frequency Axis (zoom centered around mouse pointer)"+  Environment.NewLine);

            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            richTextBox1.AppendText("Mouse scroll + Ctrl:");
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
            richTextBox1.AppendText(" Zoom primary Y-Axis (zoom centered around mouse pointer)" + Environment.NewLine);

            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            richTextBox1.AppendText("Mouse scroll + Shift:");
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
            richTextBox1.AppendText("Zoom secondary Y-Axis (zoom centered around mouse pointer)" + Environment.NewLine);

            richTextBox1.AppendText("" + Environment.NewLine);

            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            richTextBox1.AppendText("Left mouse click in chart:");
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
            richTextBox1.AppendText("Show both cursors, info at bottom based on cursor position" + Environment.NewLine);

            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            richTextBox1.AppendText("Right mouse click in chart:");
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
            richTextBox1.AppendText("Open menu for copy/save and chart properties" + Environment.NewLine);

            richTextBox1.AppendText("" + Environment.NewLine);

            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            richTextBox1.AppendText("Left click on chart legend items:");
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
            richTextBox1.AppendText("Change colour, line width, rename or delete selected item" + Environment.NewLine);  
        }
    }
}
