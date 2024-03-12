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

public class Program
{
    public static void Main(string[] args)
    {
        string eventsUrl = "http://events.umich.edu/group/1965/rss?v=2&html_output=true";

        string searchInput = @"""name"": ";

        Events[] events = Methods.fetchEvents(eventsUrl);

        Uri[] uris = Methods.fetchUri(events);

        string[] bodies = Methods.fetchHtmlText(uris);



        Console.WriteLine(events[0].Uri.ToString());
        Console.WriteLine(Regex.Matches(bodies[0], searchInput).Count);
    }
}