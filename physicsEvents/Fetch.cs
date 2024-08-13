using physicsEventsCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Xml;
using HtmlAgilityPack;
using System.Net.NetworkInformation;
using System.Globalization;
using System.Text.RegularExpressions;

namespace physicsEventsCalendar
{
    internal class Fetch
    {
        //Methods called internally within Methods.cs
        public static HtmlDocument HtmlPage(Uri uri)
        {
            var web = new HtmlWeb();
            var doc = web.Load(uri);
            return doc;
        }
        public static HtmlDocument[] HtmlPages(Uri[] uri)
        {
            int pageNumber = uri.Length;
            int pageIter = 0;
            HtmlDocument[] docs = new HtmlDocument[pageNumber];
            foreach ( Uri item in uri )
            {
                docs[pageIter] = HtmlPage(item);
                pageIter++;
            }
            return docs;
        }

        public static string[] HtmlText(HtmlDocument[] docs)
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

        public static string[] HtmlText(Uri[] uris)
        {
            HtmlDocument[] pages = HtmlPages(uris);
            string[] bodies = HtmlText(pages);
            return bodies;
        }
        public static Uri FetchUri(PhysicsEvents Event)
        {
            return Event.Uri;
        }

        public static Uri[] Uri(PhysicsEvents[] events)
        {
            Uri[] uris = new Uri[events.Length];
            for (int i = 0; i < events.Length; i++)
            {
                uris[i] = events[i].Uri;
            }
            return uris;
        }

        public static PhysicsEvents[] Events(string url)
        {

            XmlReader reader = XmlReader.Create(url);

            SyndicationFeed feed = SyndicationFeed.Load(reader);

            reader.Close();

            int size = feed.Items.Count();

            int eventIter = 0;

            PhysicsEvents[] events = new PhysicsEvents[size];

            foreach (SyndicationItem item in feed.Items)
            {
                PhysicsEvents Event = new PhysicsEvents();
                Event.Title = item.Title.Text.ToString().Substring(0, item.Title.Text.ToString().IndexOf("("));
                Event.Uri = item.Links[0].Uri;
                events[eventIter] = Event;
                eventIter++;
            }

            return events;
        }
        public static int GroupId(string input)
        {
            int startIndex = input.IndexOf("/group/") + 7;
            int output = Int32.Parse(input.Substring(startIndex, 4));
            return output;
        }
        public static string[] Date(string input)
        {
            string[] strings = new string[2];

            int startIndex1 = input.IndexOf(@"""startDate"":") + 14;
            int endIndex1 = input.IndexOf(@"""", startIndex1);
            int length1 = endIndex1 - startIndex1;
            strings[0] = input.Substring(startIndex1, length1);

            int startIndex2 = input.IndexOf(@"""endDate"":") + 12;
            int endIndex2 = input.IndexOf(@"""", startIndex2);
            int length2 = endIndex2 - startIndex2;
            strings[1] = input.Substring(startIndex2, length2);

            return strings;
        }
        public static int NameIndex(string input)
        {
            return input.LastIndexOf(@"""name"": ") + 9;
        }
        public static Uri EventUri(string input)
        {
            int eventsIndex = input.IndexOf(@"""iCal_href"": ");
            string uriSubstring = input.Substring(eventsIndex, 36).Substring(21, 15);
            return new System.Uri("https://lsa.umich.edu/physics/news-events/all-events.detail.html/" + uriSubstring + ".html"); //This must be changed when moving to a different department
        }
        public static int EventId(string input)
        {
            int eventsIndex = input.IndexOf(@"""iCal_href"": ");

            Int32.TryParse(input.Substring(eventsIndex, 36).Substring(21, 15), out int result);

            return result;
        }
        public static string SpeakerName(string input)
        {
            int startIndex = NameIndex(input);
            int endIndex = input.IndexOf(@"""", startIndex);
            int length = endIndex - startIndex;
            return input.Substring(startIndex, length);
        }
        public static string Location(string input)
        {
            int startIndexOne = Methods.SecondOccurrence(input, @"""name"": ") + 9;
            int endIndexOne = input.IndexOf(@"""", startIndexOne);
            int lengthOne = endIndexOne - startIndexOne;
            string roomTag = @"""room"":";
            int startIndexTwo = input.IndexOf(roomTag) + 8;
            int endIndexTwo = input.IndexOf(@"""", startIndexTwo);
            int lengthTwo = endIndexTwo - startIndexTwo;
            string building = input.Substring(startIndexOne, lengthOne);
            string roomNumber = input.Substring(startIndexTwo, lengthTwo);
            string location = roomNumber + " " + building;
            int count = Regex.Matches(input, "zoom.us").Count;
            if (count > 1)
            {
                int zoomIndex = input.IndexOf("https://umich.zoom.us");
                string zoomLink = input.Substring(zoomIndex, 35);
                if (int.TryParse(zoomLink.Substring(33), out int i) == false)
                {
                    zoomIndex = input.IndexOf("https://zoom.us");
                    zoomLink = input.Substring(zoomIndex, 29);
                }
                location = location + " " + zoomLink;
            }
            return location;
        }
        public static string[] BodyText(string eventsUri)
        {
            PhysicsEvents[] events = Events(eventsUri);

            Uri[] uris = Uri(events);

            string[] bodies = HtmlText(uris);

            return bodies;
        }

        public static PhysicsEvents[] AssignStreamed(PhysicsEvents[] events, string[] bodies)
        {
            int iter = 0;
            foreach (PhysicsEvents e in events)
            {
                int count = Regex.Matches(bodies[iter], "livestream").Count;
                if ((bodies[iter].IndexOf("live stream", StringComparison.OrdinalIgnoreCase) >= 0) | count > 9)
                {
                    e.IsLivestreamed = true;
                } else { e.IsLivestreamed = false;}
                iter++;
            }
            return events;
        }
    }
}
