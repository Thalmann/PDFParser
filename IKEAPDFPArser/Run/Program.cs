using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEAPDFPArser;

namespace Run
{
    class Program
    {
        

        static void Main(string[] args)
        {
            PDFParser pdfToText = new PDFParser();
            iCal iCalParser = new iCal();
            Parser p = new Parser();

            CalendarEvent[] events = p.Parse(pdfToText.GetString("bruno.pdf"));
            byte[] ics = iCalParser.ICalSerializeToBytes(iCalParser.CreateICalendar(events), "hej");
            File.WriteAllBytes("output.txt", ics);
        }
    }
}
