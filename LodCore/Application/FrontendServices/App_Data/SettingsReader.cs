using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using Common;
using FilesManagement;
using Gateways.Gitlab;
using Gateways.Redmine;
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
                settings["MailerSettings.Caption"]);
        }

        public static RedmineSettings ReadRedmineSettings(NameValueCollection settings)
        {
            return new RedmineSettings(
                settings["Redmine.Host"],
                settings["Redmine.ApiKey"]);
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

        public static GitlabSettings ReadGitlabSettings(NameValueCollection settings)
        {
            return new GitlabSettings(
                settings["Gitlab.Host"],
                settings["Gitlab.ApiKey"]);
        }

        public static FileStorageSettings ReadFileStorageSettings(NameValueCollection settings)
        {
            return new FileStorageSettings(
                settings["FileStorage.FileFolder"],
                settings["FileStorage.FileExtensions"].Split(','),
                settings["FileStorage.ImageFolder"],
                settings["FileStorage.ImageExtensions"].Split(','));
        }

        public static ProjectManagement.Domain.PaginationSettings ReadProjectsPaginationSettings(NameValueCollection settings)
        {
            return new ProjectManagement.Domain.PaginationSettings(int.Parse(settings["Projects.Pagination.PageSize"]));
        }

        public static NotificationService.PaginationSettings ReadNotificationsPaginationSettings(NameValueCollection settings)
        {
            return new NotificationService.PaginationSettings(int.Parse(settings["Notifications.Pagination.PageSize"]));
        }

        public static ConfirmationSettings ReadConfirmationSettings(NameValueCollection settings)
        {
            return new ConfirmationSettings(
                new Uri(settings["Confirmation.FrontendConfirmationUri"]),
                bool.Parse(settings["Confirmation.GitlabAccountCreationEnabled"]),
                bool.Parse(settings["Confirmation.RedmineAccountCreationEnabled"]));
        }
    }
}