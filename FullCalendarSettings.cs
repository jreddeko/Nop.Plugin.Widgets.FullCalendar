
using Nop.Core.Configuration;
using System;

namespace Nop.Plugin.Widgets.FullCalendar
{
    public class FullCalendarSettings : ISettings
    {
        public string ClassName { get; set; }
        public string PublicApiKey { get; internal set; }
        public string CalendarId { get; internal set; }
        public string DaysOfWeekEnabled { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}