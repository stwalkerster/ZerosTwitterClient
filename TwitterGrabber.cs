using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.XPath;

namespace ZerosTwitterClient
{
    class TwitterGrabber
    {
        private static readonly object _twitterLock = new object();

        private static ulong _id = 0;

        public static LinkedList<Tweet> getTweets(string search = "#hwunion")
        {
            var tweets = new LinkedList<Tweet>();

            lock (_twitterLock)
            {
                var webRequest = (HttpWebRequest) WebRequest.Create(
                    "http://search.twitter.com/search.atom?since_id=" + _id +
                    "&result_type=recent" +
                    "&q=" + System.Web.HttpUtility.UrlEncode(search)
                                                      );

                webRequest.UserAgent = "HwunionTwitterClient/1.0 (+ simon@stwalkerster.net )";

                var response = (HttpWebResponse) webRequest.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException(response.StatusDescription);
                }

                Stream responseStream;

                if ((responseStream = response.GetResponseStream()) == null)
                    throw new ArgumentException();

                var xpd = new XPathDocument(responseStream);
                var xpn = xpd.CreateNavigator();
                var xpni = xpn.Select("//entry");


                while (xpni.MoveNext())
                {
                    var t = new Tweet();

                    Debug.Assert(xpni.Current != null, "xpni.Current != null");

                    string idbase;
                    t.Content = (string) xpni.Current.Evaluate(XPathExpression.Compile("/title/text()"), xpni);
                    t.Author = (string) xpni.Current.Evaluate(XPathExpression.Compile("/author/name/text()"), xpni);
                    t.Id = ((idbase = ((string) xpni.Current.Evaluate(XPathExpression.Compile("/id/text()"), xpni))) !=
                            null)
                               ? ulong.Parse(idbase.Split(':')[2])
                               : 0;
                    t.Image = (string) xpni.Current.Evaluate(XPathExpression.Compile("/link/attribute::href"), xpni);

                    if (_id < t.Id) _id = t.Id;

                    tweets.AddLast(t);
                }
            }
            return tweets;
        }
    }
}
