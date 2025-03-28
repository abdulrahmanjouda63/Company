using System.Net;
using System.Net.Mail;

namespace Company.PL.Helpers
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            // Mail Server : Gmail
            // SMTP
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("abdulrahmanjouda63@gmail.com", "qqgofrozbhzflikv"); // Sender
                client.Send("abdulrahmanjouda63@gmail.com", email.To, email.Subject, email.Body);

                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
    }
}
