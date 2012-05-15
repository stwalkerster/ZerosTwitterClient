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
            LinkedList<Tweet> newTweets;
            try
            {
                newTweets = TwitterGrabber.getTweets();
                foreach (var t in newTweets)
                {
                    var td = new ModTweet(t);
                    flowLayoutPanel1.Controls.Add(td);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                control.Width = flowLayoutPanel1.Width - 6;
            }
        }
    }
}
