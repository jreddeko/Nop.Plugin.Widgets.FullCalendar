using Nop.Web.Framework.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nop.Plugin.Widgets.FullCalendar
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Widgets.FullCalendar.List",
                 "Plugins/FullCalendar/List",
                 new { controller = "FullCalendar", action = "List" },
                 new[] { "Nop.Plugin.Widgets.FullCalendar.Controllers" }
            );
        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
