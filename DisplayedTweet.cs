using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ZerosTwitterClient
{
    public partial class DisplayedTweet : UserControl
    {
        private readonly Tweet _t;

        public DisplayedTweet()
        {
            InitializeComponent();
        }


        public DisplayedTweet(Tweet t)
        {
            _t = t;
            InitializeComponent();
            label1.Text = t.Content;
            
            label2.Text = t.Author;
            button1.Enabled = button2.Enabled = true;

            pictureBox1.Image = ImageCache.fetch(t.Image);
            label3.Text = t.Timestamp;
        }


        private void DisplayedTweet_Load(object sender, EventArgs e)
        {

            Width = Parent.Width - 26;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.ModerationForm.Display.addTweet(new TweetDisplay(_t));
            Program.ModerationForm.flowLayoutPanel2.Controls.SetChildIndex(this,0);
        }
    }
}
