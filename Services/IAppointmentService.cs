using Nop.Plugin.Widgets.FullCalendar.Domain;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.FullCalendar.Services
{
    public interface IAppointmentService
    {
        void InsertAppointment(Appointment appointment);

        /// <summary>
        /// Gets all appointments
        /// </summary>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Languages</returns>
        IList<Appointment> GetAllAppointments(bool showHidden = false, int storeId = 0);
    }
}