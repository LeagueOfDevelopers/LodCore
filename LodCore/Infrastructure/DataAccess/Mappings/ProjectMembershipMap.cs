using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using ProjectManagement.Domain;

namespace DataAccess.Mappings
{
    public class ProjectMembershipMap : ClassMapping<ProjectMembership>
    {
        public ProjectMembershipMap()
        {
            Id(project => project.MembershipId, mapper => mapper.Generator(Generators.Identity));
            Property(project => project.DeveloperId);
            Property(project => project.Role);
            ////ManyToOne(project => project.Project, mapper =>
            ////{
            ////    mapper.Cascade(Cascade.All);
            ////    mapper.Column("ProjectId");
            ////});
        }
    }
}