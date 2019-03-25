using System;

namespace LodCore.QueryService.Views.ProjectView
{
    public class ProjectMembershipView : IEquatable<ProjectMembershipView>
    {
        public ProjectMembershipView(int developerId, string role)
        {
            DeveloperId = developerId;
            Role = role;
        }

        public int DeveloperId { get; }
        public string Role { get; }

        public bool Equals(ProjectMembershipView other)
        {
            return DeveloperId == other.DeveloperId;
        }
    }
}