using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IKEAPDFPArser;

namespace IKEAPDFPArser
{
    public class Parser
    {
        private string stringPattern =  @"\d\d:\d\d\s-\s\d\d:\d\d\s\d\d:\d\d\s\d\d:\d\d\s\d\d-\d\d";
        public Parser(){}

        public CalendarEvent[] Parse(string text)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            string[] lines = splitToDays(text);
            lines = findWorkingDays(lines);
            //var e = getEventTime(Regex.Match(lines[0], stringPattern).ToString());
            events = getEvents(lines);
            return events.ToArray();
        }

        private string[] splitToDays(string s)
        {
            return Regex.Split(s.ToString(), "(?=Mandag)|(?=Tirsdag)|(?=Onsdag)|(?=Torsdag)|(?=Fredag)|(?=Lørdag)|(?=Søndag)"); 
        }

        private string[] findWorkingDays(string[] days)
        {
            List<string> workingDays = new List<string>();
            foreach (string line in days)
            {
                if (Regex.IsMatch(line, stringPattern))
                    workingDays.Add(line);
            }
            return workingDays.ToArray();
        }

        private Tuple<DateTime,DateTime> getEventTime(string s)
        {
            int month = Int16.Parse(s[29].ToString() + s[30]);
            int day =  Int16.Parse(s[26].ToString() + s[27]);
            DateTime start = new DateTime(2015, month, day, Int16.Parse(s[0].ToString() + s[1]), Int16.Parse(s[3].ToString() + s[4]), 0);
            DateTime end = new DateTime(2015, month, day, Int16.Parse(s[8].ToString() + s[9]), Int16.Parse(s[11].ToString() + s[12]), 0);

            return Tuple.Create<DateTime, DateTime>(start, end);
        }

        private List<CalendarEvent> getEvents(string[] workingDays)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            foreach (string s in workingDays)
            {
                var eventTimes = getEventTime(Regex.Match(s, stringPattern).ToString());
                events.Add(new CalendarEvent(getEventType(s), eventTimes.Item1, eventTimes.Item2));
            }
            return events;
        }

        private EventType getEventType(string workingDay)
        {
            if (workingDay.Contains("Feriedag"))
                return EventType.Holiday;
            else if (workingDay.ToLower().Contains("møde"))
                return EventType.Meeting;
            return EventType.Standard;
        }

    }

}
