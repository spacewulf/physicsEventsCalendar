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
    internal class Assign
    {
        public static Events[] Date(Events[] events, string[] bodies)
        {
            int iter = 0;
            foreach (Events e in events)
            {
                string[] dates = Fetch.Date(bodies[iter]);
                string date = dates[0].Substring(0, dates[0].IndexOf("T"));
                DateTime dateTemp = DateTime.Parse(date);
                e.Date = DateTime.Parse(date);
                e.DateUri = new System.Uri("https://lsa.umich.edu/physics/news-events/all-events.html#date=" + date + "&view=day"); //Must change if you want a different department

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
        public static Events[] SpeakerName(Events[] events, string[] bodies)
        {
            int iter = 0;
            foreach (Events e in events)
            {
                e.Speaker = Fetch.SpeakerName(bodies[iter]);
                iter++;
            }
            return events;
        }

        public static Events[] EventUri(Events[] events, string[] bodies)
        {
            int iter = 0;
            foreach (Events e in events)
            {
                e.Uri = Fetch.EventUri(bodies[iter]);
                iter++;
            }
            return events;
        }

        public static Events[] Location(Events[] events, string[] bodies)
        {
            int iter = 0;
            foreach (Events e in events)
            {
                e.Location = Fetch.Location(bodies[iter]);
                iter++;
            }
            return events;
        }

        public static Events[] Streamed(Events[] events, string[] bodies)
        {
            int iter = 0;
            foreach (Events e in events)
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
