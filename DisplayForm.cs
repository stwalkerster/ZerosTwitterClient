using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZerosTwitterClient
{
    public partial class DisplayForm : Form
    {
        public DisplayForm()
        {
            InitializeComponent();

        }

        public bool isRunning { get; private set; }

        private FlowLayoutPanel flp;

        public void setupFeed()
        {
            isRunning = true;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;

            Controls.Clear();
            SuspendLayout();

            flp = new FlowLayoutPanel {Dock = DockStyle.Fill};

            flp.Controls.Add(new TweetDisplay());

            Controls.Add(flp);
            ResumeLayout(true);
        }

        public void destroyFeed()
        {
            isRunning = false;
            FormBorderStyle = FormBorderStyle.Sizable;
            WindowState = FormWindowState.Normal;
            Controls.Clear();
        }

        public void addTweet(TweetDisplay t)
        {
            flp.Controls.Add(t);

            t.BackColor = Color.Red;

            // move it to the top
            flp.Controls.SetChildIndex(t,0);

        }
    }
}
