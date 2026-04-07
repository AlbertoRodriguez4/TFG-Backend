using System.Security.Claims;
using AA2_CS.JWT;
using AA2_CS.Model;
using AA2_CS.Service;
using AA2_CS.Repository;

namespace AA2_CS.Services
{
    public class AuthService
    {
        private readonly UserService _userService;
        private readonly JWTConfigurer _jwtConfigurer;
        private readonly EmailService _emailService;
        private readonly EmailVerificationRepository _verificationRepository;

        public AuthService(UserService userService, JWTConfigurer jwtConfigurer, EmailService emailService, EmailVerificationRepository verificationRepository)
        {
            _userService = userService;
            _jwtConfigurer = jwtConfigurer;
            _emailService = emailService;
            _verificationRepository = verificationRepository;
        }

        public string Login(string email, string password)
        {
            var user = _userService.Login(email, password);
            if (user == null)
                return null;

            return _jwtConfigurer.GenerateToken(user);
        }

        public async Task<(User user, string token, bool emailSent)> RegisterAsync(User user)
        {
            // Registrar el usuario
            var registeredUser = _userService.Register(user);
            var token = _jwtConfigurer.GenerateToken(registeredUser);

            // Generar código de verificación aleatorio (6 dígitos)
            var verificationCode = GenerateVerificationCode();

            // Invalidar verificaciones anteriores pendientes
            await _verificationRepository.InvalidatePreviousAsync(registeredUser.id);

            // Crear y guardar la verificación
            var verification = new EmailVerification(registeredUser.id, verificationCode, expirationMinutes: 15);
            await _verificationRepository.AddAsync(verification);

            // Enviar correo de verificación
            bool emailSent = await _emailService.SendVerificationEmail(user.email, user.name, verificationCode);

            return (registeredUser, token, emailSent);
        }

        public async Task<bool> VerifyEmailAsync(int userId, string code)
        {
            var verification = await _verificationRepository.GetValidVerificationAsync(userId, code);

            if (verification == null)
            {
                return false; // Código no válido, expirado o ya usado
            }

            // Marcar como usado
            await _verificationRepository.MarkAsUsedAsync(verification);

            // Actualizar el usuario como verificado
            var user = _userService.FindById(userId);
            if (user != null)
            {
                user.isEmailVerified = true;
                _userService.Update(user);
            }

            return true;
        }

        public async Task<bool> ResendVerificationEmailAsync(int userId)
        {
            var user = _userService.FindById(userId);
            if (user == null || user.isEmailVerified)
            {
                return false;
            }

            // Generar nuevo código
            var verificationCode = GenerateVerificationCode();

            // Invalidar verificaciones anteriores
            await _verificationRepository.InvalidatePreviousAsync(userId);

            // Crear nueva verificación
            var verification = new EmailVerification(userId, verificationCode, expirationMinutes: 15);
            await _verificationRepository.AddAsync(verification);

            // Enviar nuevo correo
            return await _emailService.SendVerificationEmail(user.email, user.name, verificationCode);
        }

        private string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // Código de 6 dígitos
        }
        public bool HasAccessToResource(int requestedUserID, ClaimsPrincipal user) 
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int userId)) 
            { 
                return false; 
            }
            var isOwnResource = userId == requestedUserID;

            var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            var isAdmin = roleClaim!.Value == Roles.userMaster;
            
            var hasAccess = isOwnResource || isAdmin;
            return hasAccess;
        }
    }
}
