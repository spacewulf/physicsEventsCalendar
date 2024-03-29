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
using System.Globalization;


namespace physicsEvents
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            
            string eventsUrl = "http://events.umich.edu/group/1965/rss?v=2&html_output=true";

            Console.WriteLine("Enter the first date: (M/D)");
            string startDate = Console.ReadLine();
            Console.WriteLine("Enter the last date: (M/D)");
            string endDate = Console.ReadLine();

            try
            {
                DateTime StartDate = DateTime.Parse(startDate);
                DateTime EndDate = DateTime.Parse(endDate);
                if (DateTime.Compare(StartDate, EndDate) > 0)
                {
                    throw new Exception("The start date must be prior to the end date.");
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Events[] events = Methods.GetEvents(eventsUrl, startDate, endDate);

            foreach ( Events e in events )
            {
                Console.WriteLine(e.Title);
                Console.WriteLine(e.Speaker);
                Console.WriteLine(e.Location);
                Console.WriteLine(e.Date);
                Console.WriteLine(e.StartTime);
                Console.WriteLine(e.EndTime);
            }
        }
    }
}
