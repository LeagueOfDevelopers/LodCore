using System;
using System.Collections.Specialized;
using Gateways.Redmine;
using Mailing;
using ProjectManagement.Domain;

namespace FrontendServices.App_Data
{
    public static class SettingsReader
    {
        public static MailerSettings ReadMailerSettings(NameValueCollection settings)
        {
            return new MailerSettings(
                settings["MailerSettings.SmtpServer"],
                Int32.Parse(settings["MailerSettings.Port"]), 
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
    }
}