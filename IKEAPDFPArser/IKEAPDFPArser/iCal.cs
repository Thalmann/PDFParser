using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDay.Collections;
using DDay.iCal;
using DDay.iCal.Serialization;
using DDay.iCal.Serialization.iCalendar;

namespace IKEAPDFPArser
{
    public class iCal
    {
        public iCal()
        {
        }

        public iCalendar CreateICalendar(CalendarEvent[] calendarEvents)
        {
            iCalendar iCal = new iCalendar();

            foreach (CalendarEvent ce in calendarEvents)
            {
                Event e = new Event();

                e.Summary = ce.Title.ToString();
                e.DTStart = new iCalDateTime(ce.Start);
                e.DTEnd = new iCalDateTime(ce.End);

                iCal.Events.Add(e);
            }

            return iCal;
        }

        public void ICalSerialize(iCalendar iCal, string filename)
        {
            iCalendarSerializer serializer = new iCalendarSerializer(iCal);
            serializer.Serialize(iCal, filename + ".ics");
        }

        public byte[] ICalSerializeToBytes(iCalendar iCal, string filename)
        {
            ISerializationContext ctx = new SerializationContext();
            ISerializerFactory factory = new SerializerFactory();
            IStringSerializer serializer = factory.Build(iCal.GetType(), ctx) as IStringSerializer;

            string output = serializer.SerializeToString(iCal);

            return Encoding.UTF8.GetBytes(output);
        }

    }
}
