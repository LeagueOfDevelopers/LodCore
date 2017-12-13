using System;
using System.Collections.Specialized;
using Common;
using FilesManagement;
using Mailing;
using ProjectManagement.Domain;
using UserManagement.Application;

namespace FrontendServices.App_Data
{
    public static class SettingsReader
    {
        public static MailerSettings ReadMailerSettings(NameValueCollection settings)
        {
            return new MailerSettings(
                settings["MailerSettings.SmtpServer"],
                int.Parse(settings["MailerSettings.Port"]),
                settings["MailerSettings.Password"],
                settings["MailerSettings.From"],
                settings["MailerSettings.MessageTemplate"],
                settings["MailerSettings.Caption"],
                int.Parse(settings["MailerSettings.BasicEmailTimeoutInSecond"]),
                int.Parse(settings["MailerSettings.TimeoutIncrementInSeconds"]),
                int.Parse(settings["MailerSettings.MaxEmailTimeout"]));
        }

        public static UserRoleAnalyzerSettings ReadUserRoleAnalyzerSettings(NameValueCollection settings)
        {
            return new UserRoleAnalyzerSettings(
                int.Parse(settings["UserRoleAnalyzer.AppropriateEditDistance"]),
                settings["UserRoleAnalyzer.DefaultRole"]);
        }

        public static RelativeEqualityComparer ReadRelativeEqualityComparerSettings(NameValueCollection settings)
        {
            return new RelativeEqualityComparer(int.Parse(settings["UserRoleAnalyzer.AppropriateEditDistance"]));
        }

        public static IssuePaginationSettings ReadIssuePaginationSettings(NameValueCollection settings)
        {
            return new IssuePaginationSettings(0);
        }

        public static ApplicationLocationSettings ReadApplicationLocationSettings(NameValueCollection settings)
        {
            return new ApplicationLocationSettings(settings["BackendDomain"], settings["FrontendDomain"]);
        }

        public static FileStorageSettings ReadFileStorageSettings(NameValueCollection settings)
        {
            return new FileStorageSettings(
                settings["FileStorage.FileFolder"],
                settings["FileStorage.FileExtensions"].Split(','),
                settings["FileStorage.ImageFolder"],
                settings["FileStorage.ImageExtensions"].Split(','));
        }

        public static PaginationSettings ReadProjectsPaginationSettings(NameValueCollection settings)
        {
            return new PaginationSettings(int.Parse(settings["Projects.Pagination.PageSize"]));
        }

        public static NotificationService.PaginationSettings ReadNotificationsPaginationSettings(
            NameValueCollection settings)
        {
            return new NotificationService.PaginationSettings(int.Parse(settings["Notifications.Pagination.PageSize"]));
        }

        public static ConfirmationSettings ReadConfirmationSettings(NameValueCollection settings)
        {
            return new ConfirmationSettings(new Uri(settings["Confirmation.FrontendConfirmationUri"]));
        }
    }
}