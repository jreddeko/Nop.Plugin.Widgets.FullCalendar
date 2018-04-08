using Autofac;
using Autofac.Core;
using Nop.Core.Caching;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Widgets.FullCalendar.Controllers;
using Nop.Plugin.Widgets.FullCalendar.Data;
using Nop.Plugin.Widgets.FullCalendar.Domain;
using Nop.Plugin.Widgets.FullCalendar.Services;
using Nop.Services.Helpers;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.FullCalendar.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            //we cache presentation models between requests
            builder.RegisterType<WidgetsFullCalendarController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"));

            //installation localization service
            builder.RegisterType<AppointmentService>().As<IAppointmentService>().InstancePerLifetimeScope();

            //helpers
            builder.RegisterType<DateTimeHelper>().As<IDateTimeHelper>().InstancePerLifetimeScope();

            //data context
            this.RegisterPluginDataContext<FullCalendarObjectContext>(builder, "nop_object_context_full_calendar");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<Appointment>>()
                .As<IRepository<Appointment>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_full_calendar"))
                .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 2; }
        }
    }
}
