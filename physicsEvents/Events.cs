using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;

namespace physicsEvents
{
    public class Events
    {
        public string Title { get; set; }
        public string Speaker { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Time { get; set; }
        public int Duration { get; set; }
        public Uri Uri { get; set; }
    }
}
