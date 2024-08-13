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
        public static Events[] CollectEvents(string uri)
        {
            Events[] events = Fetch.Events(uri);
            string[] bodies = Fetch.BodyText(uri);
            Events[] eventsOutput = Assign.SpeakerName(events, bodies);
            eventsOutput = Assign.Streamed(eventsOutput, bodies);
            eventsOutput = Assign.Location(eventsOutput, bodies);
            eventsOutput = Assign.Date(eventsOutput, bodies);
            eventsOutput = Assign.EventUri(eventsOutput, bodies);
            return eventsOutput;
        }
        //Methods used in Program.cs
        public static Events[] GetEvents(string uri, DateTime StartDate, DateTime EndDate)
        {
            Events[] events = CollectEvents(uri);
            int checker = 0;

            foreach (Events e in events)
            {
                if ( DateTime.Compare(StartDate, e.Date) <= 0 
                    && DateTime.Compare(e.Date, EndDate) <= 0)
                {
                    checker++;
                }
            }

            Events[] eventsOutput = new Events[checker];

            int iter = 0;
            foreach (Events e in events)
            {
                if (DateTime.Compare(StartDate, e.Date) <= 0
                    && DateTime.Compare(e.Date, EndDate) <= 0)
                {
                    eventsOutput[iter] = e;
                    iter++;
                }
            }
            return eventsOutput;

        }

        public static bool CheckConfig()
        {
            try
            {
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ConfigurationManager.AppSettings.Get("FirstLaunch");
            } catch (ConfigurationErrorsException)
            {
                return false;
            }
            return true;
        }
        public static async void GenerateConfigFile()
        {
            XmlWriter writer = null;

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("\t");
                settings.OmitXmlDeclaration = false;

                writer = XmlWriter.Create("physicsEventsCalendar.dll.config", settings);
                writer.WriteStartElement("configuration");
                writer.WriteStartElement("appSettings");

                string[,] keyPairs = { { "FirstLaunch", "true" }, { "Path", "" }, { "DynamicNaming", "true" }, { "Name", "" }, { "HyperlinkColor", "0057E4" } };

                for (int i = 0; i < keyPairs.GetLength(0); i++)
                {
                    WriteKeyValuePair(writer, [keyPairs[i, 0], keyPairs[i, 1]]);
                }

                writer.Flush();
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

        }
        public static void WriteKeyValuePair(XmlWriter writer, string[] keyValuePair)
        {
            writer.WriteStartElement("add");
            writer.WriteAttributeString("key", keyValuePair[0]);
            writer.WriteAttributeString("value", keyValuePair[1]);
            writer.WriteEndElement();
        }
        public static string Organization(string title)
        {
            return title.Substring(0, title.IndexOf("|"));
        }
        public static string ReturnMonth(int month)
        {
            string output;
            switch (month)
            {
                case 1:
                    output = "January";
                    break;
                case 2:
                    output = "February";
                    break;
                case 3:
                    output = "March";
                    break;
                case 4:
                    output = "April";
                    break;
                case 5:
                    output = "May";
                    break;
                case 6:
                    output = "June";
                    break;
                case 7:
                    output = "July";
                    break;
                case 8:
                    output = "August";
                    break;
                case 9:
                    output = "September";
                    break;
                case 10:
                    output = "October";
                    break;
                case 11:
                    output = "November";
                    break;
                case 12:
                    output = "December";
                    break;
                default:
                    throw new Exception("Not a valid month.");
            }
            return output;
        }
    }
}
