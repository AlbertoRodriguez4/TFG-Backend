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
    }
}
