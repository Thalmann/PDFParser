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
        private string stringPatternNoBreak = @"\d\d:\d\d\s-\s\d\d:\d\d\s\d\d:\d\d\s\d\d-\d\d";
        private Tuple<int,int> yearFromTo;
        public Parser(){}

        public CalendarEvent[] Parse(string text)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            string[] lines = splitToDays(text);
            yearFromTo = getEventYear(lines[0]);
            lines = findWorkingDays(lines);
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
                if (Regex.IsMatch(line, stringPattern) || Regex.IsMatch(line, stringPatternNoBreak))
                    workingDays.Add(line);
            }
            return workingDays.ToArray();
        }

        private Tuple<DateTime,DateTime> getEventTime(string s)
        {
            int month = Int16.Parse(s[29].ToString() + s[30]); // Dør her pga no break string pattern - DER SKAL tages højde for at man kan have en vagt uden pause!
            int day =  Int16.Parse(s[26].ToString() + s[27]);
            int year = yearFromTo.Item1;

            bool switched = false;
            if (month == 1 && !switched)
            {
                year = yearFromTo.Item2;
                switched = true;
            }

            DateTime start = new DateTime(year, month, day, Int16.Parse(s[0].ToString() + s[1]), Int16.Parse(s[3].ToString() + s[4]), 0);
            DateTime end = new DateTime(year, month, day, Int16.Parse(s[8].ToString() + s[9]), Int16.Parse(s[11].ToString() + s[12]), 0);

            return Tuple.Create<DateTime, DateTime>(start, end);
        }

        private Tuple<int,int> getEventYear(string s)
        {
            int from=0, to=0;
            Boolean fromYear = true;
            foreach (string item in s.Split(new char[]{'-', '\n'}))
            {
                if (Regex.IsMatch(item.Replace(" ", String.Empty), @"\d\d\d\d"))
                {
                    if (fromYear)
                    {
                        from = Int16.Parse(item);
                        fromYear = false;
                    }
                    else if (!fromYear)
                    {
                        to = Int16.Parse(item);
                        fromYear = true;
                    }
                }
            }
            if (from == 0 || to == 0)
                throw new ArgumentNullException();
            return Tuple.Create<int,int>(from,to);
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
