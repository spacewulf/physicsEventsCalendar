using physicsEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Xml;

namespace physicsEvents
{
    internal class Methods
    {
        public static Events[] fetchEvents(string url)
        {
            int eventNumber = 0;

            XmlReader reader = XmlReader.Create(url);

            SyndicationFeed feed = SyndicationFeed.Load(reader);

            reader.Close();

            int size = feed.Items.Count();

            int eventIter = 0;

            Events[] events = new Events[size];

            foreach (SyndicationItem item in feed.Items)
            {
                Events Event = new Events();
                Event.Title = item.Title.Text.ToString();
                Event.Description = item.Summary.Text.ToString();
                Event.Uri = item.Links[0].Uri;
                events[eventIter] = Event;
                eventIter++;
            }

            return events;
        }
    }
}
