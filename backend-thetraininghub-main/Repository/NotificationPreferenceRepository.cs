using AA2_CS.Database;
using AA2_CS.Model;
using Microsoft.EntityFrameworkCore;

namespace AA2_CS.Repository
{
    public class NotificationPreferenceRepository
    {
        private readonly AppDbContext _context;

        public NotificationPreferenceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task<NotificationPreference> AddAsync(NotificationPreference preference)
        {
            _context.NotificationPreferences.Add(preference);
            await _context.SaveChangesAsync();
            return preference;
        }

        public async System.Threading.Tasks.Task<NotificationPreference?> FindByUserIdAsync(int userId)
        {
            return await _context.NotificationPreferences
                .FirstOrDefaultAsync(np => np.userid == userId);
        }

        public async System.Threading.Tasks.Task<NotificationPreference> GetOrCreateAsync(int userId)
        {
            var existing = await FindByUserIdAsync(userId);
            if (existing != null)
                return existing;

            var newPref = new NotificationPreference
            {
                userid = userId,
                inactivityEnabled = true,
                inactivityDays = 3,
                roomsEnabled = true,
                purchasesEnabled = true,
                subscriptionExpiryEnabled = true,
                createdat = DateTime.UtcNow,
                updatedat = DateTime.UtcNow
            };

            return await AddAsync(newPref);
        }

        public async System.Threading.Tasks.Task UpdateAsync(NotificationPreference preference)
        {
            preference.updatedat = DateTime.UtcNow;
            _context.NotificationPreferences.Update(preference);
            await _context.SaveChangesAsync();
        }

        public List<NotificationPreference> GetAllWithInactivityEnabled()
        {
            return _context.NotificationPreferences
                .Where(np => np.inactivityEnabled)
                .ToList();
        }

        public List<NotificationPreference> GetAllWithSubscriptionExpiryEnabled()
        {
            return _context.NotificationPreferences
                .Where(np => np.subscriptionExpiryEnabled)
                .ToList();
        }
    }
}