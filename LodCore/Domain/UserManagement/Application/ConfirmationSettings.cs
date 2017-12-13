using System;
using Journalist;

namespace UserManagement.Application
{
    public class ConfirmationSettings
    {
        public ConfirmationSettings(
            Uri frontendMailConfirmationUri)
        {
            Require.NotNull(frontendMailConfirmationUri, nameof(frontendMailConfirmationUri));
            FrontendMailConfirmationUri = frontendMailConfirmationUri;
        }

        public Uri FrontendMailConfirmationUri { get; private set; }
    }
}