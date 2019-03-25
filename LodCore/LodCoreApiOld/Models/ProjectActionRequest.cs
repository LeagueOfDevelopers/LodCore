using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LodCoreLibraryOld.Common;
using LodCoreLibraryOld.Domain.ProjectManagment;

namespace LodCoreApiOld.Models
{
    public class ProjectActionRequest
    {
        [MaxLength(50)] [Required] public string Name { get; set; }

        [Required] public ProjectType[] ProjectTypes { get; set; }

        [MaxLength(9000)] public string Info { get; set; }

        [Required] public ProjectStatus ProjectStatus { get; set; }

        public LodCoreLibraryOld.Common.Image LandingImage { get; set; }

        public LodCoreLibraryOld.Common.Image[] Screenshots { get; set; }

        [ProjectLinksValidation] public ProjectLink[] Links { get; set; }

        public Uri[] LinksToGithubRepositories { get; set; }
    }

    public class ProjectLinksValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var links = value as ProjectLink[];

            if (value != null && !links.ToList().TrueForAll(link => link.Name.Length <= 50))
                return false;

            return true;
        }
    }
}