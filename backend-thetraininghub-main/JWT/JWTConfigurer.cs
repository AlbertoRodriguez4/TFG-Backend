using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AA2_CS.Model;
using AA2_CS.Database; // Necesario para acceder a las compras
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore; // Necesario para .Include

namespace AA2_CS.JWT
{
    public class JWTConfigurer
    {
        private readonly IConfiguration _config;
        // Inyectamos el DbContext para leer los items del usuario
        private readonly AppDbContext _context; 

        public JWTConfigurer(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        public string GenerateToken(User user)
        {
            // 1. CÁLCULO DE STATS TOTALES (Base + Items)
            // Primero, obtenemos las compras del usuario incluyendo el Item
            var userPurchases = _context.Purchases
                .Include(p => p.Item)
                .Where(p => p.userid == user.id)
                .ToList();

            // Calculamos Fuerza Total
            int bonusStrength = userPurchases
                .Where(p => p.Item.type == "Strength")
                .Sum(p => p.Item.bonus);
            
            int totalStrength = user.strength + bonusStrength;

            // Calculamos Resistencia Total
            int bonusEndurance = userPurchases
                .Where(p => p.Item.type == "Endurance")
                .Sum(p => p.Item.bonus);

            int totalEndurance = user.endurance + bonusEndurance;


            // 2. CREACIÓN DE CLAIMS CON LOS DATOS CALCULADOS
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Role, user.role),
                new Claim("email", user.email),
                new Claim("name", user.name),
                // Nota: Pasar el passwordhash en el token es una mala práctica de seguridad,
                // pero lo dejo porque así lo tenías. Idealmente, quítalo.
                new Claim("passwordhash", user.passwordhash), 
                
                new Claim("level", user.level.ToString()),
                
                // AQUÍ USAMOS LOS TOTALES CALCULADOS
                new Claim("strength", totalStrength.ToString()), 
                new Claim("endurance", totalEndurance.ToString()), 
                
                new Claim("gold", user.gold.ToString()),
                new Claim("consistencystreak", user.consistencystreak.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:DurationInMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}