﻿using Journalist;

namespace LodCore.Domain.UserManagement
{
    public class ProfileSettings
    {
        public ProfileSettings(string frontendProfileUri)
        {
            Require.NotEmpty(frontendProfileUri, nameof(frontendProfileUri));

            FrontendProfileUri = frontendProfileUri;
        }

        public readonly string FrontendProfileUri;
    }
}