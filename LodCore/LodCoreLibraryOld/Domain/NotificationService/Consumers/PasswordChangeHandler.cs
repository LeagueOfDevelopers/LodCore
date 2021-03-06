﻿using LodCoreLibraryOld.Common;
using LodCoreLibraryOld.Domain.UserManagement;
using LodCoreLibraryOld.Infrastructure.DataAccess.Repositories;
using LodCoreLibraryOld.Infrastructure.Mailing;

namespace LodCoreLibraryOld.Domain.NotificationService
{
    public class PasswordChangeHandler : IEventConsumer<PasswordChangeRequest>
    {
        private readonly ApplicationLocationSettings _applicationLocationSettings;

        private readonly IMailer _mailer;
        private readonly IPasswordManager _passwordManager;
        private readonly IUserRepository _userRepository;

        public PasswordChangeHandler(
            IMailer mailer,
            IPasswordManager passwordManager,
            ApplicationLocationSettings applicationLocationSettings,
            IUserRepository userRepository)
        {
            _mailer = mailer;
            _passwordManager = passwordManager;
            _applicationLocationSettings = applicationLocationSettings;
            _userRepository = userRepository;
        }

        public void Consume(PasswordChangeRequest request)
        {
            var user = _userRepository.GetAccount(request.UserId);
            var link = $"{_applicationLocationSettings.FrontendAdress}/password/recovery/{request.Token}";
            _mailer.SendPasswordResetMail(user.Firstname, link, user.Email);
        }
    }
}