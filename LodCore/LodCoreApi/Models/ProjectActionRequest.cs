using LodCoreLibrary.Domain.ProjectManagment;
using System;
using System.ComponentModel.DataAnnotations;

namespace LodCoreApi.Models
{
    public class ProjectActionRequest
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        public ProjectType[] ProjectTypes { get; set; }

        [MaxLength(9000)]
        public string Info { get; set; }
        
        [Required]
        public ProjectStatus ProjectStatus { get; set; }

        public LodCoreLibrary.Common.Image LandingImage { get; set; }

        public LodCoreLibrary.Common.Image[] Screenshots { get; set; }

        public Uri[] LinksToGithubRepositories { get; set; }
    }
}