using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private DisplayForm _display;

        private void ModerationForm_Load(object sender, EventArgs e)
        {
            _display = new DisplayForm();
            _display.Show();
        }

        private void displayFullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_display.isRunning)
            {
                _display.destroyFeed();
                displayFullscreenToolStripMenuItem.Checked = false;
            }
            else
            {
                _display.setupFeed();

                displayFullscreenToolStripMenuItem.Checked = true;
            }
        }

        private void getMoreTweetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var t in TwitterGrabber.getTweets())
            {
                TweetDisplay td = new TweetDisplay(t);
            }
        }
    }
}
