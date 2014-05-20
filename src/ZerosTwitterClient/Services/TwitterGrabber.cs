// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwitterGrabber.cs" company="Simon Walker">
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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Xml;
    using System.Xml.XPath;

    using ZerosTwitterClient.Services.Interfaces;

    /// <summary>
    /// The twitter grabber.
    /// </summary>
    internal class TwitterGrabber : ITwitterGrabber
    {
        #region Fields

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
        /// Initialises a new instance of the <see cref="TwitterGrabber"/> class.
        /// </summary>
        /// <param name="imageCache">
        /// The image cache.
        /// </param>
        public TwitterGrabber(IImageCache imageCache)
        {
            this.imageCache = imageCache;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get tweets.
        /// </summary>
        /// <param name="search">
        /// The search.
        /// </param>
        /// <returns>
        /// The <see cref="LinkedList{Tweet}"/>.
        /// </returns>
        public LinkedList<Tweet> GetTweets(string search)
        {
            var tweets = new LinkedList<Tweet>();

            lock (this.twitterLock)
            {
                var webRequest =
                    (HttpWebRequest)
                    WebRequest.Create(
                        "http://search.twitter.com/search.atom?since_id=" + this.id + "&result_type=recent" + "&q="
                        + HttpUtility.UrlEncode(search));

                webRequest.UserAgent = "HwunionTwitterClient/1.0 (+ simon@stwalkerster.net )";
                webRequest.Timeout = 5000;
                var response = (HttpWebResponse)webRequest.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException(response.StatusDescription);
                }

                Stream responseStream;

                if ((responseStream = response.GetResponseStream()) == null)
                {
                    throw new ArgumentException();
                }

                string originaldata = new StreamReader(responseStream).ReadToEnd();
                string newdata = originaldata.Replace("<georss:", "<").Replace("</georss:", "</");

                Stream datas = new MemoryStream(Encoding.ASCII.GetBytes(newdata));

                var xpd = new XPathDocument(datas);

                var xpn = xpd.CreateNavigator();
                var xnm = new XmlNamespaceManager(xpn.NameTable);
                xnm.AddNamespace("atom", "http://www.w3.org/2005/Atom");
                var xpath = XPathExpression.Compile("//atom:entry", xnm);
                var xpni = xpn.Select(xpath);

                while (xpni.MoveNext())
                {
                    var t = new Tweet(this.imageCache);

                    var xtr = new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(xpni.Current.OuterXml)));

                    while (xtr.Read())
                    {
                        if (xtr.NodeType != XmlNodeType.Element)
                        {
                            continue;
                        }

                        switch (xtr.Name)
                        {
                            case "id":
                                var idbase = xtr.ReadElementContentAsString();
                                t.Id = (idbase != null) ? ulong.Parse(idbase.Split(':')[2]) : 0;
                                break;
                            case "title":
                                t.Content = HttpUtility.HtmlDecode(xtr.ReadElementContentAsString()).Replace("&", "&&");
                                break;
                            case "link":
                                t.ImageUrl = xtr.GetAttribute("href");
                                break;
                            case "name":
                                t.Author = xtr.ReadElementContentAsString();
                                break;
                            case "published":
                                t.Timestamp = xtr.ReadElementContentAsString();
                                break;
                        }
                    }

                    if (this.id < t.Id)
                    {
                        this.id = t.Id;
                    }

                    tweets.AddLast(t);
                }
            }

            return tweets;
        }

        #endregion
    }
}