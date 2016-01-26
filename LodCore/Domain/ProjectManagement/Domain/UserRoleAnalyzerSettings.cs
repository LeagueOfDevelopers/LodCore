using Journalist;

namespace ProjectManagement.Domain
{
    public class UserRoleAnalyzerSettings
    {
        public UserRoleAnalyzerSettings(int appropriateEditDistance, string defaultRole)
        {
            Require.Positive(appropriateEditDistance, nameof(appropriateEditDistance));
            Require.NotEmpty(defaultRole, nameof(defaultRole));

            AppropriateEditDistance = appropriateEditDistance;
            DefaultRole = defaultRole;
        }

        public int AppropriateEditDistance { get; private set; } 
        
        public string DefaultRole { get; private set; }
    }
}