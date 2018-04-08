using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Plugin.Widgets.FullCalendar.Models;
using System.Collections.Generic;
using Nop.Plugin.Widgets.FullCalendar.Services;
using Nop.Web.Framework.Kendoui;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Plugin.Widgets.FullCalendar.Domain;
using Newtonsoft.Json;

namespace Nop.Plugin.Widgets.FullCalendar.Controllers
{
    public class WidgetsFullCalendarController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly IOrderService _orderService;
        private readonly ILogger _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly ILocalizationService _localizationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IAppointmentService _appointmentService;
        private readonly IDateTimeHelper _dateTimeHelper;

        public WidgetsFullCalendarController(IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService,
            ISettingService settingService,
            IOrderService orderService,
            ILogger logger,
            ICategoryService categoryService,
            IProductAttributeParser productAttributeParser,
            ILocalizationService localizationService,
            IGenericAttributeService genericAttributeService,
            IAppointmentService appointmentService,
            IDateTimeHelper datetimeHelper)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._orderService = orderService;
            this._logger = logger;
            this._categoryService = categoryService;
            this._productAttributeParser = productAttributeParser;
            this._localizationService = localizationService;
            this._genericAttributeService = genericAttributeService;
            this._appointmentService = appointmentService;
            this._dateTimeHelper = datetimeHelper;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var fullCalendarSettings = _settingService.LoadSetting<FullCalendarSettings>(storeScope);
            var model = new ConfigurationModel();
            model.PublicApiKey = fullCalendarSettings.PublicApiKey;
            model.CalendarId = fullCalendarSettings.CalendarId;
            model.ClassName = fullCalendarSettings.ClassName;
            model.DaysOfWeekEnabled = String.IsNullOrEmpty(fullCalendarSettings.DaysOfWeekEnabled) ? new int[0] : JsonConvert.DeserializeObject<int[]>(fullCalendarSettings.DaysOfWeekEnabled);
            model.StartTime = fullCalendarSettings.StartTime;
            model.EndTime = fullCalendarSettings.EndTime;

            return View("~/Plugins/Widgets.FullCalendar/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var fullCalendarSettings = _settingService.LoadSetting<FullCalendarSettings>(storeScope);
            fullCalendarSettings.PublicApiKey = model.PublicApiKey;
            fullCalendarSettings.CalendarId = model.CalendarId;
            fullCalendarSettings.ClassName = model.ClassName;
            fullCalendarSettings.StartTime = model.StartTime;
            fullCalendarSettings.EndTime = model.EndTime;
            fullCalendarSettings.DaysOfWeekEnabled = JsonConvert.SerializeObject(model.DaysOfWeekEnabled);

            _settingService.SaveSetting(fullCalendarSettings);

            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone, object additionalData = null)
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var fullCalendarSettings = _settingService.LoadSetting<FullCalendarSettings>(storeScope);
            var currentCustomer = _workContext.CurrentCustomer;

            var model = new AppointmentModel
            {
                PublicApiKey = fullCalendarSettings.PublicApiKey,
                CalendarId = fullCalendarSettings.CalendarId,
                ClassName = fullCalendarSettings.ClassName,
                AppointmentDate = DateTime.Now,
                ContactName = currentCustomer.GetFullName(),
                WidgetZone = widgetZone,
                StartTime = fullCalendarSettings.StartTime,
                EndTime = fullCalendarSettings.EndTime,
                DaysOfWeekEnabled = String.IsNullOrEmpty(fullCalendarSettings.DaysOfWeekEnabled) ? new int[0] : JsonConvert.DeserializeObject<int[]>(fullCalendarSettings.DaysOfWeekEnabled)
            };
            return View("~/Plugins/Widgets.FullCalendar/Views/PublicInfo.cshtml", model);
        }

        [HttpPost]
        public ActionResult PublicInfo(AppointmentModel model)
        {
            if (ModelState.IsValid) // check form inputs
            {
                try
                {
                    var appointment = new Appointment
                    {
                        AppointmentDateTimeUTC = _dateTimeHelper.ConvertToUtcTime(model.AppointmentDate),
                        AppointmentReason = model.AppointmentReason,
                        ContactName = model.ContactName,
                        ContactNumber = model.ContactNumber,
                        CreatedOnUTC = DateTime.UtcNow,
                    };

                    // create new appointment
                    _appointmentService.InsertAppointment(appointment);

                    // log result for debugging
                    var logMessage = String.Format("New appointment created on {0} for {1}", model.AppointmentDate, model.ContactNumber);
                    _logger.Debug(logMessage, null, _workContext.CurrentCustomer);

                    // show success notification
                    SuccessNotification(_localizationService.GetResource("Plugins.Widgets.FullCalendar.Form.Success"));

                    // return
                    return PublicInfo(model.WidgetZone);
                }
                catch (Exception ex)
                {
                    // log error message
                    _logger.Error("Error creating appointment request.", ex, _workContext.CurrentCustomer);

                    // show error notification
                    ErrorNotification(_localizationService.GetResource("Plugins.Widgets.FullCalendar.Form.Error"));

                    // return
                    return View("~/Plugins/Widgets.FullCalendar/Views/PublicInfo.cshtml", model);
                }
            }

            return View("~/Plugins/Widgets.FullCalendar/Views/PublicInfo.cshtml", model);
        }

        [HttpPost]
        public virtual ActionResult List(DataSourceRequest command)
        {
            var appointments = _appointmentService.GetAllAppointments(true);
            var gridModel = new DataSourceResult
            {
                Data = appointments.Select(x => new AppointmentModel
                {
                    AppointmentDate = _dateTimeHelper.ConvertToUserTime(x.AppointmentDateTimeUTC, DateTimeKind.Utc),
                    AppointmentReason = x.AppointmentReason,
                    ContactName = x.ContactName,
                    ContactNumber = x.ContactNumber,
                    Created = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUTC, DateTimeKind.Utc),
                }),
                Total = appointments.Count()
            };

            return Json(gridModel);
        }
    }
}