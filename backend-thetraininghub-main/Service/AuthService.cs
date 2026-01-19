using System.Security.Claims;
using AA2_CS.JWT;
using AA2_CS.Model;
using AA2_CS.Service;

namespace AA2_CS.Services
{
    public class AuthService
    {
        private readonly UserService _userService;
        private readonly JWTConfigurer _jwtConfigurer;

        public AuthService(UserService userService, JWTConfigurer jwtConfigurer)
        {
            _userService = userService;
            _jwtConfigurer = jwtConfigurer;
        }

        public string Login(string email, string password)
        {
            var user = _userService.Login(email, password);
            if (user == null)
                return null;

            return _jwtConfigurer.GenerateToken(user);
        }

        public string Register(User user)
        {
            _userService.Add(user);
            return _jwtConfigurer.GenerateToken(user);
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
