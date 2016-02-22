using System;
using Journalist;

namespace UserManagement.Application
{
    public class ConfirmationSettings
    {
        public ConfirmationSettings(
            Uri frontendMailConfirmationUri,
            bool gitlabAccountCreationEnabled, 
            bool redmineAccountCreationEnabled)
        {
            Require.NotNull(frontendMailConfirmationUri, nameof(frontendMailConfirmationUri));
            FrontendMailConfirmationUri = frontendMailConfirmationUri;
            GitlabAccountCreationEnabled = gitlabAccountCreationEnabled;
            RedmineAccountCreationEnabled = redmineAccountCreationEnabled;
        }

        public Uri FrontendMailConfirmationUri { get; private set; }
        
        public bool RedmineAccountCreationEnabled { get; private set; } 

        public bool GitlabAccountCreationEnabled { get; private set; }
    }
}