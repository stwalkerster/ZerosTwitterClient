using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace ZerosTwitterClient
{
    public partial class ModerationForm : Form
    {
        public ModerationForm()
        {
            InitializeComponent();
            tweetGrabberThread = new BackgroundWorker();
            tweetGrabberThread.DoWork+=addTweets;
            tweetGrabberThread.RunWorkerCompleted += tweetGrabberThread_RunWorkerCompleted;
        }

        void tweetGrabberThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            getMoreTweetsToolStripMenuItem.Enabled = true;
        }

        private static List<ulong> _activeTweets = new List<ulong>();
        public DisplayForm Display;

        private void ModerationForm_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.Tag = this;
            Display = new DisplayForm();
            Display.Show();
        }

        private void displayFullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Display.isRunning)
            {
                Display.destroyFeed();
                displayFullscreenToolStripMenuItem.Checked = false;
            }
            else
            {
                Display.setupFeed();

                displayFullscreenToolStripMenuItem.Checked = true;
            }
        }

        private BackgroundWorker tweetGrabberThread;

        private void getMoreTweetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(tweetGrabberThread.IsBusy) return;

            tweetGrabberThread.RunWorkerAsync();
            getMoreTweetsToolStripMenuItem.Enabled = false;
            toolStripStatusLabel1.Text = "Getting tweets...";
        }

        private void addTweets(object sender, DoWorkEventArgs args)
        {
            LinkedList<Tweet> newTweets;
            try
            {
                newTweets = TwitterGrabber.getTweets(Properties.Settings.Default.TwitterSearchTerm);
                foreach (var t in newTweets)
                {
                    if(_activeTweets.Contains(t.Id))
                        continue;

                    _activeTweets.Add(t.Id);

                    var td = new ModTweet(t);
                    
                    addTweetToModPanelAsync(td);
                }

                toolStripStatusLabel1.Text = "Done. " + newTweets.Count + " tweets fetched.";
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = (ex.Message);
            }
        }

        private delegate void AddTweetToModPanelAsyncDelegate(ModTweet t);
        private void addTweetToModPanelAsync(ModTweet t)
        {
            if(InvokeRequired)
            {
                Invoke(new AddTweetToModPanelAsyncDelegate(addTweetToModPanelAsync), new object[] {t});
                return;
            }

            flowLayoutPanel1.Controls.Add(t);
        }

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                control.Width = flowLayoutPanel1.Width - 26;
            }
        }

        private void flowLayoutPanel2_SizeChanged(object sender, EventArgs e)
        {

            foreach (Control control in flowLayoutPanel2.Controls)
            {
                control.Width = flowLayoutPanel2.Width - 26;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = checkBox1.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = (int)(numericUpDown1.Value * 1000);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tweetGrabberThread.IsBusy) return;

            tweetGrabberThread.RunWorkerAsync();
            getMoreTweetsToolStripMenuItem.Enabled = false;
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
