using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZerosTwitterClient
{
    public partial class ModerationForm : Form
    {
        public ModerationForm()
        {
            InitializeComponent();
        }

        private DisplayForm display;

        private void ModerationForm_Load(object sender, EventArgs e)
        {
            display = new DisplayForm();
            display.Show();
        }

        private void displayFullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (display.FormBorderStyle == System.Windows.Forms.FormBorderStyle.None) 
            {
                display.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            }
            else
            {
                display.WindowState = FormWindowState.Maximized;
                display.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            }
        }
    }
}
