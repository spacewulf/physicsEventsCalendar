using physicsEvents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Xml;
using HtmlAgilityPack;
using static System.Convert;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace physicsEvents
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string eventsUrl = "http://events.umich.edu/group/1965/rss?v=2&html_output=true";

            string[] bodies = Methods.fetchBodyText(eventsUrl);

            Events[] events = Methods.getEvents(eventsUrl);
            foreach (Events e in events)
            {
                Console.WriteLine(e.Title);
                Console.WriteLine(e.Speaker);
                Console.WriteLine(e.Location);
            }
            // Console.WriteLine(bodies[1].Substring(Methods.fetchNameIndex(bodies[1])));

            // Console.WriteLine(events[0].Uri.ToString());
            // Console.WriteLine(Regex.Matches(bodies[0], searchInput).Count);
            // Console.WriteLine(bodies[0].LastIndexOf(searchInput));

        }
    }
}
