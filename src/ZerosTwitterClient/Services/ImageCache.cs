// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageCache.cs" company="Simon Walker">
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
//   The image cache.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ZerosTwitterClient.Services
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Net;

    using ZerosTwitterClient.Services.Interfaces;

    /// <summary>
    /// The image cache.
    /// </summary>
    internal class ImageCache : IImageCache
    {
        #region Static Fields

        /// <summary>
        /// The static cache.
        /// </summary>
        [Obsolete]
        private static readonly ImageCache StaticCache = new ImageCache();

        #endregion

        #region Fields

        /// <summary>
        /// The cache.
        /// </summary>
        private readonly IDictionary<string, Image> cache;

        /// <summary>
        /// The lock object.
        /// </summary>
        private readonly object lockObject = new object();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ImageCache"/> class.
        /// </summary>
        public ImageCache()
        {
            this.cache = new Dictionary<string, Image>();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The Fetch.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="Image"/>.
        /// </returns>
        [Obsolete]
        public static Image StaticFetch(string url)
        {
            return StaticCache.Fetch(url);
        }

        /// <summary>
        /// The fetch.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="Image"/>.
        /// </returns>
        public Image Fetch(string url)
        {
            if (!this.HasImage(url))
            {
                Image image = this.RetrieveImage(url);
                lock (this.lockObject)
                {
                    this.cache.Add(url, image);
                }

                return image;
            }

            lock (this.lockObject)
            {
                return this.cache[url];
            }
        }

        /// <summary>
        /// The has image.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool HasImage(string url)
        {
            lock (this.lockObject)
            {
                return this.cache.ContainsKey(url);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The retrieve image.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="Image"/>.
        /// </returns>
        private Image RetrieveImage(string url)
        {
            var webResponse = WebRequest.Create(url).GetResponse();

            if (webResponse == null)
            {
                throw new ApplicationException("Null webresponse fetching image");
            }

            var responseStream = webResponse.GetResponseStream();

            if (responseStream == null)
            {
                throw new ApplicationException("Null response stream fetching image");
            }

            return Image.FromStream(responseStream);
        }

        #endregion
    }
}