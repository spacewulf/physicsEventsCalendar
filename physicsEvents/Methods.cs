using physicsEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Xml;
using HtmlAgilityPack;
using System.Net.NetworkInformation;

namespace physicsEvents
{
    internal class Methods
    {
        public static HtmlDocument fetchHtmlPage(Uri uri)
        {
            var web = new HtmlWeb();
            var doc = web.Load(uri);
            return doc;
        }

        // public static HtmlDocument[] fetchHtmlPage(string[] url)
        // {
        //     int pageIter = 0;
        //     int pageNumber = url.Length;
        //     HtmlDocument[] docs = new HtmlDocument[pageNumber];
        //     foreach ( var item in url )
        //     {
        //         var web = new HtmlWeb();
        //         var doc = web.Load(item);
        //         docs[pageIter] = doc;
        //         pageIter++;
        //     }
        //     return docs;
        // }
        public static HtmlDocument[] fetchHtmlPages(Uri[] uri)
        {
            int pageNumber = uri.Length;
            int pageIter = 0;
            HtmlDocument[] docs = new HtmlDocument[pageNumber];
            foreach ( Uri item in uri )
            {
                docs[pageIter] = fetchHtmlPage(item);
                pageIter++;
            }
            return docs;
        }

        public static string[] fetchHtmlText(HtmlDocument[] docs)
        {
            int pageNumber = 0;
            string[] strings = new string[docs.Length];
            foreach ( HtmlDocument doc in docs )
            {
                string text = doc.Text;
                strings[pageNumber] = text;
                pageNumber++;
            }
            return strings;
        }

        public static string[] fetchHtmlText(Uri[] uris)
        {
            HtmlDocument[] pages = fetchHtmlPages(uris);
            string[] bodies = fetchHtmlText(pages);
            return bodies;
        }
        public static Uri fetchUri(Events Event)
        {
            return Event.Uri;
        }

        public static Uri[] fetchUri(Events[] events)
        {
            Uri[] uris = new Uri[events.Length];
            for (int i = 0; i < events.Length; i++)
            {
                uris[i] = events[i].Uri;
            }
            return uris;
        }

        public static Events[] fetchEvents(string url)
        {
            // int eventNumber = 0;

            XmlReader reader = XmlReader.Create(url);

            SyndicationFeed feed = SyndicationFeed.Load(reader);

            reader.Close();

            int size = feed.Items.Count();

            int eventIter = 0;

            Events[] events = new Events[size];

            foreach (SyndicationItem item in feed.Items)
            {
                Events Event = new Events();
                Event.Title = item.Title.Text.ToString();
                Event.Uri = item.Links[0].Uri;
                events[eventIter] = Event;
                eventIter++;
            }

            return events;
        }
    }
}
