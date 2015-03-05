using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IKEAPDFPArser
{
    public class Parser
    {
        public Parser(){}

        public string[] Parse(string text)
        {
            string[] lines = splitToDays(text);
            lines = findWorkingDays(lines);
            return lines;
        }

        private string[] splitToDays(string s)
        {
            return Regex.Split(s.ToString(), "(?=Mandag)|(?=Tirsdag)|(?=Onsdag)|(?=Torsdag)|(?=Fredag)|(?=Lørdag)|(?=Søndag)"); 
        }

        private string[] findWorkingDays(string[] days)
        {
            List<string> workingDays = new List<string>();
            string pattern = @"\d\d:\d\d\s-\s\d\d:\d\d\s\d\d:\d\d\s\d\d:\d\d\s\d\d-\d\d";
            foreach (string line in days)
            {
                if (Regex.IsMatch(line, pattern))
                    workingDays.Add(line);
            }
            return workingDays.ToArray();
        }

    }

}
