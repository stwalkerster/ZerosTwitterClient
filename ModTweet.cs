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
    public partial class ModTweet : UserControl
    {
        public ModTweet()
        {
            InitializeComponent();
        }


        public ModTweet(Tweet t)
        {
            InitializeComponent();
            label1.Text = t.Content;
            
            label2.Text = t.Author;
            button1.Enabled = button2.Enabled = true;

            pictureBox1.Image = Image.FromStream( WebRequest.Create(t.Image).GetResponse().GetResponseStream());

        }


        private void ModTweet_Load(object sender, EventArgs e)
        {

            Width = Parent.Width - 6;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
