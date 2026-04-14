using System.Net;
using System.Net.Mail;
using System.Text;

namespace AA2_CS.Service
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService()
        {
            // Configuración SMTP de Gmail
            _smtpServer = "smtp.gmail.com";
            _smtpPort = 587;
            _smtpUser = "a26873@svalero.com";
            _smtpPass = "dmux uzgn dsvv cxsq"; // App password
            _fromEmail = "a26873@svalero.com";
            _fromName = "The Training Hub";
        }

        public async Task<bool> SendVerificationEmail(string toEmail, string userName, string verificationCode)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(_fromEmail, _fromName);
                message.To.Add(toEmail);
                message.Subject = "Verifica tu cuenta - The Training Hub";
                message.Body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px; }}
        .container {{ max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1); }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; }}
        .header h1 {{ margin: 0; font-size: 28px; }}
        .content {{ padding: 40px 30px; }}
        .greeting {{ font-size: 18px; color: #333; margin-bottom: 20px; }}
        .message {{ font-size: 16px; color: #666; line-height: 1.6; margin-bottom: 30px; }}
        .code-container {{ text-align: center; margin: 30px 0; }}
        .verification-code {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; font-size: 32px; font-weight: bold; padding: 20px 40px; border-radius: 8px; letter-spacing: 5px; }}
        .warning {{ background-color: #fff3cd; border-left: 4px solid #ffc107; padding: 15px; margin: 20px 0; font-size: 14px; color: #856404; }}
        .footer {{ background-color: #f8f9fa; padding: 20px; text-align: center; font-size: 14px; color: #6c757d; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🏋️ The Training Hub</h1>
            <p style='margin: 10px 0 0 0; opacity: 0.9;'>Verificación de Cuenta</p>
        </div>
        <div class='content'>
            <p class='greeting'>¡Hola, {userName}!</p>
            <p class='message'>
                Gracias por registrarte en <strong>The Training Hub</strong>.
                Para completar tu registro y verificar tu cuenta, por favor utiliza el siguiente código de verificación:
            </p>
            <div class='code-container'>
                <div class='verification-code'>{verificationCode}</div>
            </div>
            <div class='warning'>
                <strong>⚠️ Importante:</strong> Este código expira en <strong>15 minutos</strong>.
                Si no solicitaste este código, puedes ignorar este correo de forma segura.
            </div>
            <p class='message'>
                Una vez verificado tu correo, tendrás acceso completo a todas las funcionalidades de la plataforma.
            </p>
        </div>
        <div class='footer'>
            <p>&copy; 2026 The Training Hub. Todos los derechos reservados.</p>
        </div>
    </div>
</body>
</html>";
                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;

                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;

                    await client.SendMailAsync(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar email: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendInactivityReminderEmail(string toEmail, string userName, int inactiveDays)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(_fromEmail, _fromName);
                message.To.Add(toEmail);
                message.Subject = $"Te echamos de menos, {userName} - The Training Hub";
                message.Body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px; }}
        .container {{ max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1); }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; }}
        .header h1 {{ margin: 0; font-size: 28px; }}
        .content {{ padding: 40px 30px; }}
        .greeting {{ font-size: 18px; color: #333; margin-bottom: 20px; }}
        .message {{ font-size: 16px; color: #666; line-height: 1.6; margin-bottom: 30px; }}
        .highlight {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; font-size: 24px; font-weight: bold; padding: 15px 30px; border-radius: 8px; }}
        .cta-button {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; text-decoration: none; padding: 15px 30px; border-radius: 8px; font-size: 16px; font-weight: bold; margin-top: 20px; }}
        .footer {{ background-color: #f8f9fa; padding: 20px; text-align: center; font-size: 14px; color: #6c757d; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🏋️ The Training Hub</h1>
            <p style='margin: 10px 0 0 0; opacity: 0.9;'>Recordatorio de Inactividad</p>
        </div>
        <div class='content'>
            <p class='greeting'>¡Hola, {userName}!</p>
            <p class='message'>
                Llevas <strong class='highlight'>{inactiveDays} días</strong> sin registrar un entrenamiento.
                ¡Tu progreso importa! Vuelve a entrenar y sigue mejorando tus estadísticas.
            </p>
            <p class='message'>
                La constancia es la clave del éxito. Registra tu rutina de hoy y mantén tu racha activa.
            </p>
        </div>
        <div class='footer'>
            <p>&copy; 2026 The Training Hub. Todos los derechos reservados.</p>
        </div>
    </div>
</body>
</html>";
                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;

                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    await client.SendMailAsync(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar email de inactividad: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendRoomActivityEmail(string toEmail, string userName, string roomName, string action)
        {
            var actionText = action == "created" ? "creado" : "te has unido a";
            var actionTitle = action == "created" ? "Sala Creada" : "Te Has Unido a una Sala";

            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(_fromEmail, _fromName);
                message.To.Add(toEmail);
                message.Subject = $"{actionTitle} - The Training Hub";
                message.Body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px; }}
        .container {{ max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1); }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; }}
        .header h1 {{ margin: 0; font-size: 28px; }}
        .content {{ padding: 40px 30px; }}
        .greeting {{ font-size: 18px; color: #333; margin-bottom: 20px; }}
        .message {{ font-size: 16px; color: #666; line-height: 1.6; margin-bottom: 30px; }}
        .room-name {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; font-size: 20px; font-weight: bold; padding: 12px 24px; border-radius: 8px; }}
        .footer {{ background-color: #f8f9fa; padding: 20px; text-align: center; font-size: 14px; color: #6c757d; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🏋️ The Training Hub</h1>
            <p style='margin: 10px 0 0 0; opacity: 0.9;'>{actionTitle}</p>
        </div>
        <div class='content'>
            <p class='greeting'>¡Hola, {userName}!</p>
            <p class='message'>
                Has {actionText} la sala de entrenamiento <strong class='room-name'>{roomName}</strong>.
                ¡Prepárate para entrenar con otros usuarios y mejorar tus estadísticas!
            </p>
        </div>
        <div class='footer'>
            <p>&copy; 2026 The Training Hub. Todos los derechos reservados.</p>
        </div>
    </div>
</body>
</html>";
                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;

                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    await client.SendMailAsync(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar email de actividad de sala: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendPurchaseReceiptEmail(string toEmail, string userName, string itemName, string itemType, int itemBonus, int itemPrice, DateTime purchaseDate)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(_fromEmail, _fromName);
                message.To.Add(toEmail);
                message.Subject = "Confirmación de Compra - The Training Hub";
                message.Body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px; }}
        .container {{ max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1); }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; }}
        .header h1 {{ margin: 0; font-size: 28px; }}
        .content {{ padding: 40px 30px; }}
        .greeting {{ font-size: 18px; color: #333; margin-bottom: 20px; }}
        .message {{ font-size: 16px; color: #666; line-height: 1.6; margin-bottom: 20px; }}
        .receipt {{ background-color: #f8f9fa; border-radius: 8px; padding: 20px; margin: 20px 0; }}
        .receipt-row {{ display: flex; justify-content: space-between; padding: 10px 0; border-bottom: 1px solid #e9ecef; }}
        .receipt-row:last-child {{ border-bottom: none; font-weight: bold; font-size: 18px; }}
        .receipt-label {{ color: #666; }}
        .receipt-value {{ color: #333; }}
        .footer {{ background-color: #f8f9fa; padding: 20px; text-align: center; font-size: 14px; color: #6c757d; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🏋️ The Training Hub</h1>
            <p style='margin: 10px 0 0 0; opacity: 0.9;'>Confirmación de Compra</p>
        </div>
        <div class='content'>
            <p class='greeting'>¡Hola, {userName}!</p>
            <p class='message'>Tu compra se ha realizado con éxito. Aquí tienes el recibo:</p>
            <div class='receipt'>
                <div class='receipt-row'>
                    <span class='receipt-label'>Objeto:</span>
                    <span class='receipt-value'>{itemName}</span>
                </div>
                <div class='receipt-row'>
                    <span class='receipt-label'>Tipo:</span>
                    <span class='receipt-value'>{itemType}</span>
                </div>
                <div class='receipt-row'>
                    <span class='receipt-label'>Bonificación:</span>
                    <span class='receipt-value'>+{itemBonus}</span>
                </div>
                <div class='receipt-row'>
                    <span class='receipt-label'>Precio:</span>
                    <span class='receipt-value'>{itemPrice} oro</span>
                </div>
                <div class='receipt-row'>
                    <span class='receipt-label'>Fecha:</span>
                    <span class='receipt-value'>{purchaseDate:dd/MM/yyyy HH:mm}</span>
                </div>
            </div>
        </div>
        <div class='footer'>
            <p>&copy; 2026 The Training Hub. Todos los derechos reservados.</p>
        </div>
    </div>
</body>
</html>";
                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;

                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    await client.SendMailAsync(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar email de recibo de compra: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendSubscriptionExpiryEmail(string toEmail, string userName, DateTime expiryDate, int daysLeft)
        {
            try
            {
                var urgencyText = daysLeft <= 1 ? "¡Tu suscripción expira mañana!" :
                                  daysLeft <= 2 ? "¡Tu suscripción expira en 2 días!" :
                                  $"Tu suscripción expira en {daysLeft} días.";

                var message = new MailMessage();
                message.From = new MailAddress(_fromEmail, _fromName);
                message.To.Add(toEmail);
                message.Subject = $"Tu Plan Premium expira en {daysLeft} día(s) - The Training Hub";
                message.Body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px; }}
        .container {{ max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1); }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; }}
        .header h1 {{ margin: 0; font-size: 28px; }}
        .content {{ padding: 40px 30px; }}
        .greeting {{ font-size: 18px; color: #333; margin-bottom: 20px; }}
        .message {{ font-size: 16px; color: #666; line-height: 1.6; margin-bottom: 20px; }}
        .warning {{ background-color: #fff3cd; border-left: 4px solid #ffc107; padding: 15px; margin: 20px 0; font-size: 14px; color: #856404; }}
        .expiry-date {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; font-size: 20px; font-weight: bold; padding: 12px 24px; border-radius: 8px; }}
        .cta-button {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; text-decoration: none; padding: 15px 30px; border-radius: 8px; font-size: 16px; font-weight: bold; margin-top: 20px; }}
        .footer {{ background-color: #f8f9fa; padding: 20px; text-align: center; font-size: 14px; color: #6c757d; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🏋️ The Training Hub</h1>
            <p style='margin: 10px 0 0 0; opacity: 0.9;'>Aviso de Expiración</p>
        </div>
        <div class='content'>
            <p class='greeting'>¡Hola, {userName}!</p>
            <p class='message'>
                {urgencyText} Tu Plan Premium expira el <strong class='expiry-date'>{expiryDate:dd/MM/yyyy}</strong>.
            </p>
            <div class='warning'>
                <strong>⚠️ Importante:</strong> Cuando expire tu suscripción, perderás acceso a las funciones premium.
                ¡Renueva ahora para seguir disfrutando de todas las ventallas!
            </div>
        </div>
        <div class='footer'>
            <p>&copy; 2026 The Training Hub. Todos los derechos reservados.</p>
        </div>
    </div>
</body>
</html>";
                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;

                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    await client.SendMailAsync(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar email de expiración de suscripción: {ex.Message}");
                return false;
            }
        }
    }
}
