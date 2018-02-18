using System.ComponentModel.DataAnnotations;
using Common;
using ProjectManagement.Domain;

namespace FrontendServices.Models
{
    public class ProjectActionRequest
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        public ProjectType[] ProjectTypes { get; set; }

        [MaxLength(500)]
        public string Info { get; set; }

        [Required]
        public AccessLevel AccessLevel { get; set; }

        [Required]
        public ProjectStatus ProjectStatus { get; set; }

        public Common.Image LandingImage { get; set; }

        public Common.Image[] Screenshots { get; set; }
    }
}