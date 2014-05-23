// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tweet.cs" company="Simon Walker">
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
//   The tweet.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ZerosTwitterClient
{
    using System.Drawing;

    using ZerosTwitterClient.Services.Interfaces;

    /// <summary>
    /// The tweet.
    /// </summary>
    public class Tweet
    {
        #region Fields

        /// <summary>
        /// The image cache.
        /// </summary>
        private readonly IImageCache imageCache;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Tweet"/> class.
        /// </summary>
        /// <param name="imageCache">
        /// The image cache.
        /// </param>
        public Tweet(IImageCache imageCache)
        {
            this.imageCache = imageCache;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public ulong Id { get; set; }

        /// <summary>
        /// Gets the image.
        /// </summary>
        public Image Image
        {
            get
            {
                return this.imageCache.Fetch(this.ImageUrl);
            }
        }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public string Timestamp { get; set; }

        #endregion

        
    }
}