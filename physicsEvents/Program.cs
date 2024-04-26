using physicsEvents;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Xml;
using HtmlAgilityPack;
using static System.Convert;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using DocumentFormat.OpenXml.Wordprocessing;


namespace physicsEvents
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string path = @"c:\Users\Kees Wolterstorff\Desktop\test.docx";

            //ConfigurationManager.AppSettings.Set("Path", path);

            string pathGeneric;

            pathGeneric = ConfigurationManager.AppSettings.Get("Path");

            string eventsUrl = "http://events.umich.edu/group/1965/rss?v=2&html_output=true";

            Start:

            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Run the program");
            Console.WriteLine("2: Modify Settings");
            Console.WriteLine("3: Quit");
            string choice = Console.ReadLine();
            
            if (!Int32.TryParse(choice, out int value))
            {
                Console.Clear();
                Console.WriteLine("That's not a valid choice.");
                Console.WriteLine();
                goto Start;
            }
            
            int Choice = Int32.Parse(choice);

            switch (Choice)
            {
                case 1:
                    goto StartProgram;
                case 2:
                    goto ChangeSettings;
                case 3:
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("That's not a valid choice.");
                    goto Start;
            }

        StartProgram:
            pathGeneric = ConfigurationManager.AppSettings.Get("Path") + ConfigurationManager.AppSettings.Get("Name") + ".docx";
            Console.Clear();
            Console.WriteLine(path);
            Console.WriteLine(pathGeneric);
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

            GenerateWordDocument.Create(events, pathGeneric, DateTime.Parse(startDate), DateTime.Parse(endDate));
            Console.Clear();
            Console.WriteLine("Successfully generated word document to: " + ConfigurationManager.AppSettings.Get("Path") + ConfigurationManager.AppSettings.Get("Name"));
            Console.WriteLine("Would you like to continue? [Y/N]");

            string finishChoice = Console.ReadLine();

            switch (finishChoice)
            {
                case "Y":
                    Console.Clear();
                    goto Start;
                case "y":
                    Console.Clear();
                    goto Start;
                case "N":
                    System.Environment.Exit(0);
                    break;
                case "n":
                    System.Environment.Exit(0);
                    break;
            }

            goto Start;

            ChangeSettings:
            Console.Clear();
            Console.WriteLine("Which settings would you like to change?");
            Console.WriteLine("1: Path of File");
            Console.WriteLine("2: Name of File");
            Console.WriteLine("3: Return");
            string settingsChoice = Console.ReadLine();

            if (Int32.TryParse(settingsChoice, out int valueSettings))
            {
                int intChoice = Convert.ToInt32(choice);
                if (intChoice < 1 | intChoice > 3)
                {
                    Console.Clear();
                    Console.WriteLine("That's not a valid choice.");
                    Console.WriteLine();
                    goto Start;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("That's not a valid choice.");
                Console.WriteLine();
                goto Start;
            }

            int SettingsChoice = Convert.ToInt32(settingsChoice);

            string confirmSetting;
            string returnString;

            switch (SettingsChoice)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("Please enter the path you would like the file saved to.");
                    Console.WriteLine(@"Ex: c:\Users\Kees Wolterstorff\Desktop\");
                    string enteredPath = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("The path you entered was: " + enteredPath);
                    Console.WriteLine("Your current path is: " + ConfigurationManager.AppSettings.Get("Path"));
                    Console.WriteLine("Are you sure? [Y/N]");
                    confirmSetting = Console.ReadLine();
                    switch (confirmSetting)
                    {
                        case "Y":
                            config.AppSettings.Settings["Path"].Value = enteredPath;
                            config.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                            goto ChangeSettings;
                        case "y":
                            config.AppSettings.Settings["Path"].Value = enteredPath;
                            config.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                            goto ChangeSettings;
                        case "N":
                            Console.Clear();
                            Console.WriteLine("Nothing changed. Press enter to continue.");
                            goto ChangeSettings;
                        case "n":
                            Console.Clear();
                            Console.WriteLine("Nothing changed. Press enter to continue.");
                            goto ChangeSettings;
                    }
                    goto Start;
                case 2:
                    Filename:
                    Console.Clear();
                    Console.WriteLine("Please enter the filename you would like the document to be saved as.");
                    string enteredName = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("The filename you entered was: " + enteredName);
                    Console.WriteLine("Your current filename is: " + ConfigurationManager.AppSettings.Get("Name"));
                    Console.WriteLine("Are you sure? [Y/N]");
                    confirmSetting = Console.ReadLine();
                    switch (confirmSetting)
                    {
                        case "Y":
                            config.AppSettings.Settings["Name"].Value = enteredName;
                            config.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                            goto ChangeSettings;
                        case "y":
                            config.AppSettings.Settings["Name"].Value = enteredName;
                            config.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                            goto ChangeSettings;
                        case "N":
                            Console.Clear();
                            Console.WriteLine("Nothing changed. Press enter to continue.");
                            goto ChangeSettings;
                        case "n":
                            Console.Clear();
                            Console.WriteLine("Nothing changed. Press enter to continue.");
                            goto ChangeSettings;
                        default:
                            goto Start;
                    }
                case 3:
                    Console.Clear();
                    goto Start;
                default:
                    System.Environment.Exit(1);
                    break;
            }
            

        }
    }
}
