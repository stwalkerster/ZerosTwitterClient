// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModTweet.cs" company="Simon Walker">
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
//   The mod tweet.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZerosTwitterClient
{
    using System;
    using System.Windows.Forms;

    using ZerosTwitterClient.Services;

    /// <summary>
    /// The mod tweet.
    /// </summary>
    public partial class ModTweet : UserControl
    {
        #region Fields

        /// <summary>
        /// The _t.
        /// </summary>
        private readonly Tweet t;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ModTweet"/> class.
        /// </summary>
        public ModTweet()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ModTweet"/> class.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        public ModTweet(Tweet t)
        {
            this.t = t;
            this.InitializeComponent();
            this.label1.Text = t.Content;

            this.label2.Text = t.Author;
            this.button1.Enabled = this.button2.Enabled = true;

            this.pictureBox1.Image = ImageCache.StaticFetch(t.ImageUrl);
            this.label3.Text = t.Timestamp;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The mod tweet_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ModTweetLoad(object sender, EventArgs e)
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
            this.Parent.Controls.Remove(this);
            Program.ModerationForm.Display.AddTweet(new TweetDisplay(this.t));
            var displayedTweet = new DisplayedTweet(this.t);
            Program.ModerationForm.flowLayoutPanel2.Controls.Add(displayedTweet);
            Program.ModerationForm.flowLayoutPanel2.Controls.SetChildIndex(displayedTweet, 0);
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
        }

        /// <summary>
        /// The label 1_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Label1Click(object sender, EventArgs e)
        {
        }

        #endregion
    }
}