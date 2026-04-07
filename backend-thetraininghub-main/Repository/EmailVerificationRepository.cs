using AA2_CS.Database;
using AA2_CS.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AA2_CS.Repository
{
    public class EmailVerificationRepository
    {
        private readonly AppDbContext _context;

        public EmailVerificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task<EmailVerification> AddAsync(EmailVerification verification)
        {
            await _context.EmailVerifications.AddAsync(verification);
            await _context.SaveChangesAsync();
            return verification;
        }

        public async System.Threading.Tasks.Task<EmailVerification?> GetValidVerificationAsync(int userId, string code)
        {
            var verification = await _context.EmailVerifications
                .FirstOrDefaultAsync(v =>
                    v.userid == userId &&
                    v.code == code &&
                    v.isused == false &&
                    v.expiresat > DateTime.UtcNow);

            return verification;
        }

        public async System.Threading.Tasks.Task MarkAsUsedAsync(EmailVerification verification)
        {
            verification.isused = true;
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteExpiredAsync(int userId)
        {
            var expired = await _context.EmailVerifications
                .Where(v => v.userid == userId && v.expiresat <= DateTime.UtcNow)
                .ToListAsync();

            if (expired.Any())
            {
                _context.EmailVerifications.RemoveRange(expired);
                await _context.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task InvalidatePreviousAsync(int userId)
        {
            var previousVerifications = await _context.EmailVerifications
                .Where(v => v.userid == userId && v.isused == false)
                .ToListAsync();

            foreach (var verification in previousVerifications)
            {
                verification.isused = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
