using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IKEAPDFWorkingScheduleParser;

namespace IKEAPDFPArser
{
    public class Parser
    {
        private string stringPattern =  @"\d\d:\d\d\s-\s\d\d:\d\d\s\d\d:\d\d\s\d\d:\d\d\s\d\d-\d\d";
        public Parser(){}

        public string[] Parse(string text)
        {
            string[] lines = splitToDays(text);
            lines = findWorkingDays(lines);
            var e = getEventTime(Regex.Match(lines[0], stringPattern).ToString());
            return lines;
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

    }

}
