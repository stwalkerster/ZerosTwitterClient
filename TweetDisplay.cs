﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ZerosTwitterClient
{
    public partial class TweetDisplay : UserControl
    {
        public TweetDisplay()
        {
            InitializeComponent();
        }

        public TweetDisplay(Tweet t)
        {
            InitializeComponent();
            label1.Text = t.Content;
            label2.Text = t.Author;


            pictureBox1.Image = Image.FromStream( WebRequest.Create(t.Image).GetResponse().GetResponseStream());

        }
    }
}
