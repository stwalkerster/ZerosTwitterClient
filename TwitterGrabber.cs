using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;

namespace ZerosTwitterClient
{
    class TwitterGrabber
    {
        private static readonly object _twitterLock = new object();

        private static ulong _id;

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
                webRequest.Timeout = 5000;
                var response = (HttpWebResponse) webRequest.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException(response.StatusDescription);
                }

                Stream responseStream;

                if ((responseStream = response.GetResponseStream()) == null)
                    throw new ArgumentException();


                string originaldata = new StreamReader(responseStream).ReadToEnd();
                string newdata = originaldata
                    .Replace("<georss:", "<")
                    .Replace("</georss:", "</");

                Stream datas = new MemoryStream(Encoding.ASCII.GetBytes(newdata));

                var xpd = new XPathDocument(datas);
                
                var xpn = xpd.CreateNavigator();
                var xnm = new XmlNamespaceManager(xpn.NameTable);
                xnm.AddNamespace("atom", "http://www.w3.org/2005/Atom");
                var xpath = XPathExpression.Compile("//atom:entry", xnm);
                var xpni = xpn.Select(xpath);


                while (xpni.MoveNext())
                {
                    var t = new Tweet();


                    var xtr = new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(xpni.Current.OuterXml)));

                    while (xtr.Read())
                    {
                        if (xtr.NodeType != XmlNodeType.Element) continue;

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
                                t.Image = xtr.GetAttribute("href");
                                break;
                            case "name":
                                t.Author = xtr.ReadElementContentAsString();
                                break;
                            case "published":
                                t.Timestamp = xtr.ReadElementContentAsString();
                                break;
                        }
                    }


                    if (_id < t.Id)
                    {
                        _id = t.Id;
                    }

                    tweets.AddLast(t);
                }
            }
            return tweets;
        }

    }
}
