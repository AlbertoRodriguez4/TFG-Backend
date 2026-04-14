using AA2_CS.Repository;

namespace AA2_CS.Service
{
    public class NotificationBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationBackgroundService> _logger;

        public NotificationBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<NotificationBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async System.Threading.Tasks.Task ExecuteAsync(System.Threading.CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotificationBackgroundService iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                // Calcular el tiempo hasta las 8:00 UTC del próximo día
                var now = DateTime.UtcNow;
                var nextRun = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0, DateTimeKind.Utc);
                if (nextRun <= now)
                {
                    nextRun = nextRun.AddDays(1);
                }

                var delay = nextRun - now;
                _logger.LogInformation("Próxima ejecución de notificaciones a las {NextRun} UTC (en {Delay})", nextRun, delay);

                await System.Threading.Tasks.Task.Delay(delay, stoppingToken);

                if (stoppingToken.IsCancellationRequested) break;

                _logger.LogInformation("Ejecutando comprobaciones de notificaciones...");

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var prefRepo = scope.ServiceProvider.GetRequiredService<NotificationPreferenceRepository>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();

                    // Comprobación de inactividad
                    var inactivePrefs = prefRepo.GetAllWithInactivityEnabled();
                    _logger.LogInformation("Comprobando inactividad para {Count} usuarios", inactivePrefs.Count);

                    foreach (var pref in inactivePrefs)
                    {
                        try
                        {
                            await notificationService.SendInactivityReminderIfNeeded(pref.userid);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error al enviar recordatorio de inactividad al usuario {UserId}", pref.userid);
                        }
                    }

                    // Comprobación de expiración de suscripción
                    var subPrefs = prefRepo.GetAllWithSubscriptionExpiryEnabled();
                    _logger.LogInformation("Comprobando expiración de suscripción para {Count} usuarios", subPrefs.Count);

                    foreach (var pref in subPrefs)
                    {
                        try
                        {
                            await notificationService.SendSubscriptionExpiryWarningIfNeeded(pref.userid);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error al enviar alerta de expiración al usuario {UserId}", pref.userid);
                        }
                    }

                    _logger.LogInformation("Comprobaciones de notificaciones completadas.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error general en NotificationBackgroundService");
                }
            }
        }
    }
}