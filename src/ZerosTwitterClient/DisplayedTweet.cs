// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayedTweet.cs" company="Simon Walker">
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
//   The displayed tweet.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZerosTwitterClient
{
    using System;
    using System.Windows.Forms;

    using ZerosTwitterClient.Services;

    /// <summary>
    /// The displayed tweet.
    /// </summary>
    public partial class DisplayedTweet : UserControl
    {
        #region Fields

        /// <summary>
        /// The tweet.
        /// </summary>
        private readonly Tweet tweet;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DisplayedTweet"/> class.
        /// </summary>
        public DisplayedTweet()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DisplayedTweet"/> class.
        /// </summary>
        /// <param name="tweet">
        /// The tweet.
        /// </param>
        public DisplayedTweet(Tweet tweet)
        {
            this.tweet = tweet;
            this.InitializeComponent();
            this.label1.Text = tweet.Content;

            this.label2.Text = tweet.Author;
            this.button1.Enabled = this.button2.Enabled = true;

            this.pictureBox1.Image = ImageCache.StaticFetch(tweet.ImageUrl);
            this.label3.Text = tweet.Timestamp;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The displayed tweet_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DisplayedTweetLoad(object sender, EventArgs e)
        {
            this.Width = this.Parent.Width - 26;
        }

        /// <summary>
        /// The button 1_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Button1Click(object sender, EventArgs e)
        {
            Program.ModerationForm.Display.AddTweet(new TweetDisplay(this.tweet));
            Program.ModerationForm.flowLayoutPanel2.Controls.SetChildIndex(this, 0);
        }

        /// <summary>
        /// The button 2_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Button2Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);

            bool changed;
            do
            {
                changed = false;
                foreach (var control in Program.ModerationForm.Display.FlowLayoutPanel.Controls)
                {
                    var x = (TweetDisplay)control;

                    if (x.T == null)
                    {
                        continue;
                    }

                    if (x.T.Id != this.tweet.Id)
                    {
                        continue;
                    }

                    Program.ModerationForm.Display.FlowLayoutPanel.Controls.Remove(x);
                    changed = true;
                }
            }
            while (changed);
        }

        #endregion
    }
}