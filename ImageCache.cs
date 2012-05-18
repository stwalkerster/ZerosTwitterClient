using System.Collections.Generic;
using System.Drawing;
using System.Net;

namespace ZerosTwitterClient
{
    class ImageCache
    {
        private static Dictionary<string, Image> cache = new Dictionary<string, Image>();

        private static object lockObject = new object();

        public static bool hasImage(string url)
        {
            lock (lockObject)
            {
                return cache.ContainsKey(url);
            }
        }

        public static Image fetch(string url)
        {
            if (!hasImage(url))
            {
                Image image = retrieveImage(url);
                lock (lockObject)
                {
                    cache.Add(url, image);
                }
            }
            lock (lockObject)
            {
                return cache[url];
            }
        }

        private static Image retrieveImage(string url)
        {
            return Image.FromStream(WebRequest.Create(url).GetResponse().GetResponseStream());
        }
    }
}
