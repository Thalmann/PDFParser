using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKEAPDFPArser
{
    public class CalendarEvent
    {
        public CalendarEvent(EventType title, DateTime start, DateTime end)
        {
            this.title = title;
            this.start = start;
            this.end = end;
        }

        private EventType title;

        public EventType Title
        {
            get { return title; }
            set { title = value; }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private DateTime start;

        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }
        
        private DateTime end;

        public DateTime End
        {
            get { return end; }
            set { end = value; }
        }
    }
}
