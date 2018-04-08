using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.FullCalendar.Domain
{
    public class Appointment : BaseEntity
    {
        public DateTime AppointmentDateTimeUTC { get; set; }
        public string AppointmentReason { get; set; }
        public string ContactNumber { get; set; }
        public string ContactName { get; set; }
        public DateTime CreatedOnUTC { get; internal set; }
    }
}
