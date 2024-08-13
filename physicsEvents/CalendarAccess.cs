using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Vbe.Interop;
using Events = Google.Apis.Calendar.v3.Data.Events;

namespace physicsEventsCalendar
{
    class CalendarAccess
    {
        static string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "UMich Physics Events Google Calendar";

        static Event[] QueryEvents()
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.TimeMax = DateTime.Now.AddMonths(9);
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Events events = request.Execute();

            Event[] outputEvents = new Event[events.Items.Count];

            int iter = 0;
            foreach (Event e in events.Items)
            {
                outputEvents[iter] = e;
                ++iter;
            }

            return outputEvents;
        }

        public static void InsertEvents(Event[] events, string calendarId)
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            string[] ids = Methods.GetIds(events);

            foreach (Event e in events)
            {
                if (!ids.Contains(e.Id))
                {
                    service.Events.Insert(e, calendarId);
                } 
                else
                {
                    Event existingEvent = service.Events.Get(calendarId, e.Id).Execute();

                    existingEvent.Summary = e.Summary;
                    existingEvent.Description = e.Description;
                    existingEvent.Start = e.Start;
                    existingEvent.End = e.End;
                    existingEvent.Location = e.Location;

                    service.Events.Update(existingEvent, calendarId, e.Id);
                }
            }
        }

        public static void SetEvents(Event[] events, string calendarId)
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            foreach (Event e in events)
            {
                service.Events.Insert(e, calendarId);
            }
        }
    }
}
