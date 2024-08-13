using Google.Apis.Calendar.v3.Data;
using System.Configuration;


namespace physicsEventsCalendar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string eventsUrl = "http://events.umich.edu/group/1965/rss?v=2&html_output=true";
            string calendarId = "kwolterstorff531@gmail.com";
        Start:
            Console.Clear();
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Run the program");
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
                case 3:
                    System.Environment.Exit(0);
                    break;
                default:
                    return;
            }

        StartProgram:
            Event[] events = Methods.GetEvents(eventsUrl, DateTime.Now, DateTime.Now.AddMonths(9));
            CalendarAccess.InsertEvents(events, calendarId);

            Console.ReadLine();
        }
    }
}
