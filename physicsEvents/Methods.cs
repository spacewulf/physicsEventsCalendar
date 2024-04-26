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
using System.Text.RegularExpressions;

namespace physicsEvents
{
    internal class Methods
    {
        //Methods Used in GenerateWordDocument.cs
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
        //Methods Used in Fetch.cs
        public static int SecondOccurrence(string input, string searchInput)
        {
            int indexFirst = input.IndexOf(searchInput);
            try
            {
                int indexSecond = input.IndexOf(searchInput, indexFirst + 1);
                return indexSecond;
            }
            catch
            {
                return -1;
            }
        }
        //Methods used internally in Methods.cs
        public static Events[] CollectEvents(string uri)
        {
            Events[] events = Fetch.Events(uri);
            string[] bodies = Fetch.BodyText(uri);
            Events[] eventsOutput = Assign.SpeakerName(events, bodies);
            eventsOutput = Assign.Streamed(eventsOutput, bodies);
            eventsOutput = Assign.Location(eventsOutput, bodies);
            eventsOutput = Assign.Date(eventsOutput, bodies);
            eventsOutput = Assign.EventUri(eventsOutput, bodies);
            return eventsOutput;
        }
        //Methods used in Program.cs
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
