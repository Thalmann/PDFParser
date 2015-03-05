using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IKEAPDFPArser
{
    class Parser
    {
        public void Parse(string text)
        {

        }

        private string[] splitToDays(string s)
        {
            return Regex.Split(s.ToString(), "(?=Mandag)|(?=Tirsdag)|(?=Onsdag)|(?=Torsdag)|(?=Fredag)|(?=Lørdag)|(?=Søndag)"); 
        }
    }

}
