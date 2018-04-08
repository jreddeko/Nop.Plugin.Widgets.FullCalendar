using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.Widgets.FullCalendar.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Config.CalendarId")]
        public string CalendarId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Config.ClassName")]
        public string ClassName { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Config.PublicApiKey")]
        public string PublicApiKey { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Config.DaysOfWeekEnabled")]
        public int[] DaysOfWeekEnabled { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Config.StartTime")]
        public TimeSpan StartTime { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Config.EndTime")]
        public TimeSpan EndTime { get; set; }

    }
}
