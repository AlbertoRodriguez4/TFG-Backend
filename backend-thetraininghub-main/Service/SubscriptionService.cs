using AA2_CS.Model;
using AA2_CS.Repository;

namespace AA2_CS.Service
{
    public class SubscriptionService
    {
        private readonly SubscriptionRepository _repository;

        public SubscriptionService(SubscriptionRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Compra una nueva suscripción premium mensual (simulación de pago de 10€)
        /// </summary>
        public int PurchaseSubscription(int userId)
        {
            // Verificar si ya tiene una suscripción activa
            var existingSubscription = _repository.FindByUserId(userId);

            if (existingSubscription != null)
            {
                throw new InvalidOperationException("El usuario ya tiene una suscripción activa.");
            }

            var subscription = new Subscription
            {
                userid = userId,
                startDate = DateTime.UtcNow,
                endDate = DateTime.UtcNow.AddMonths(1), // Suscripción mensual
                planType = "Premium",
                monthlyPrice = 10.00m, // 10€ mensuales (simulación)
                isActive = true
            };

            return _repository.Add(subscription);
        }

        /// <summary>
        /// Renueva una suscripción existente por un mes más
        /// </summary>
        public int RenewSubscription(int userId)
        {
            return _repository.RenewSubscription(userId, months: 1);
        }

        /// <summary>
        /// Cancela una suscripción
        /// </summary>
        public int CancelSubscription(int subscriptionId)
        {
            return _repository.CancelSubscription(subscriptionId);
        }

        /// <summary>
        /// Obtiene la suscripción activa de un usuario
        /// </summary>
        public SubscriptionDTO? GetActiveSubscription(int userId)
        {
            return _repository.GetActiveSubscriptionDTO(userId);
        }

        /// <summary>
        /// Verifica si un usuario tiene suscripción activa
        /// </summary>
        public bool HasActiveSubscription(int userId)
        {
            return _repository.HasActiveSubscription(userId);
        }

        /// <summary>
        /// Obtiene el historial de suscripciones de un usuario
        /// </summary>
        public List<SubscriptionDTO> GetSubscriptionHistory(int userId)
        {
            return _repository.FindHistoryByUserId(userId);
        }

        /// <summary>
        /// Obtiene todas las suscripciones activas del sistema
        /// </summary>
        public List<Subscription> GetAllActiveSubscriptions()
        {
            return _repository.FindAllActive();
        }

        /// <summary>
        /// Desactiva suscripciones expiradas (tarea de mantenimiento)
        /// </summary>
        public int DeactivateExpiredSubscriptions()
        {
            return _repository.DeactivateExpiredSubscriptions();
        }

        /// <summary>
        /// Obtiene una suscripción por ID
        /// </summary>
        public Subscription? GetById(int id)
        {
            return _repository.FindById(id);
        }

        /// <summary>
        /// Obtiene todas las suscripciones (admin)
        /// </summary>
        public List<Subscription> GetAll()
        {
            return _repository.FindAll();
        }
    }
}
