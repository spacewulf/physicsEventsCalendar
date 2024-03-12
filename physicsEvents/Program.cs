using physicsEvents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Xml;
using static System.Convert;

public class Program
{
    public static void Main(string[] args)
    {
        string url = "http://events.umich.edu/group/1965/rss?v=2&html_output=true";

        Events[] events = Methods.fetchEvents(url);

        foreach (Events e in events)
        {
            Console.WriteLine(e.Uri.ToString());
        }

    }
}