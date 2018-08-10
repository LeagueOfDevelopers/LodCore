using LodCoreLibrary.Common;
using LodCoreLibrary.Domain.ProjectManagment;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace LodCoreLibrary.Infrastructure.DataAccess.Mappings
{
    public class ProjectMap : ClassMapping<Project>
    {
        public ProjectMap()
        {
            Table("Projects");
            Id(project => project.ProjectId, mapper => mapper.Generator(Generators.Identity));
            Property(project => project.Name, mapper => mapper.Column("Name"));
            Property(project => project.Info, mapper => mapper.Column("Info"));
            Component(project => project.LandingImage,
                mapper =>
                {
                    mapper.Lazy(false);
                    mapper.Property(
                        image => image.BigPhotoUri,
                        propertyMapper => propertyMapper.Column("BigPhotoUri"));
                    mapper.Property(
                        image => image.SmallPhotoUri,
                        propertyMapper => propertyMapper.Column("SmallPhotoUri"));
                });
            Property(project => project.ProjectStatus, mapper => mapper.Column("ProjectStatus"));
            Set(project => project.ProjectTypes, mapper =>
            {
                mapper.Table("ProjectTypes");
                mapper.Cascade(Cascade.All);
            });
            Set(project => project.Screenshots, mapper =>
            {
                mapper.Cascade(Cascade.All);
                mapper.Table("Screenshots");
            });
            Set(project => project.Links, mapper =>
            {
                mapper.Table("ProjectLinks");
                mapper.Cascade(Cascade.All);
            }, elementRelation =>
            {
                elementRelation.Component(component =>
                {
                    component.Lazy(false);
                    component.Property(p => p.Name);
                    component.Property(p => p.Uri);
                });
            });
            Set(
                membership => membership.ProjectMemberships,
                mapper =>
                {
                    mapper.Cascade(Cascade.All);
                    mapper.Key(m => m.Column("ProjectId"));
                },
                action => action.OneToMany());
            Set(project => project.LinksToGithubRepositories, mapper =>
            {
                mapper.Cascade(Cascade.All);
                mapper.Table("GithubRepositoriesLinks");
            });
        }
    }
}