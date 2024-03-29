using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;
using System.Collections.ObjectModel;
using System.ServiceModel.Syndication;

namespace physicsEvents
{
    public class Events
    {
        private string title;
        public string summary;
        private string speaker;
        private string location;
        private DateTime date;
        private string startTime;
        private string endTime;
        private Uri uri;
        public string Title
        // {
        //     get { return title; }
        //     private set { title = value; }
        // }
        {
            get { return title; }
            set
            {
                if (title == value)
                    return;
                title = value;
            }
        }
        public string Summary
        {
            get { return summary; }
            set
            {
                if (summary == value)
                    return;
                summary = value;
            }
        }
        public string Speaker
        // {
        //     get { return speaker; }
        //     private set { speaker = value; }
        // }
        {
            get { return speaker; }
            set
            {
                if (speaker == value)
                    return;
                speaker = value;
            }
        }
        public string Location
        // {
        //     get { return location; }
        //     private set { location = value; }
        // }
        {
            get { return location; }
            set
            {
                if (location == value)
                    return;
                location = value;
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date == value)
                    return;
                date = value;
            }
        }

        public string StartTime
        // {
        //     get { return time; }
        //     private set { time = value; }
        // }
        {
            get { return startTime; }
            set
            {
                if (startTime == value)
                    return;
                startTime = value;
            }
        }
        public string EndTime
        // {
        //     get { return duration; }
        //     private set { duration = value; }
        // }
        {
            get { return endTime; }
            set
            {
                if (endTime == value)
                    return;
                endTime = value;
            }
        }
        public Uri Uri
        // {
        //     get { return uri; }
        //     private set { uri = value; }
        // }
        {
            get { return uri; }
            set
            {
                if (uri == value)
                    return;
                uri = value;
            }
        }
    }
}
