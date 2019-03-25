using Journalist;

namespace LodCore.Domain.UserManagement
{
    public class ProfileSettings
    {
        public readonly string FrontendProfileUri;

        public ProfileSettings(string frontendProfileUri)
        {
            Require.NotEmpty(frontendProfileUri, nameof(frontendProfileUri));

            FrontendProfileUri = frontendProfileUri;
        }
    }
}