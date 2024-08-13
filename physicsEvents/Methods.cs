using physicsEventsCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Xml;
using System.IO;
using HtmlAgilityPack;
using System.Net.NetworkInformation;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Configuration;
using Google.Apis.Calendar.v3.Data;

namespace physicsEventsCalendar
{
    internal class Methods
    {
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
        public static PhysicsEvents[] CollectEvents(string uri)
        {
            PhysicsEvents[] events = Fetch.Events(uri);
            string[] bodies = Fetch.BodyText(uri);
            PhysicsEvents[] eventsOutput = Assign.SpeakerName(events, bodies);
            eventsOutput = Assign.Streamed(eventsOutput, bodies);
            eventsOutput = Assign.Location(eventsOutput, bodies);
            eventsOutput = Assign.Date(eventsOutput, bodies);
            eventsOutput = Assign.EventUri(eventsOutput, bodies);
            //eventsOutput = Assign.Organization(eventsOutput);
            eventsOutput = Assign.EventId(eventsOutput, bodies);
            eventsOutput = Assign.Title(eventsOutput);
            return eventsOutput;
        }
        //Methods used in Program.cs
        public static Event[] GetEvents(string uri, DateTime StartDate, DateTime EndDate)
        {
            PhysicsEvents[] events = CollectEvents(uri);
            int checker = 0;

            foreach (PhysicsEvents e in events)
            {
                if ( DateTime.Compare(StartDate, e.Date) <= 0 
                    && DateTime.Compare(e.Date, EndDate) <= 0)
                {
                    checker++;
                }
            }

            PhysicsEvents[] eventsOutput = new PhysicsEvents[checker];

            int iter = 0;
            foreach (PhysicsEvents e in events)
            {
                eventsOutput[iter] = e;
                iter++;

            }
            return ConvertEvents(eventsOutput);
        }
        /*public static string Organization(string title)
        {
            return title.Substring(0, title.IndexOf("|"));
        }*/

        // Methods used in CalendarAccess.cs

        public static string[] GetIds(Event[] events)
        {
            String[] ids = new String[events.Length];

            int iter = 0;

            foreach (Event e in events)
            {
                ids[iter] = e.Id;
                ++iter;
            }

            return ids;
        }

        public static Event[] ConvertEvents(PhysicsEvents[] physicsEvents)
        {

            Event[] events = new Event[physicsEvents.Length];

            int iter = 0;

            foreach (PhysicsEvents physicsEvent in physicsEvents)
            {
                Event ev = new Event();

                EventDateTime start = new EventDateTime();
                EventDateTime end = new EventDateTime();

                int eventYear = physicsEvent.Date.Year;
                int eventMonth = physicsEvent.Date.Month;
                int eventDay = physicsEvent.Date.Day;
                int eventStartHour = Int32.Parse(physicsEvent.StartTime.Substring(0, 2));
                int eventEndHour = Int32.Parse(physicsEvent.EndTime.Substring(0, 2));
                int eventStartMinute = Int32.Parse(physicsEvent.StartTime.Substring(3));
                int eventEndMinute = Int32.Parse(physicsEvent.EndTime.Substring(3));

                start.DateTime = new DateTime(eventYear, eventMonth, eventDay, eventStartHour, eventStartMinute, 0);
                end.DateTime = new DateTime(eventYear, eventMonth, eventDay, eventEndHour, eventEndMinute, 0);


                ev.Start = start;
                ev.End = end;
                ev.Summary = physicsEvent.Title;
                ev.Description = physicsEvent.Summary;
                ev.Id = physicsEvent.EventId.ToString();
                ev.Location = physicsEvent.Location;

                events[iter] = ev;
                ++iter;
            }

            return events;
        }
    }
}
