// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TweetinviGrabber.cs" company="Simon Walker">
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
//   The tweetinvi grabber.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ZerosTwitterClient.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;

    using Tweetinvi;
    using Tweetinvi.Core.Interfaces;

    using Tweet = ZerosTwitterClient.Tweet;

    /// <summary>
    /// The Tweetinvi grabber.
    /// </summary>
    public class TweetinviGrabber : ITwitterGrabber
    {
        #region Fields

        /// <summary>
        /// The latest id.
        /// </summary>
        private long latestId = 0;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get tweets.
        /// </summary>
        /// <param name="search">
        /// The search.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{Tweet}"/>.
        /// </returns>
        public IEnumerable<Tweet> GetTweets(string search)
        {
            ITweetSearchParameters searchParameters = Search.GenerateSearchTweetParameter(search);
            searchParameters.SinceId = this.latestId;

            List<ITweet> searchTweets = Search.SearchTweets(searchParameters);

            List<Tweet> returnedTweets = searchTweets.Select(x => new Tweet(x)).ToList();

            this.latestId = (long)returnedTweets.Max(x => x.Id);

            return returnedTweets;
        }

        #endregion
    }
}