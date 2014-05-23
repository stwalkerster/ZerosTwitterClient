// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModerationForm.cs" company="Simon Walker">
//   Copyright (C) 2014 Simon Walker
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
//   to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions of 
//   the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//   THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
//   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
//   SOFTWARE.
// </copyright>
// <summary>
//   The moderation form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ZerosTwitterClient.Forms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using LinqToTwitter;

    using ZerosTwitterClient.Properties;
    using ZerosTwitterClient.Services.Interfaces;

    using Settings = ZerosTwitterClient.Properties.Settings;
    using Tweet = ZerosTwitterClient.Tweet;

    /// <summary>
    /// The moderation form.
    /// </summary>
    public partial class ModerationForm : Form
    {
        #region Static Fields

        /// <summary>
        /// The _active tweets.
        /// </summary>
        private static readonly List<ulong> ActiveTweets = new List<ulong>();

        #endregion

        #region Fields

        /// <summary>
        /// The twitter grabber.
        /// </summary>
        private readonly ITwitterGrabber twitterGrabber;

        /// <summary>
        /// The tweet grabber thread.
        /// </summary>
        private readonly BackgroundWorker tweetGrabberThread;

        /// <summary>
        /// The image cache.
        /// </summary>
        private readonly IImageCache imageCache;

        /// <summary>
        /// The context.
        /// </summary>
        private readonly TwitterContext context;


        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ModerationForm"/> class.
        /// </summary>
        /// <param name="display">
        /// The display.
        /// </param>
        /// <param name="twitterGrabber">
        /// The twitter Grabber.
        /// </param>
        /// <param name="imageCache">
        /// The image Cache.
        /// </param>
        public ModerationForm(DisplayForm display, ITwitterGrabber twitterGrabber, IImageCache imageCache, TwitterContext context)
        {
            this.twitterGrabber = twitterGrabber;
            this.imageCache = imageCache;
            this.context = context;
            this.Display = display;

            this.InitializeComponent();
            this.tweetGrabberThread = new BackgroundWorker();
            this.tweetGrabberThread.DoWork += this.AddTweets;
            this.tweetGrabberThread.RunWorkerCompleted += this.TweetGrabberThreadRunWorkerCompleted;
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The add tweet to mod panel async delegate.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        private delegate void AddTweetToModPanelAsyncDelegate(ModTweet t);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the display.
        /// </summary>
        public DisplayForm Display { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// The add tweet to mod panel async.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        private void AddTweetToModPanelAsync(ModTweet t)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new AddTweetToModPanelAsyncDelegate(this.AddTweetToModPanelAsync), new object[] { t });
                return;
            }

            this.flowLayoutPanel1.Controls.Add(t);
        }

        /// <summary>
        /// The add tweets.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        private void AddTweets(object sender, DoWorkEventArgs args)
        {
            try
            {
                IEnumerable<Tweet> newTweets = this.twitterGrabber.GetTweets(Settings.Default.TwitterSearchTerm).ToList();
                foreach (var t in newTweets)
                {
                    this.ReceivedTweet(t);
                }

                this.toolStripStatusLabel.Text = Resources.Done + string.Format("{0} tweets fetched.", newTweets.Count());
            }
            catch (Exception ex)
            {
                this.toolStripStatusLabel.Text = ex.Message;
            }
        }

        /// <summary>
        /// The received tweet.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        private void ReceivedTweet(Tweet t)
        {
            if (ActiveTweets.Contains(t.Id))
            {
                return;
            }

            ActiveTweets.Add(t.Id);

            var td = new ModTweet(t);

            this.AddTweetToModPanelAsync(td);
        }

        /// <summary>
        /// The check box 1_ checked changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void CheckBox1CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = streamEnabledCheckbox.Checked;
        }

        /// <summary>
        /// The display full-screen tool strip menu item_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DisplayFullscreenToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (this.Display.IsRunning)
            {
                this.Display.DestroyFeed();
                this.displayFullscreenToolStripMenuItem.Checked = false;
            }
            else
            {
                this.Display.SetupFeed();

                this.displayFullscreenToolStripMenuItem.Checked = true;
            }
        }

        /// <summary>
        /// The flow layout panel 1_ size changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void FlowLayoutPanel1SizeChanged(object sender, EventArgs e)
        {
            foreach (Control control in this.flowLayoutPanel1.Controls)
            {
                control.Width = this.flowLayoutPanel1.Width - 26;
            }
        }

        /// <summary>
        /// The flow layout panel 2_ size changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void FlowLayoutPanel2SizeChanged(object sender, EventArgs e)
        {
            foreach (Control control in this.flowLayoutPanel2.Controls)
            {
                control.Width = this.flowLayoutPanel2.Width - 26;
            }
        }

        /// <summary>
        /// The get more tweets tool strip menu item_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void GetMoreTweetsToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (this.tweetGrabberThread.IsBusy)
            {
                return;
            }

            this.tweetGrabberThread.RunWorkerAsync();
            this.getMoreTweetsToolStripMenuItem.Enabled = false;
            this.toolStripStatusLabel.Text = Resources.GettingTweets;
        }

        /// <summary>
        /// The moderation form_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ModerationFormLoad(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Tag = this;
            this.Display.Show();
        }

        /// <summary>
        /// The timer 1_ tick.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Timer1Tick(object sender, EventArgs e)
        {
            if (this.tweetGrabberThread.IsBusy)
            {
                return;
            }

            this.tweetGrabberThread.RunWorkerAsync();
            this.getMoreTweetsToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// The tool strip status label 1_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ToolStripStatusLabel1Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The tweet grabber thread_ run worker completed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TweetGrabberThreadRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.getMoreTweetsToolStripMenuItem.Enabled = true;
        }

        #endregion
    }
}