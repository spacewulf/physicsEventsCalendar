using physicsEvents;
using System;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Xml;

public class Program
{
    public static void Main(string[] args)
    {
        string url = "http://events.umich.edu/group/1965/rss?v=2&html_output=true";
        int eventNumber = 0;

        XmlReader reader = XmlReader.Create(url);

        SyndicationFeed feed = SyndicationFeed.Load(reader);

        reader.Close();

        foreach (SyndicationItem item in feed.Items)
        {
            eventNumber++;
            Events events1 = new Events();
            events1.Title = item.Title.Text.ToString();
            events1.Description = item.Summary.Text.ToString();
            events1.Uri = item.BaseUri;
            Console.WriteLine(events1.Title);
            // Console.WriteLine(events1.Description);
            // Console.WriteLine(events1.Uri.ToString());
        }
    }
}