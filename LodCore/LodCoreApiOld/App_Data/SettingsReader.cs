using LodCoreLibraryOld.Common;
using LodCoreLibraryOld.Domain.ProjectManagment;
using LodCoreLibraryOld.Domain.UserManagement;
using LodCoreLibraryOld.Infrastructure.EventBus;
using LodCoreLibraryOld.Infrastructure.FilesManagement;
using LodCoreLibraryOld.Infrastructure.Gateway;
using LodCoreLibraryOld.Infrastructure.Mailing;
using System;
using System.Collections.Specialized;
using System.Web.Configuration;

namespace LodCoreApiOld.App_Data
{
    public static class SettingsReader
    {
        public static GithubGatewaySettings ReadGithubGatewaySettings(NameValueCollection settings)
        {
            return new GithubGatewaySettings(
                settings["GithubGateway.ClientId"],
                settings["GithubGateway.ClientSecret"],
                settings["GithubGateway.GithubApiDefaultCallbackUri"],
                settings["GithubGateway.OrganizationName"]);
        }

        public static LodCoreLibraryOld.Domain.UserManagement.ProfileSettings ReadProfileSettings(NameValueCollection settings)
        {
            return new LodCoreLibraryOld.Domain.UserManagement.ProfileSettings(settings["Profile.FrontendProfileUri"]);
        }

        public static EventBusSettings ReadEventBusSettings(NameValueCollection settings)
        {
            return new EventBusSettings(
                settings["EventBusSettings.HostName"],
                settings["EventBusSettings.VirtualHost"],
                settings["EventBusSettings.UserName"],
                settings["EventBusSettings.Password"]);
        }

        public static MailerSettings ReadMailerSettings(NameValueCollection settings)
        {
            return new MailerSettings(
                settings["MailerSettings.SmtpRelayer"],
                settings["MailerSettings.SmtpServer"],
                int.Parse(settings["MailerSettings.Port"]),
                settings["MailerSettings.Password"],
                settings["MailerSettings.From"],
                settings["MailerSettings.DisplayName"],
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

        public static ProjectPaginationSettings ReadProjectPaginationSettings(NameValueCollection settings)
        {
            return new ProjectPaginationSettings(int.Parse(settings["Projects.Pagination.PageSize"]));
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
        
        public static PaginationSettings ReadNotificationsPaginationSettings(
            NameValueCollection settings)
        {
            return new PaginationSettings(int.Parse(settings["Notifications.Pagination.PageSize"]));
        }

        public static ConfirmationSettings ReadConfirmationSettings(NameValueCollection settings)
        {
            return new ConfirmationSettings(new Uri(settings["Confirmation.FrontendConfirmationUri"]));
        }
    }
}