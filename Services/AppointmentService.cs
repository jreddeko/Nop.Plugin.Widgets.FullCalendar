using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Stores;
using Nop.Plugin.Widgets.FullCalendar.Data;
using Nop.Plugin.Widgets.FullCalendar.Domain;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.FullCalendar.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _newAppointmentRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStoreContext _storeContext;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IEmailAccountService _emailAccountService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private IWorkContext _workContext;
        private LocalizationSettings _localizationSettings;

        public AppointmentService(IRepository<Appointment> newAppointmentRepository,
            IEventPublisher eventPublisher, 
            IStoreContext storeContext,
            IMessageTemplateService messageTemplateService, 
            IWorkflowMessageService workflowMessageService,
            IMessageTokenProvider messageTokenProvider,
            IEmailAccountService emailAccountService,
            EmailAccountSettings emailAccountSettings,
            IWorkContext workContext,
            LocalizationSettings localizationSettings)
        {
            _newAppointmentRepository = newAppointmentRepository;
            _eventPublisher = eventPublisher;
            _storeContext = storeContext;
            _messageTemplateService = messageTemplateService;
            _workflowMessageService = workflowMessageService;
            _messageTokenProvider = messageTokenProvider;
            _emailAccountService = emailAccountService;
            _emailAccountSettings = emailAccountSettings;
            _workContext = workContext;
            _localizationSettings = localizationSettings;
        }

        public void InsertAppointment(Appointment appointment)
        {
            _newAppointmentRepository.Insert(appointment);

            //event notification
            _eventPublisher.EntityInserted(appointment);

            var currentCustomer = _workContext.CurrentCustomer;

            //tokens
            var tokens = new List<Token>();
            tokens.Add(new Token("Appointment.ContactNumber", appointment.ContactNumber));
            tokens.Add(new Token("Appointment.Date", appointment.AppointmentDateTimeUTC));
            tokens.Add(new Token("Appointment.Reason", appointment.AppointmentReason));

            SendCustomerAppointmentNotificationMessage(currentCustomer, _localizationSettings.DefaultAdminLanguageId, tokens);
        }

        /// <summary>
        /// Sends 'New customer' notification message to a store owner
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendCustomerAppointmentNotificationMessage(Customer customer, int languageId, IList<Token> tokens)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var store = _storeContext.CurrentStore;

            var messageTemplate = _messageTemplateService.GetMessageTemplateByName("Appointment.New", store.Id);
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

            _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);
            _messageTokenProvider.AddCustomerTokens(tokens, customer);
            

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

            var toEmail = emailAccount.Email;
            var toName = emailAccount.DisplayName;

            return _workflowMessageService.SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
        }

        protected virtual EmailAccount GetEmailAccountOfMessageTemplate(MessageTemplate messageTemplate, int languageId)
        {
            var emailAccountId = messageTemplate.GetLocalized(mt => mt.EmailAccountId, languageId);
            //some 0 validation (for localizable "Email account" dropdownlist which saves 0 if "Standard" value is chosen)
            if (emailAccountId == 0)
                emailAccountId = messageTemplate.EmailAccountId;

            var emailAccount = _emailAccountService.GetEmailAccountById(emailAccountId);
            if (emailAccount == null)
                emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
            if (emailAccount == null)
                emailAccount = _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
            return emailAccount;
        }

        public IList<Appointment> GetAllAppointments(bool showHidden = false, int storeId = 0)
        {
            return _newAppointmentRepository.Table
                .ToList();
        }
    }
}
