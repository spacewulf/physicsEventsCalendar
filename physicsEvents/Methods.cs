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
                Event.Title = item.Title.Text.ToString().Substring(0, item.Title.Text.ToString().IndexOf("("));
                Event.Uri = item.Links[0].Uri;
                events[eventIter] = Event;
                eventIter++;
            }

            return events;
        }
        public static int secondOccurrence(string input, string searchInput)
        {
            int indexFirst = input.IndexOf(searchInput);
            try
            {
                int indexSecond = input.IndexOf(searchInput, indexFirst + 1);
                return indexSecond;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public static string[] fetchDate(string input)
        {
            string[] strings = new string[2];

            int startIndex1 = input.IndexOf(@"""startDate"":") + 14;
            int endIndex1 = input.IndexOf(@"""", startIndex1);
            int length1 = endIndex1 - startIndex1;
            strings[0] = input.Substring(startIndex1, length1);

            int startIndex2 = input.IndexOf(@"""startDate"":") + 12;
            int endIndex2 = input.IndexOf(@"""", startIndex2);
            int length2 = endIndex2 - startIndex2;
            strings[1] = input.Substring(startIndex2, length2);

            return strings;
        }
        public static int fetchNameIndex(string input)
        {
            return input.LastIndexOf(@"""name"": ") + 9;
        }
        public static string fetchSpeakerName(string input)
        {
            int startIndex = fetchNameIndex(input);
            int endIndex = input.IndexOf(@"""", startIndex);
            int length = endIndex - startIndex;
            return input.Substring(startIndex, length);
        }
        public static string fetchLocation(string input)
        {
            int startIndexOne = secondOccurrence(input, @"""name"": ") + 9;
            int endIndexOne = input.IndexOf(@"""", startIndexOne);
            int lengthOne = endIndexOne - startIndexOne;
            string roomTag = @"""room"":";
            int startIndexTwo = input.IndexOf(roomTag) + 8;
            int endIndexTwo = input.IndexOf(@"""", startIndexTwo);
            int lengthTwo = endIndexTwo - startIndexTwo;
            string building = input.Substring(startIndexOne, lengthOne);
            string roomNumber = input.Substring(startIndexTwo, lengthTwo);
            string location = building + " " + roomNumber;
            return location;
        }
        public static string[] fetchBodyText(string eventsUri)
        {
            Events[] events = Methods.fetchEvents(eventsUri);

            Uri[] uris = Methods.fetchUri(events);

            string[] bodies = Methods.fetchHtmlText(uris);

            return bodies;
        }
        public static Events[] assignSpeakerName(Events[] events, string[] bodies)
        {
            int iter = 0;
            foreach (Events e in events)
            {
                e.Speaker = fetchSpeakerName(bodies[iter]);
                iter++;
            }
            return events;
        }
        public static Events[] assignLocation(Events[] events, string[] bodies)
        {
            int iter = 0;
            foreach (Events e in events)
            {
                e.Location = fetchLocation(bodies[iter]);
                iter++;
            }
            return events;
        }
        public static Events[] getEvents(string uri)
        {
            Events[] events = fetchEvents(uri);
            string[] bodies = fetchBodyText(uri);
            Events[] eventsOutput = assignSpeakerName(events, bodies);
            eventsOutput = assignLocation(eventsOutput, bodies);
            return eventsOutput;
        }
    }
}
