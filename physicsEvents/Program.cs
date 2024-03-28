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
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.IO;


namespace physicsEvents
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string eventsUrl = "http://events.umich.edu/group/1965/rss?v=2&html_output=true";

            Events[] events = Methods.getEvents(eventsUrl);
            //foreach (Events e in events)
            //{
            //   Console.WriteLine(e.Title);
            //    Console.WriteLine(e.Speaker);
            //    Console.WriteLine(e.Location);
            //}
            // int eventNumber = 0;
            Events[] events1 = Methods.fetchEvents(eventsUrl);
            string[] bodies = Methods.fetchBodyText(eventsUrl);
            string[] dates = Methods.fetchDate(bodies[0]);
            Console.WriteLine(dates[0]);
        }

    }
}
