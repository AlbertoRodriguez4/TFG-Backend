using AA2_CS.Database;
using AA2_CS.Model;
using Microsoft.EntityFrameworkCore;

namespace AA2_CS.Repository
{
    public class SubscriptionRepository
    {
        private readonly AppDbContext _context;

        public SubscriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Añade una nueva suscripción (simulación de pago de 10€)
        /// </summary>
        public int Add(Subscription subscription)
        {
            var user = _context.Users.FirstOrDefault(u => u.id == subscription.userid);

            if (user == null)
                throw new ArgumentException("User not found.");

            // Simulación de pago - no se descuenta oro del juego
            // En producción aquí iría la integración con pasarela de pago (Stripe, PayPal, etc.)

            subscription.startDate = subscription.startDate.ToUniversalTime();
            subscription.endDate = subscription.endDate.ToUniversalTime();

            _context.Subscriptions.Add(subscription);
            _context.SaveChanges();

            return subscription.id;
        }

        /// <summary>
        /// Obtiene todas las suscripciones activas
        /// </summary>
        public List<Subscription> FindAllActive()
        {
            return _context.Subscriptions
                .Where(s => s.isActive && s.endDate > DateTime.UtcNow)
                .ToList();
        }

        /// <summary>
        /// Obtiene todas las suscripciones (incluyendo inactivas)
        /// </summary>
        public List<Subscription> FindAll()
        {
            return _context.Subscriptions.ToList();
        }

        /// <summary>
        /// Obtiene una suscripción por ID
        /// </summary>
        public Subscription? FindById(int id)
        {
            return _context.Subscriptions.FirstOrDefault(s => s.id == id);
        }

        /// <summary>
        /// Obtiene la suscripción activa de un usuario
        /// </summary>
        public Subscription? FindByUserId(int userId)
        {
            return _context.Subscriptions
                .FirstOrDefault(s => s.userid == userId && s.isActive && s.endDate > DateTime.UtcNow);
        }

        /// <summary>
        /// Obtiene todas las suscripciones de un usuario (histórico)
        /// </summary>
        public List<SubscriptionDTO> FindHistoryByUserId(int userId)
        {
            var subscriptions = (from s in _context.Subscriptions
                                 join user in _context.Users on s.userid equals user.id
                                 where s.userid == userId
                                 orderby s.startDate descending
                                 select new SubscriptionDTO
                                 {
                                     SubscriptionId = s.id,
                                     UserId = s.userid,
                                     UserName = user.name,
                                     UserEmail = user.email,
                                     StartDate = s.startDate,
                                     EndDate = s.endDate,
                                     IsActive = s.isActive,
                                     PlanType = s.planType,
                                     MonthlyPrice = s.monthlyPrice,
                                     NextRenewalDate = s.isActive && s.endDate > DateTime.UtcNow ? s.endDate : null
                                 }).ToList();

            return subscriptions;
        }

        /// <summary>
        /// Verifica si un usuario tiene una suscripción activa
        /// </summary>
        public bool HasActiveSubscription(int userId)
        {
            return _context.Subscriptions
                .Any(s => s.userid == userId && s.isActive && s.endDate > DateTime.UtcNow);
        }

        /// <summary>
        /// Renueva una suscripción existente (extiende la fecha de fin)
        /// </summary>
        public int RenewSubscription(int userId, int months = 1)
        {
            var subscription = FindByUserId(userId);

            if (subscription == null)
                throw new InvalidOperationException("No hay una suscripción activa para renovar.");

            // Simulación de pago - no se descuenta oro del juego
            // En producción aquí iría la integración con pasarela de pago (Stripe, PayPal, etc.)

            // Extender la fecha de fin
            subscription.endDate = subscription.endDate.AddMonths(months);
            subscription.isActive = true;

            _context.Subscriptions.Update(subscription);
            _context.SaveChanges();

            return subscription.id;
        }

        /// <summary>
        /// Cancela una suscripción (marca como inactiva)
        /// </summary>
        public int CancelSubscription(int subscriptionId)
        {
            var subscription = FindById(subscriptionId);
            if (subscription == null)
                return 0;

            subscription.isActive = false;
            _context.Subscriptions.Update(subscription);
            _context.SaveChanges();

            return 1;
        }

        /// <summary>
        /// Actualiza el estado de las suscripciones expiradas
        /// </summary>
        public int DeactivateExpiredSubscriptions()
        {
            var expiredSubscriptions = _context.Subscriptions
                .Where(s => s.isActive && s.endDate <= DateTime.UtcNow)
                .ToList();

            foreach (var subscription in expiredSubscriptions)
            {
                subscription.isActive = false;
            }

            if (expiredSubscriptions.Count > 0)
            {
                _context.Subscriptions.UpdateRange(expiredSubscriptions);
                _context.SaveChanges();
            }

            return expiredSubscriptions.Count;
        }

        /// <summary>
        /// Obtiene el DTO de la suscripción activa de un usuario
        /// </summary>
        public SubscriptionDTO? GetActiveSubscriptionDTO(int userId)
        {
            var subscription = (from s in _context.Subscriptions
                                join user in _context.Users on s.userid equals user.id
                                where s.userid == userId && s.isActive && s.endDate > DateTime.UtcNow
                                select new SubscriptionDTO
                                {
                                    SubscriptionId = s.id,
                                    UserId = s.userid,
                                    UserName = user.name,
                                    UserEmail = user.email,
                                    StartDate = s.startDate,
                                    EndDate = s.endDate,
                                    IsActive = s.isActive,
                                    PlanType = s.planType,
                                    MonthlyPrice = s.monthlyPrice,
                                    NextRenewalDate = s.endDate
                                }).FirstOrDefault();

            return subscription;
        }
    }
}
