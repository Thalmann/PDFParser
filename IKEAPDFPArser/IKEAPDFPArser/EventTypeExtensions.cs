using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEAPDFPArser
{
    public enum EventType
    {
        Standard,
        Holiday,
        Meeting
    }
    public static class EventTypeExtensions
    {
        public static string ToString(this EventType e)
        {
            switch (e)
            {
                case EventType.Standard: return "Arbejde IKEA";
                case EventType.Holiday: return "Arbejde IKEA - Feriedag";
                case EventType.Meeting: return "Arbejde IKEA - Møde";
                default: throw new ArgumentException("Unknown enum of type: EventType.");
            }
        }
    }
}
