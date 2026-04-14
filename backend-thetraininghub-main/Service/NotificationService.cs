using AA2_CS.Repository;

namespace AA2_CS.Service
{
    public class NotificationService
    {
        private readonly EmailService _emailService;
        private readonly NotificationPreferenceRepository _prefRepo;
        private readonly UserRepository _userRepo;
        private readonly TasksRepository _taskRepo;
        private readonly SubscriptionRepository _subscriptionRepo;

        public NotificationService(
            EmailService emailService,
            NotificationPreferenceRepository prefRepo,
            UserRepository userRepo,
            TasksRepository taskRepo,
            SubscriptionRepository subscriptionRepo)
        {
            _emailService = emailService;
            _prefRepo = prefRepo;
            _userRepo = userRepo;
            _taskRepo = taskRepo;
            _subscriptionRepo = subscriptionRepo;
        }

        /// <summary>
        /// Envía un recordatorio de inactividad si el usuario lo tiene habilitado
        /// y no ha completado una tarea en los días configurados.
        /// </summary>
        public async System.Threading.Tasks.Task SendInactivityReminderIfNeeded(int userId)
        {
            var pref = await _prefRepo.GetOrCreateAsync(userId);
            if (!pref.inactivityEnabled) return;

            var lastTaskDate = _taskRepo.GetLastCompletedTaskDate(userId);
            if (lastTaskDate == null) return; // Usuario nuevo, sin tareas completadas

            var daysSinceLastTask = (DateTime.UtcNow - lastTaskDate.Value).Days;
            if (daysSinceLastTask < pref.inactivityDays) return;

            var user = _userRepo.FindById(userId);
            if (user == null || string.IsNullOrEmpty(user.email)) return;

            await _emailService.SendInactivityReminderEmail(user.email, user.name, daysSinceLastTask);
        }

        /// <summary>
        /// Envía una notificación de actividad de sala si el usuario lo tiene habilitado.
        /// </summary>
        public async System.Threading.Tasks.Task SendRoomActivityNotificationIfNeeded(int userId, string roomName, string action)
        {
            var pref = await _prefRepo.GetOrCreateAsync(userId);
            if (!pref.roomsEnabled) return;

            var user = _userRepo.FindById(userId);
            if (user == null || string.IsNullOrEmpty(user.email)) return;

            await _emailService.SendRoomActivityEmail(user.email, user.name, roomName, action);
        }

        /// <summary>
        /// Envía un recibo de compra si el usuario lo tiene habilitado.
        /// </summary>
        public async System.Threading.Tasks.Task SendPurchaseReceiptIfNeeded(int userId, string itemName, string itemType, int itemBonus, int itemPrice)
        {
            var pref = await _prefRepo.GetOrCreateAsync(userId);
            if (!pref.purchasesEnabled) return;

            var user = _userRepo.FindById(userId);
            if (user == null || string.IsNullOrEmpty(user.email)) return;

            await _emailService.SendPurchaseReceiptEmail(
                user.email, user.name, itemName, itemType, itemBonus, itemPrice, DateTime.UtcNow);
        }

        /// <summary>
        /// Envía una alerta de expiración de suscripción si el usuario lo tiene habilitado
        /// y su suscripción activa expira en 3 días o menos.
        /// </summary>
        public async System.Threading.Tasks.Task SendSubscriptionExpiryWarningIfNeeded(int userId)
        {
            var pref = await _prefRepo.GetOrCreateAsync(userId);
            if (!pref.subscriptionExpiryEnabled) return;

            var subscription = _subscriptionRepo.FindByUserId(userId);
            if (subscription == null) return;

            var daysLeft = (subscription.endDate - DateTime.UtcNow).Days;
            if (daysLeft > 3 || daysLeft < 0) return;

            var user = _userRepo.FindById(userId);
            if (user == null || string.IsNullOrEmpty(user.email)) return;

            await _emailService.SendSubscriptionExpiryEmail(user.email, user.name, subscription.endDate, daysLeft);
        }
    }
}