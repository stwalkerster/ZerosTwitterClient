// --------------------------------------------------------------------------------------------------------------------
// <copyright file="L2TGrabber.cs" company="Simon Walker">
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
//   The twitter grabber.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ZerosTwitterClient.Services
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using LinqToTwitter;

    using ZerosTwitterClient.Services.Interfaces;

    /// <summary>
    /// The twitter grabber.
    /// </summary>
    internal class L2TGrabber : ITwitterGrabber
    {
        #region Fields

        /// <summary>
        /// The context.
        /// </summary>
        private readonly TwitterContext context;

        /// <summary>
        /// The image cache.
        /// </summary>
        private readonly IImageCache imageCache;

        /// <summary>
        /// The twitter lock.
        /// </summary>
        private readonly object twitterLock = new object();

        /// <summary>
        /// The _id.
        /// </summary>
        private ulong id;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="L2TGrabber"/> class. 
        /// </summary>
        /// <param name="imageCache">
        /// The image cache.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public L2TGrabber(IImageCache imageCache, TwitterContext context)
        {
            this.imageCache = imageCache;
            this.context = context;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get tweets.
        /// </summary>
        /// <param name="searchQuery">
        /// The search.
        /// </param>
        /// <returns>
        /// The <see cref="LinkedList{Tweet}"/>.
        /// </returns>
        public IEnumerable<Tweet> GetTweets(string searchQuery)
        {
            lock (this.twitterLock)
            {
                var searchResponse =
                    (from search in this.context.Search
                     where search.Type == SearchType.Search && search.Query == searchQuery && search.SinceID == this.id
                     select search).SingleOrDefault();

                if (searchResponse != null && searchResponse.Statuses != null)
                {
                    var tweets = searchResponse.Statuses.Select(
                        x =>
                        new Tweet(this.imageCache)
                            {
                                Author = x.User.ScreenNameResponse, 
                                Content = x.Text, 
                                Id = x.StatusID, 
                                ImageUrl = x.User.ProfileImageUrl, 
                                Timestamp = x.CreatedAt.ToString(CultureInfo.InvariantCulture)
                            }).ToList();

                    if (tweets.Any())
                    {
                        this.id = tweets.Max(x => x.Id);
                    }

                    return tweets;
                }
            }

            return new List<Tweet>();
        }

        #endregion
    }
}