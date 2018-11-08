using LodCoreLibrary.Domain.UserManagement;
using LodCoreLibrary.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Views
{
    public class FullDeveloperView
    {
        public FullDeveloperView(AccountWithProjectsDto accountDto)
        {
            Firstname = accountDto.Firstname;
            Lastname = accountDto.Lastname;
            Email = accountDto.Email;
            IsHidden = accountDto.IsHidden;
            AccountRole = accountDto.AccountRole;
            ConfirmationStatus = accountDto.ConfirmationStatus;
            IsOauthRegistered = accountDto.IsOauthRegistered;
            RegistrationTime = accountDto.RegistrationTime;
            Avatar = new ImageView(accountDto.BigPhotoUri, accountDto.SmallPhotoUri);
            VkProfileUri = accountDto.VkProfileUri;
            GithubProfileUri = accountDto.GithubProfileUri;
            PhoneNumber = accountDto.PhoneNumber;
            StudentAccessionYear = accountDto.StudentAccessionYear;
            IsGraduated = accountDto.IsGraduated;
            StudyingDirection = accountDto.StudyingDirection;
            InstituteName = accountDto.InstituteName;
            Specialization = accountDto.Specialization;

            Projects = new List<DeveloperProjectView>();
            accountDto.Projects.ForEach(p => Projects.Add(new DeveloperProjectView(p.Project, p.Membership)));
        }

        public string Firstname { get; }
        public string Lastname { get; }
        public string Email { get; }
        public bool IsHidden { get; }
        public AccountRole AccountRole { get; }
        public ConfirmationStatus ConfirmationStatus { get; }
        public bool IsOauthRegistered { get; }
        public DateTimeOffset RegistrationTime { get; }
        public ImageView Avatar { get; }
        public string VkProfileUri { get; }
        public string GithubProfileUri { get; }
        public string PhoneNumber { get; }
        public int StudentAccessionYear { get; }
        public bool IsGraduated { get; }
        public string StudyingDirection { get; }
        public string InstituteName { get; }
        public string Specialization { get; }
        public List<DeveloperProjectView> Projects { get; }

        public GuestFullDeveloperView GetGuestView()
        {
            return new GuestFullDeveloperView(this);
        }
    }
}
