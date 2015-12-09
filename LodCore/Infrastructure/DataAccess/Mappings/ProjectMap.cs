using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using ProjectManagement.Domain;

namespace DataAccess.Mappings
{
    public class ProjectMap : ClassMapping<Project>
    {
        public ProjectMap()
        {
            Table("Projects");
            Id(project => project.ProjectId, mapper => mapper.Generator(Generators.Identity));
            Property(project => project.Name, mapper => mapper.Column("Name"));
            Property(project => project.AccessLevel, mapper => mapper.Column("AccessLevel"));
            Property(project => project.Info, mapper => mapper.Column("Info"));
            Property(project => project.LandingImageUri, mapper => mapper.Column("LandingImageUri"));
            Property(project => project.ProjectStatus, mapper => mapper.Column("ProjectStatus"));
            Property(project => project.ProjectType, mapper => mapper.Column("ProjectType"));
            Property(project => project.ProjectManagementSystemId, mapper => mapper.Column("ProjectManagementSystemId"));
            Property(project => project.VersionControlSystemId, mapper => mapper.Column("VersionControlSystemId"));
            Set(project => project.Screenshots, mapper =>
            {
                mapper.Cascade(Cascade.All);
                mapper.Table("Screenshots");
            });
            Set(project => project.ProjectDevelopers, mapper =>
            {
                mapper.Cascade(Cascade.All);
                mapper.Table("ProjectUsers");
            });
        }
    }
}