using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AA2_CS.Model;
using Microsoft.IdentityModel.Tokens;
namespace AA2_CS.JWT;
public class JWTConfigurer
{
    private readonly IConfiguration _config;

    public JWTConfigurer(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
        new Claim(ClaimTypes.Role, user.role), //Generar tokens dependiendo del rol del usuario
        new Claim("email", user.email),
        new Claim("name", user.name),
        new Claim ("passwordhash", user.passwordhash),
        new Claim("level", user.level.ToString()),
        new Claim("strength", user.strength.ToString()),
        new Claim("endurance", user.endurance.ToString()), //Campos que voy a pasar en el JWT
        new Claim("gold", user.gold.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"])); //Accede a la configuaración del JWT en el appsettings.json
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //Credenciales del token
        var expiry = DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:DurationInMinutes"])); //Duración del token

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"], //"Objeto" que enviamos en el jwt
            claims: claims,
            expires: expiry,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token); //Devolver el token
    }
}
