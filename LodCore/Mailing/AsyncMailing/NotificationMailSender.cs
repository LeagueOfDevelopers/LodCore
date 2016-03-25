using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Journalist;
using UserManagement.Infrastructure;

namespace Mailing.AsyncMailing
{
    public class NotificationMailSender
    {
        public NotificationMailSender(
            MailerSettings mailerSettings, 
            IUserRepository userRepository, 
            INotificationMailRepository notificationMailRepository)
        {
            Require.NotNull(mailerSettings, nameof(mailerSettings));
            Require.NotNull(userRepository, nameof(userRepository));
            Require.NotNull(notificationMailRepository, nameof(notificationMailRepository));

            _mailerSettings = mailerSettings;
            _userRepository = userRepository;
            _notificationMailRepository = notificationMailRepository;
            _currentTimeout = mailerSettings.BasicEmailTimeout;
        }

        public void StartSending()
        {
            Task.Factory.StartNew(() =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    var emailProcessed = TryProcessEmail();
                    _currentTimeout = emailProcessed
                        ? _mailerSettings.BasicEmailTimeout
                        : _currentTimeout + _mailerSettings.TimeoutIncrement;
                    Task.Delay(_currentTimeout);
                }
            }, 
            _cancellationTokenSource.Token, 
            TaskCreationOptions.LongRunning, 
            TaskScheduler.Default);
        }

        public void StopSending()
        {
            var cancellationTokenSnapshot = _cancellationTokenSource;
            _cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSnapshot.Cancel();
            cancellationTokenSnapshot.Dispose();
        }

        private bool TryProcessEmail()
        {
            var notificationEmail = _notificationMailRepository.PullNotificationEmail();
            if (notificationEmail == null)
            {
                return false;
            }

            try
            {
                SendMail(notificationEmail);
                _notificationMailRepository.RemoveNotificationEmail(notificationEmail);
            }
            catch (Exception exception)
            {
                Trace.WriteLine($"Failed to send email {notificationEmail.Id}, because of {exception.Message}");
                _notificationMailRepository.SaveNotificationEmail(new NotificationEmail(
                    notificationEmail.UserIds.ToArray(),
                    notificationEmail.NotificationDescription));
                return false;
            }

            return true;
        }

        private void SendMail(NotificationEmail notificationEmail)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(_mailerSettings.From)
            };
            var client = _mailerSettings.GetSmtpClient();

            mail.Subject = MailingResources.NotificationMailCaption;
            mail.Body = string.Format(MailingResources.NotificationMessageTemplate, notificationEmail.NotificationDescription);

            foreach (var emailAdress in notificationEmail.UserIds.ToArray().Select(userId => _userRepository.GetAccount(userId).Email))
            {
                mail.To.Add(emailAdress);

                client.Send(mail);

                mail.To.Clear();
            }

            mail.Dispose();
        }

        private TimeSpan _currentTimeout;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly MailerSettings _mailerSettings;
        private readonly IUserRepository _userRepository;
        private readonly INotificationMailRepository _notificationMailRepository;
    }
}