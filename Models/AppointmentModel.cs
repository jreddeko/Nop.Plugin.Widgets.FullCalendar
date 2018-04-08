using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.FullCalendar.Models
{
    public class AppointmentModel : BaseNopEntityModel
    {
        [Required(ErrorMessage = "Please enter a valid date for your appointment.")]
        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Form.AppointmentDate")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Please enter a reason for your appointment.")]
        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Form.AppointmentReason")]
        public string AppointmentReason { get; set; }

        [Required(ErrorMessage = "Please enter a valid phone number."), MinLength(17, ErrorMessage = "Please enter a valid phone number.")]
        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Form.ContactNumber")]
        public string ContactNumber { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Form.ContactName")]
        public string ContactName { get; set; }
        public int[] DaysOfWeekEnabled { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string WidgetZone { get; set; }
        public string CalendarId { get; set; }
        public string ClassName { get; set; }
        public string PublicApiKey { get; set; }
        public DateTime Created { get; set; }
    }
}
