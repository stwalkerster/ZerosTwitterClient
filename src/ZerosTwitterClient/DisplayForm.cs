﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayForm.cs" company="Simon Walker">
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
//   The display form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZerosTwitterClient
{
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// The display form.
    /// </summary>
    public partial class DisplayForm : Form
    {
        #region Fields

        /// <summary>
        /// The flp.
        /// </summary>
        public FlowLayoutPanel flp;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DisplayForm"/> class.
        /// </summary>
        public DisplayForm()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add tweet.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        public void AddTweet(TweetDisplay t)
        {
            if (this.flp != null)
            {
                this.flp.Controls.Add(t);

                // move it to the top
                this.flp.Controls.SetChildIndex(t, 0);
            }
        }

        /// <summary>
        /// The destroy feed.
        /// </summary>
        public void DestroyFeed()
        {
            this.IsRunning = false;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Normal;
            this.Controls.Clear();
        }

        /// <summary>
        /// The setup feed.
        /// </summary>
        public void SetupFeed()
        {
            this.IsRunning = true;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;

            this.Controls.Clear();
            this.SuspendLayout();

            this.flp = new FlowLayoutPanel
                           {
                               Dock = DockStyle.Fill, 
                               BorderStyle = BorderStyle.FixedSingle, 
                               BackColor = Color.Black, 
                               ForeColor = Color.White, 
                               Padding = new Padding(10)
                           };

            // flp.Controls.Add(new TweetDisplay());
            this.Controls.Add(this.flp);
            this.ResumeLayout(true);
        }

        #endregion
    }
}