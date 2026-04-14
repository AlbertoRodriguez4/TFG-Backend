using AA2_CS.Model;
using AA2_CS.Service;
using Microsoft.EntityFrameworkCore;
using AA2_CS.Database;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AA2_CS.JWT;
using AA2_CS.Services;
using AA2_CS.Repository;

var builder = WebApplication.CreateBuilder(args);
Environment.SetEnvironmentVariable("ASPNETCORE_URLS", "http://+:6873");

// Configuración CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173") // Actualiza el puerto del frontend
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()); // Permitir cookies/autenticación si es necesario
});

// Agregar servicios a la contenedor
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();  // Asegúrate de agregar esto para registrar los controladores
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=3786;Database=postgres;Username=postgres;Password=password")); // Para conexión con PostgreSQL, cambiado "postgres" a "localhost" y el puerto, de 5432 a 3786



// Registrar los servicios específicos
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<PlanService>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<JWTConfigurer>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<SubscriptionService>();
builder.Services.AddScoped<EmailVerificationRepository, EmailVerificationRepository>();
builder.Services.AddScoped<ItemRepository, ItemRepository>();
builder.Services.AddScoped<PlanRepository, PlanRepository>();
builder.Services.AddScoped<PurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<RoomRepository, RoomRepository>();
builder.Services.AddScoped<UserRepository, UserRepository>();
builder.Services.AddScoped<TasksRepository, TasksRepository>();
builder.Services.AddScoped<TasksService, TasksService>();
builder.Services.AddScoped<SubscriptionRepository, SubscriptionRepository>();
builder.Services.AddHttpClient<GeocodificacionRepository, GeocodificacionRepository>();
builder.Services.AddScoped<GeocodificacionService, GeocodificacionService>();
builder.Services.AddScoped<UserRoomRepository, UserRoomRepository>();
builder.Services.AddScoped<UserRoomService, UserRoomService>();
builder.Services.AddScoped<NotificationPreferenceRepository, NotificationPreferenceRepository>();
builder.Services.AddScoped<NotificationPreferenceService, NotificationPreferenceService>();
builder.Services.AddScoped<NotificationService, NotificationService>();
builder.Services.AddHostedService<NotificationBackgroundService>();

// Configuración de JWT para autenticación
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, //verificar que el issuer sea válido
            ValidateAudience = true, //verificar que el audience sea valido
            ValidateLifetime = false, //verificar que el token no haya expirado
            ValidateIssuerSigningKey = true, //verificar la clave de firma sea válida
            ValidIssuer = config["JwtSettings:Issuer"], //el emisor que se espera (The Training Hub)
            ValidAudience = config["JwtSettings:Audience"], //la audiencia que se espera (The Training Hub Users)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"])) //clave secreta usada para validar la firma del token
        };
    });

builder.Services.AddAuthorization(); // Agregar el servicio de autorización

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();  // Aplica migraciones pendientes
}
// Configurar el pipeline de la solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar CORS antes de los controladores
app.UseCors("AllowFrontend");
app.Urls.Add("http://0.0.0.0:6873");

app.UseHttpsRedirection();
app.UseAuthentication(); // Usar la autenticación para JWT y los tokens
app.UseAuthorization();

// Mapeo de controladores
app.MapControllers();

app.Run();
