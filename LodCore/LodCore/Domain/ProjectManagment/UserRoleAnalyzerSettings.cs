using Journalist;

namespace LodCore.Domain.ProjectManagment
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

        public int AppropriateEditDistance { get; }

        public string DefaultRole { get; }
    }
}