using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AA2_CS.Model;
using AA2_CS.Database;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AA2_CS.JWT
{
    public class JWTConfigurer
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context; 

        // CONSTANTES PARA EL NIVEL (Deben ser iguales a las de UserService)
        private const int BASE_XP = 100;
        private const double EXPONENT = 1.5;

        public JWTConfigurer(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        public string GenerateToken(User user)
        {
            // 1. CÁLCULO DE STATS TOTALES (Base + Items)
            var userPurchases = _context.Purchases
                .Include(p => p.Item)
                .Where(p => p.userid == user.id)
                .ToList();

            // Calculamos Fuerza Total
            int bonusStrength = userPurchases
                .Where(p => p.Item != null && p.Item.type == "Strength") // Added null check just in case
                .Sum(p => p.Item.bonus);
            
            int totalStrength = user.strength + bonusStrength;

            // Calculamos Resistencia Total
            int bonusEndurance = userPurchases
                .Where(p => p.Item != null && p.Item.type == "Endurance")
                .Sum(p => p.Item.bonus);

            int totalEndurance = user.endurance + bonusEndurance;

            // ---------------------------------------------------------
            // 2. CÁLCULO DE EXPERIENCIA (NUEVO)
            // ---------------------------------------------------------
            
            // Calculamos cuánta XP necesita en total para pasar este nivel
            int xpRequiredForNextLevel = (int)Math.Floor(BASE_XP * Math.Pow(user.level, EXPONENT));
            
            // Calculamos cuánto le falta
            int xpRemaining = xpRequiredForNextLevel - user.experience;
            // Aseguramos que no de negativo visualmente (aunque la lógica lo maneje)
            if (xpRemaining < 0) xpRemaining = 0; 

            // ---------------------------------------------------------

            // 3. CREACIÓN DE CLAIMS
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Role, user.role),
                new Claim("email", user.email),
                new Claim("name", user.name),
                new Claim("passwordhash", user.passwordhash), 
                
                new Claim("level", user.level.ToString()),
                
                // Stats calculadas
                new Claim("strength", totalStrength.ToString()), 
                new Claim("endurance", totalEndurance.ToString()), 
                
                new Claim("gold", user.gold.ToString()),
                new Claim("consistencystreak", user.consistencystreak.ToString()),

                // Claims de experiencia
                new Claim("experience", user.experience.ToString()),       // XP Actual
                new Claim("xpRequired", xpRequiredForNextLevel.ToString()), // Meta del nivel
                new Claim("xpRemaining", xpRemaining.ToString()),            // Cuánto falta
                
                // Items equipados
                new Claim("equippedStrengthItemId", user.equippedStrengthId?.ToString() ?? ""),
                new Claim("equippedEnduranceItemId", user.equippedEnduranceId?.ToString() ?? ""),
                
                // --- NUEVO CLAIM: AVATAR URL ---
                // Si avatarUrl es null, devolvemos un string vacío para evitar excepciones
                new Claim("avatarUrl", user.avatarUrl ?? "") 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7), // Token válido por 7 días
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}