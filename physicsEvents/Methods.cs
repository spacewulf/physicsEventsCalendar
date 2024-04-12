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
using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace physicsEvents
{
    internal class Methods
    {
        public static Hyperlink HyperlinkManager(string url, MainDocumentPart mainPart)
        {
            HyperlinkRelationship hr = mainPart.AddHyperlinkRelationship(new Uri(url), true);
            string hrContactId = hr.Id;
            return
                new Hyperlink(
                    new ProofError() { Type = ProofingErrorValues.GrammarStart },
                    new Run(
                        new RunProperties(
                            new RunStyle() { Val = "Hyperlink" })))
                { History = OnOffValue.FromBoolean(true), Id=hrContactId};
        }
        public static HtmlDocument FetchHtmlPage(Uri uri)
        {
            var web = new HtmlWeb();
            var doc = web.Load(uri);
            return doc;
        }
        public static HtmlDocument[] FetchHtmlPages(Uri[] uri)
        {
            int pageNumber = uri.Length;
            int pageIter = 0;
            HtmlDocument[] docs = new HtmlDocument[pageNumber];
            foreach ( Uri item in uri )
            {
                docs[pageIter] = FetchHtmlPage(item);
                pageIter++;
            }
            return docs;
        }

        public static string[] FetchHtmlText(HtmlDocument[] docs)
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

        public static string[] FetchHtmlText(Uri[] uris)
        {
            HtmlDocument[] pages = FetchHtmlPages(uris);
            string[] bodies = FetchHtmlText(pages);
            return bodies;
        }
        public static Uri FetchUri(Events Event)
        {
            return Event.Uri;
        }

        public static Uri[] FetchUri(Events[] events)
        {
            Uri[] uris = new Uri[events.Length];
            for (int i = 0; i < events.Length; i++)
            {
                uris[i] = events[i].Uri;
            }
            return uris;
        }

        public static Events[] FetchEvents(string url)
        {

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
        public static int SecondOccurrence(string input, string searchInput)
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
        public static string[] FetchDate(string input)
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
        public static int FetchNameIndex(string input)
        {
            return input.LastIndexOf(@"""name"": ") + 9;
        }
        public static string FetchSpeakerName(string input)
        {
            int startIndex = FetchNameIndex(input);
            int endIndex = input.IndexOf(@"""", startIndex);
            int length = endIndex - startIndex;
            return input.Substring(startIndex, length);
        }
        public static string FetchLocation(string input)
        {
            int startIndexOne = SecondOccurrence(input, @"""name"": ") + 9;
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
        public static string[] FetchBodyText(string eventsUri)
        {
            Events[] events = Methods.FetchEvents(eventsUri);

            Uri[] uris = Methods.FetchUri(events);

            string[] bodies = Methods.FetchHtmlText(uris);

            return bodies;
        }

        public static Events[] AssignDate(Events[] events, string[] bodies)
        {
            int iter = 0;
            foreach (Events e in events)
            {
                string[] dates = Methods.FetchDate(bodies[iter]);
                string date = dates[0].Substring(0, dates[0].IndexOf("T"));
                DateTime dateTemp = DateTime.Parse(date);
                e.Date = DateTime.Parse(date);
                e.DateUri = new System.Uri("https://lsa.umich.edu/physics/news-events/all-events.html#date=" + date + "&view=day");

                int startIndex = dates[0].IndexOf("T") + 1;
                int length = dates[0].Substring(startIndex).IndexOf("-") - 3;
                string time = dates[0].Substring(startIndex, length);
                e.StartTime = time;

                int endStartIndex = dates[1].IndexOf("T") + 1;
                int endLength = dates[1].Substring(startIndex).IndexOf("-") - 3;
                string endTime = dates[1].Substring(startIndex, length);
                e.EndTime = endTime;
                iter++;
            }
            return events;
        }
        public static Events[] AssignSpeakerName(Events[] events, string[] bodies)
        {
            int iter = 0;
            foreach (Events e in events)
            {
                e.Speaker = FetchSpeakerName(bodies[iter]);
                iter++;
            }
            return events;
        }
        public static Events[] AssignLocation(Events[] events, string[] bodies)
        {
            int iter = 0;
            foreach (Events e in events)
            {
                e.Location = FetchLocation(bodies[iter]);
                iter++;
            }
            return events;
        }
        public static Events[] CollectEvents(string uri)
        {
            Events[] events = FetchEvents(uri);
            string[] bodies = FetchBodyText(uri);
            Events[] eventsOutput = AssignSpeakerName(events, bodies);
            eventsOutput = AssignLocation(eventsOutput, bodies);
            eventsOutput = AssignDate(eventsOutput, bodies);
            return eventsOutput;
        }

        public static Events[] GetEvents(string uri, string startDate, string endDate)
        {
            Events[] events = CollectEvents(uri);
            DateTime StartDate = DateTime.Parse(startDate);
            DateTime EndDate = DateTime.Parse(endDate);
            int checker = 0;

            foreach (Events e in events)
            {
                if ( DateTime.Compare(StartDate, e.Date) <= 0 
                    && DateTime.Compare(e.Date, EndDate) <= 0)
                {
                    checker++;
                }
            }

            Events[] eventsOutput = new Events[checker];

            int iter = 0;
            foreach (Events e in events)
            {
                if (DateTime.Compare(StartDate, e.Date) <= 0
                    && DateTime.Compare(e.Date, EndDate) <= 0)
                {
                    eventsOutput[iter] = e;
                    iter++;
                }
            }
            return eventsOutput;

        }
    }
}
