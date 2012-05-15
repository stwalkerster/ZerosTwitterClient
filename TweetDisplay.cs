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
        private readonly Tweet _t;

        public TweetDisplay()
        {
            InitializeComponent();
            pictureBox1.Image = ImageCache.fetch("https://twimg0-a.akamaihd.net/profile_images/1168462701/Square_UNIONLOGO_normal.jpg");
        }

        public TweetDisplay(Tweet t)
        {
            _t = t;
            InitializeComponent();
            label1.Text = t.Content;
            label2.Text = t.Author;


            pictureBox1.Image = ImageCache.fetch(t.Image);
        }
    }
}
