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
    internal class Assign
    {
        public static PhysicsEvents[] Date(PhysicsEvents[] events, string[] bodies)
        {
            int iter = 0;
            foreach (PhysicsEvents e in events)
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
        public static PhysicsEvents[] GroupId(PhysicsEvents[] events, string[] bodies)
        {
            int iter = 0;
            foreach (PhysicsEvents e in events)
            {
                e.GroupId = Fetch.GroupId(bodies[iter]);
                ++iter;
            }
            return events;
        }
        public static PhysicsEvents[] SpeakerName(PhysicsEvents[] events, string[] bodies)
        {
            int iter = 0;
            foreach (PhysicsEvents e in events)
            {
                e.Speaker = Fetch.SpeakerName(bodies[iter]);
                iter++;
            }
            return events;
        }

        public static PhysicsEvents[] EventUri(PhysicsEvents[] events, string[] bodies)
        {
            int iter = 0;
            foreach (PhysicsEvents e in events)
            {
                e.Uri = Fetch.EventUri(bodies[iter]);
                iter++;
            }
            return events;
        }

        public static PhysicsEvents[] EventId(PhysicsEvents[] events, string[] bodies)
        {
            int iter = 0;
            foreach(PhysicsEvents e in events)
            {
                e.EventId = Fetch.EventId(bodies[iter]);
                iter++;
            }
            return events;
        }

        public static PhysicsEvents[] Location(PhysicsEvents[] events, string[] bodies)
        {
            int iter = 0;
            foreach (PhysicsEvents e in events)
            {
                e.Location = Fetch.Location(bodies[iter]);
                iter++;
            }
            return events;
        }

        public static PhysicsEvents[] Streamed(PhysicsEvents[] events, string[] bodies)
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
        /*public static PhysicsEvents[] Organization(PhysicsEvents[] events)
        {
            foreach (PhysicsEvents e in events)
            {
                e.Organization = Methods.Organization(e.Title);
            }
            return events;
        }*/
        public static PhysicsEvents[] Title(PhysicsEvents[] events)
        {
            foreach (PhysicsEvents e in events)
            {
                e.Title = e.Title + " | " + e.Speaker;
            }
            return events;
        }
    }
}
