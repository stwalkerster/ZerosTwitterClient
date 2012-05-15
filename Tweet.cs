using System;
using System.Collections.Generic;
using System.Text;

namespace ZerosTwitterClient
{
    public class Tweet
    {
        public string Content { get; set; }

        public string Author { get; set; }

        public ulong Id { get; set; }

        public string Image { get; set; }

        public string Timestamp { get; set; }
    }
}
