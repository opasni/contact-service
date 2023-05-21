
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using services.Models;
using services.Models.Settings;
using services.Utility;
using System.Net;

namespace services.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Creates a <see cref="MimeMessage"/> instance using the provided parameters.
        /// </summary>
        /// <param name="receivers">The e-mails (Key) and display names (Value) of the receivers.</param>
        /// <param name="subject">The mail subject.</param>
        /// <param name="message">The mail message.</param>
        MimeMessage CreateMail(ContactData receiver, string subject, string message);

        /// <summary>
        /// Sends the specified mail.
        /// </summary>
        /// <param name="mail">The mail service.</param>
        /// <exception cref="ApplicationException">E-mail could not be sent</exception>
        Task SendAsync(MimeMessage mail);
    }

    public class EmailService : IEmailService
    {
        private readonly ILogger logger;
        private readonly EmailSettings emailSettings;

        public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> options)
        {
            this.logger = logger;
            this.emailSettings = options.Value;
        }

        public MimeMessage CreateMail(ContactData receiver, string subject, string message)
        {
            var mail = new MimeMessage();

            mail.From.Add(new MailboxAddress(emailSettings.DisplayName, emailSettings.Username));
            if (string.IsNullOrEmpty(receiver.Name))
            {
                mail.To.Add(MailboxAddress.Parse(receiver.Email));
            }
            else
            {
                mail.To.Add(new MailboxAddress(receiver.Name, receiver.Email));
            }

            mail.Subject = subject;

            mail.Body = new TextPart("html")
            {
                Text = message
            };

            return mail;
        }

        public async Task SendAsync(MimeMessage mail)
        {
            try
            {
                using var client = new SmtpClient();
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                //client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(emailSettings.Host, emailSettings.Port).ConfigureAwait(true);

                // Note: only needed if the SMTP server requires authentication
                var credentials = new NetworkCredential
                {
                    UserName = emailSettings.Username,
                    Password = emailSettings.Password
                };
                await client.AuthenticateAsync(credentials).ConfigureAwait(true);

                await client.SendAsync(mail).ConfigureAwait(false);

                await client.DisconnectAsync(true).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, LoggingMessage.GetMessage(LogLevel.Error, LoggingMessage.EmailError), mail.To, ex.Message);
                throw new ApplicationException("", ex);
            }
            finally
            {
                logger.LogInformation(LoggingMessage.GetMessage(LogLevel.Information, LoggingMessage.EmailSent), mail.To);
            }
        }
    }
}
