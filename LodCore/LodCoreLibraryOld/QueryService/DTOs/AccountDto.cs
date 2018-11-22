using LodCoreLibraryOld.Domain.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibraryOld.QueryService.DTOs
{
    public class AccountDto
    {
        public AccountDto()
        {
        }

        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsHidden { get; set; }
        public AccountRole AccountRole { get; set; }
        public ConfirmationStatus ConfirmationStatus { get; set; }
        public bool IsOauthRegistered { get; set; }
        public DateTimeOffset RegistrationTime { get; set; }
        public string BigPhotoUri { get; set; }
        public string SmallPhotoUri { get; set; }
        public string VkProfileUri { get; set; }
        public string GithubProfileUri { get; set; }
        public string PhoneNumber { get; set; }
        public int StudentAccessionYear { get; set; }
        public bool IsGraduated { get; set; }
        public string StudyingDirection { get; set; }
        public string InstituteName { get; set; }
        public string Specialization { get; set; }
        public List<ProjectMembershipDto> ProjectMemberships { get; set; }
    }
}
