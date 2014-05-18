using System;
using System.Windows.Forms;

namespace ZerosTwitterClient
{
    public partial class ModTweet : UserControl
    {
        private readonly Tweet _t;

        public ModTweet()
        {
            InitializeComponent();
        }


        public ModTweet(Tweet t)
        {
            _t = t;
            InitializeComponent();
            label1.Text = t.Content;
            
            label2.Text = t.Author;
            button1.Enabled = button2.Enabled = true;

            pictureBox1.Image = ImageCache.fetch(t.Image);
            label3.Text = t.Timestamp;
        }


        private void ModTweet_Load(object sender, EventArgs e)
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
            Parent.Controls.Remove(this);
            Program.ModerationForm.Display.addTweet(new TweetDisplay(_t));
            var displayedTweet = new DisplayedTweet(_t);
            Program.ModerationForm.flowLayoutPanel2.Controls.Add(displayedTweet);
            Program.ModerationForm.flowLayoutPanel2.Controls.SetChildIndex(displayedTweet, 0);
                
        }
    }
}
