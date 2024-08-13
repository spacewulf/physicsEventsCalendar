using physicsEventsCalendar;
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


namespace physicsEventsCalendar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string confirmSetting;
            string pathGeneric;
            string eventsUrl = "http://events.umich.edu/group/1965/rss?v=2&html_output=true";

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("FirstLaunch")))
            {
                Methods.GenerateConfigFile();
            }

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (ConfigurationManager.AppSettings.Get("FirstLaunch") == "true")
            {
                FirstLaunch:
                if (ConfigurationManager.AppSettings.Get("Path") != "" && (ConfigurationManager.AppSettings.Get("Name") != "" | ConfigurationManager.AppSettings.Get("DynamicNaming") == "true"))
                {
                    config.AppSettings.Settings["FirstLaunch"].Value = "false";
                    goto Start;
                }
                Console.Clear();
                Console.WriteLine("Welcome to the U-M Department of Physics Events Organizer by Kees Wolterstorff!");
                Console.WriteLine("To get started, you must set your path and filename you'd like to output.");
                Console.WriteLine("You may not proceed until you have chosen a file path and you have either chosen a name for the file or enabled dynamic naming (enabled by default).");
                Console.WriteLine("1: Path of File");
                Console.WriteLine("2: Name of File");
                Console.WriteLine("3: Enable or disable dynamic file naming");
                string firstAnswer = Console.ReadLine();

                if (!Int32.TryParse(firstAnswer, out int intFirstAnswer) | (intFirstAnswer > 3 | intFirstAnswer < 1)) 
                {
                    Console.Clear();
                    Console.WriteLine("That's not a valid selection. Please choose again.");
                    goto FirstLaunch;
                }
                switch (intFirstAnswer)
                {
                    case 1:

                        Console.Clear();
                        Console.WriteLine("Please enter the path you would like the file saved to.");
                        Console.WriteLine(@"Ex: c:\Users\Kees Wolterstorff\Desktop\");
                        string firstEnteredPath = Console.ReadLine();
                        Console.Clear();
                        Console.WriteLine("The path you entered was: " + firstEnteredPath);
                        Console.WriteLine("Are you sure? [Y/N]");
                        string firstConfirmSetting = Console.ReadLine();
                        switch (firstConfirmSetting)
                        {
                            case "Y":
                                config.AppSettings.Settings["Path"].Value = firstEnteredPath;
                                config.Save(ConfigurationSaveMode.Modified);
                                ConfigurationManager.RefreshSection("appSettings");
                                goto FirstLaunch;
                            case "y":
                                config.AppSettings.Settings["Path"].Value = firstEnteredPath;
                                config.Save(ConfigurationSaveMode.Modified);
                                ConfigurationManager.RefreshSection("appSettings");
                                goto FirstLaunch;
                            default:
                                Console.Clear();
                                Console.WriteLine("Nothing changed. Press enter to continue.");
                                goto FirstLaunch;
                        }
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Please enter the filename you would like the document to be saved as.");
                        Console.WriteLine("Note: setting a name here will disable dynamic naming.");
                        string firstEnteredName = Console.ReadLine();
                        Console.Clear();
                        Console.WriteLine("The filename you entered was: " + firstEnteredName);
                        Console.WriteLine("Are you sure? [Y/N]");
                        firstConfirmSetting = Console.ReadLine();
                        switch (firstConfirmSetting)
                        {
                            case "Y":
                                config.AppSettings.Settings["Name"].Value = firstEnteredName;
                                config.AppSettings.Settings["DynamicNaming"].Value = "false";
                                config.Save(ConfigurationSaveMode.Modified);
                                ConfigurationManager.RefreshSection("appSettings");
                                goto FirstLaunch;
                            case "y":
                                config.AppSettings.Settings["Name"].Value = firstEnteredName;
                                config.AppSettings.Settings["DynamicNaming"].Value = "false";
                                config.Save(ConfigurationSaveMode.Modified);
                                ConfigurationManager.RefreshSection("appSettings");
                                goto FirstLaunch;
                            default:
                                Console.Clear();
                                Console.WriteLine("Nothing changed. Press enter to continue.");
                                Console.ReadLine();
                                goto FirstLaunch;
                        }
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Would you like to use dynamic file names?");
                        Console.WriteLine(@"With this setting enabled, it will automatically name the document as: ""Weekly Seminars MM.DD-MM.DD.YY.docx""");
                        Console.WriteLine("Would you like enable or disable it?");
                        Console.WriteLine("1: Enable dynamic naming");
                        Console.WriteLine("2: Disable dynamic naming");
                        string dynamicNaming = Console.ReadLine();
                        switch (dynamicNaming)
                        {
                            case "1":
                                config.AppSettings.Settings["DynamicNaming"].Value = "true";
                                config.Save(ConfigurationSaveMode.Modified);
                                ConfigurationManager.RefreshSection("appSettings");
                                Console.Clear();
                                Console.WriteLine("Dynamic naming enabled. Press enter to continue.");
                                Console.ReadLine();
                                goto FirstLaunch;
                            case "2":
                                config.AppSettings.Settings["DynamicNaming"].Value = "false";
                                config.Save(ConfigurationSaveMode.Modified);
                                ConfigurationManager.RefreshSection("appSettings");
                                Console.WriteLine("Dynamic naming disabled. You must now set a file name in order for the program to work properly. Press enter to continue.");
                                Console.ReadLine();
                                goto FirstLaunch;
                            default:
                                Console.Clear();
                                Console.WriteLine("Nothing changed. Press enter to continue.");
                                Console.ReadLine();
                                goto FirstLaunch;
                        }

                }
            }

        Start:
            Console.Clear();
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Run the program");
            Console.WriteLine("2: Modify Settings");
            Console.WriteLine("3: Quit");
            string choice = Console.ReadLine();
            
            if (!Int32.TryParse(choice, out int value) | value > 3 | value < 1)
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
            }

        StartProgram:
            pathGeneric = ConfigurationManager.AppSettings.Get("Path") + ConfigurationManager.AppSettings.Get("Name") + ".docx";
            Console.Clear();
            Console.WriteLine("The file will be written to: " + pathGeneric);
            Console.WriteLine("Enter the first date: (M/D)");
            string startDate = Console.ReadLine();
            Console.WriteLine("Enter the last date: (M/D)");
            string endDate = Console.ReadLine();


            try
            {
                DateTime StartDateTest = DateTime.Parse(startDate);
                DateTime EndDateTest = DateTime.Parse(endDate);
                if (DateTime.Compare(StartDateTest, EndDateTest) > 0)
                {
                    Console.Clear();
                    Console.WriteLine("The start date must be prior to the end date. Press enter to continue.");
                    Console.ReadLine();
                    goto StartProgram;
                }
            } catch
            {
                Console.WriteLine("That's not a valid choice of a Date. Press enter to continue.");
                Console.ReadLine();
                goto StartProgram;
            }

            DateTime StartDate = DateTime.Parse(startDate);
            DateTime EndDate = DateTime.Parse(endDate);

            if (ConfigurationManager.AppSettings.Get("DynamicNaming") == "true")
            {
                pathGeneric = ConfigurationManager.AppSettings.Get("Path") + "Weekly Seminars " + StartDate.Month.ToString() + "." + StartDate.Day.ToString() + "-" + EndDate.Month.ToString() + "." + EndDate.Day.ToString() + "." + EndDate.Year.ToString().Substring(2) + ".docx";
            }

            Events[] events = Methods.GetEvents(eventsUrl, StartDate, EndDate);

            Console.Clear();
            Console.WriteLine("Successfully generated word document to: " + pathGeneric);
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
                default:
                    System.Environment.Exit(0);
                    break;
            }

            goto Start;

            ChangeSettings:
            Console.Clear();
            Console.WriteLine("Which settings would you like to change?");
            Console.WriteLine("1: Path of File");
            Console.WriteLine("2: Name of File");
            Console.WriteLine("3: Enable or disable dynamic file naming");
            Console.WriteLine("4: Change the color of output hyperlinks");
            Console.WriteLine("5: Return");
            string settingsChoice = Console.ReadLine();

            if (!Int32.TryParse(settingsChoice, out int valueSettings) | valueSettings < 1 | valueSettings > 5)
            {

                Console.Clear();
                Console.WriteLine("That's not a valid choice.");
                Console.ReadLine();
                goto Start;
            }


            switch (valueSettings)
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
                        default:
                            Console.Clear();
                            Console.WriteLine("Nothing changed. Press enter to continue.");
                            goto ChangeSettings;
                    }
                case 2:
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
                        default:
                            Console.Clear();
                            Console.WriteLine("Nothing changed. Press enter to continue.");
                            goto ChangeSettings;
                    }
                case 3:
                    Console.Clear();
                    Console.WriteLine("Would you like to use dynamic file names?");
                    Console.WriteLine(@"With this setting enabled, it will automatically name the document as: ""Weekly Seminars MM.DD-MM.DD.YY.docx""");
                    Console.WriteLine("Would you like enable or disable it?");
                    Console.WriteLine("1: Enable dynamic naming");
                    Console.WriteLine("2: Disable dynamic naming");
                    string dynamicNaming = Console.ReadLine();
                    switch (dynamicNaming)
                    {
                        case "1":
                            config.AppSettings.Settings["DynamicNaming"].Value = "true";
                            config.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                            Console.Clear();
                            Console.WriteLine("Dynamic naming enabled. Press enter to continue.");
                            Console.ReadLine();
                            goto ChangeSettings;
                        case "2":
                            config.AppSettings.Settings["DynamicNaming"].Value = "false";
                            config.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                            Console.WriteLine("Dynamic naming disabled. You must now set a file name in order for the program to work properly. Press enter to continue.");
                            Console.ReadLine();
                            goto ChangeSettings;
                        default:
                            Console.Clear();
                            Console.WriteLine("Nothing changed. Press enter to continue.");
                            Console.ReadLine();
                            goto ChangeSettings;
                    }
                case 4:
                    Console.Clear();
                    Console.WriteLine("What would you like to change the hyperlink text color to?");
                    Console.WriteLine("This requires a Hex code for the color, the default is: 0057E4");
                    string hex = Console.ReadLine();
                    string hexValues = "0123456789AaBbCcDdEe";
                    if (hex.Length != 6)
                    {
                        Console.Clear();
                        Console.WriteLine("That's not a valid choice. Press enter to continue.");
                        Console.ReadLine();
                        goto ChangeSettings;
                    }
                    foreach (char c in hex)
                    {
                        if (!hexValues.Contains(c))
                        {
                            Console.Clear();
                            Console.WriteLine("That's not a valid choice. Press enter to continue.");
                            Console.ReadLine();
                            goto ChangeSettings;
                        }
                    }
                    Console.Clear();
                    Console.WriteLine("The color code you chose was: " + hex);
                    Console.WriteLine("Are you sure? [Y/N]");
                    confirmSetting = Console.ReadLine();
                    switch (confirmSetting)
                    {
                        case "Y":
                            config.AppSettings.Settings["HyperlinkColor"].Value = hex;
                            config.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                            goto ChangeSettings;
                        case "y":
                            config.AppSettings.Settings["HyperlinkColor"].Value = hex;
                            config.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                            goto ChangeSettings;
                        default:
                            Console.Clear();
                            Console.WriteLine("Nothing changed. Press enter to continue.");
                            goto ChangeSettings;
                    }
                default:
                    Console.Clear();
                    goto Start;
            }
        }
    }
}
