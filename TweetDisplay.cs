using System.Windows.Forms;

namespace ZerosTwitterClient
{
    public partial class TweetDisplay : UserControl
    {
        public readonly Tweet _t;

        public TweetDisplay()
        {
            InitializeComponent();
            pictureBox1.Image = ImageCache.fetch("http://twimg0-a.akamaihd.net/sticky/default_profile_images/default_profile_1_normal.png");
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
