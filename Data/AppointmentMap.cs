using Nop.Data.Mapping;
using Nop.Plugin.Widgets.FullCalendar.Domain;

namespace Nop.Plugin.Widgets.FullCalendar.Data
{
    public partial class AppointmentMap : NopEntityTypeConfiguration<Appointment>
    {
        public AppointmentMap()
        {
            this.ToTable("Appointment");
            this.HasKey(tr => tr.Id);
        }
    }
}