using LodCoreLibraryOld.Domain.ProjectManagment;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Mappings
{
    public class ProjectMembershipMap : ClassMapping<ProjectMembership>
    {
        public ProjectMembershipMap()
        {
            Id(project => project.MembershipId, mapper => mapper.Generator(Generators.Identity));
            Property(project => project.DeveloperId);
            Property(project => project.Role);
        }
    }
}