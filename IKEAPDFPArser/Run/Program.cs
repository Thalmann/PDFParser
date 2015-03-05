using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEAPDFWorkingScheduleParser;

namespace Run
{
    class Program
    {
        

        static void Main(string[] args)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            TextToEvent textToEvent = new TextToEvent();
            PDFParser pdfToText = new PDFParser();
            iCal iCalParser = new iCal();

            string[] text = pdfToText.GetStringArray("bruno.pdf");
            events = textToEvent.TextToEvents(text);
            byte[] ics = iCalParser.ICalSerializeToBytes(iCalParser.CreateICalendar(events), "hej");
            File.WriteAllBytes("output.txt",ics);
        }
    }
}
