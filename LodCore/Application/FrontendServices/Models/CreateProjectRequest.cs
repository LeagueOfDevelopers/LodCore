using System;
using System.ComponentModel.DataAnnotations;
using Common;
using ProjectManagement.Domain;

namespace FrontendServices.Models
{
    public class CreateProjectRequest
    {
        [MaxLength(25)]
        public string Name { get; set; }

        public ProjectType[] ProjectTypes { get; set; }

        [MaxLength(500)]
        public string Info { get; set; }

        public AccessLevel AccessLevel { get; set; }
        
        public Uri LandingImageUri { get; set; }

    }
}