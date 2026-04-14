using AA2_CS.Model;
using AA2_CS.Repository;

namespace AA2_CS.Service
{
    public class NotificationPreferenceService
    {
        private readonly NotificationPreferenceRepository _prefRepo;

        public NotificationPreferenceService(NotificationPreferenceRepository prefRepo)
        {
            _prefRepo = prefRepo;
        }

        public async System.Threading.Tasks.Task<NotificationPreferenceDTO> GetByUserIdAsync(int userId)
        {
            var pref = await _prefRepo.GetOrCreateAsync(userId);
            return MapToDTO(pref);
        }

        public async System.Threading.Tasks.Task<NotificationPreferenceDTO> UpdateAsync(int userId, NotificationPreferenceDTO dto)
        {
            if (dto.InactivityDays < 1) dto.InactivityDays = 1;
            if (dto.InactivityDays > 30) dto.InactivityDays = 30;

            var pref = await _prefRepo.GetOrCreateAsync(userId);
            pref.inactivityEnabled = dto.InactivityEnabled;
            pref.inactivityDays = dto.InactivityDays;
            pref.roomsEnabled = dto.RoomsEnabled;
            pref.purchasesEnabled = dto.PurchasesEnabled;
            pref.subscriptionExpiryEnabled = dto.SubscriptionExpiryEnabled;
            await _prefRepo.UpdateAsync(pref);
            return MapToDTO(pref);
        }

        public async System.Threading.Tasks.Task<NotificationPreferenceDTO> ResetDefaultsAsync(int userId)
        {
            var pref = await _prefRepo.GetOrCreateAsync(userId);
            pref.inactivityEnabled = true;
            pref.inactivityDays = 3;
            pref.roomsEnabled = true;
            pref.purchasesEnabled = true;
            pref.subscriptionExpiryEnabled = true;
            await _prefRepo.UpdateAsync(pref);
            return MapToDTO(pref);
        }

        private NotificationPreferenceDTO MapToDTO(NotificationPreference pref)
        {
            return new NotificationPreferenceDTO
            {
                UserId = pref.userid,
                InactivityEnabled = pref.inactivityEnabled,
                InactivityDays = pref.inactivityDays,
                RoomsEnabled = pref.roomsEnabled,
                PurchasesEnabled = pref.purchasesEnabled,
                SubscriptionExpiryEnabled = pref.subscriptionExpiryEnabled
            };
        }
    }
}