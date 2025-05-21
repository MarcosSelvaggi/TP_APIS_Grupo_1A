using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Services
{
    public class EmailService
    {
        private MailMessage email;
        private SmtpClient server;

        public EmailService() 
        {
            server = new SmtpClient();
            string user = ConfigurationManager.AppSettings["EmailUsername"];
            string pass = ConfigurationManager.AppSettings["EmailPassword"];
            server.Credentials = new NetworkCredential(user, pass);
            server.EnableSsl = true;
            server.Port = 587;
            server.Host = "smtp.gmail.com";
        }

        public void correo(string emailDestino)
        {
            email = new MailMessage();
            email.From = new MailAddress("correoprogramaciontp@gmail.com", "Promoción Tienda Electronics Store");
            email.To.Add(emailDestino);
            email.Subject = "¡Gracias por participar y canjear tu premio!";
            email.IsBodyHtml = true;
            email.Body = "<img src='https://i.ibb.co/F4xqv50Q/logo.png' alt='Logo' style='width:150px;' /><br/>" +
                "<h2>¡Felicidades!</h2><br/>" +
                "<p>Tu participación en nuestro sorteo ha sido registrada exitosamente.</p>" +
                "<p>Gracias por confiar en nosotros. ¡Esperamos verte pronto nuevamente!</p>";
        }

        public void enviarEmail() 
        {
            try
            {
                server.Send(email);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
