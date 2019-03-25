using System;
using System.Collections.Generic;
using LodCore.Domain.UserManagement;
using LodCore.QueryService.Views.ProjectView;

namespace LodCore.QueryService.Views.DeveloperView
{
    public class GuestFullDeveloperView
    {
        public GuestFullDeveloperView(FullDeveloperView fullDeveloperView)
        {
            Firstname = fullDeveloperView.Firstname;
            Lastname = fullDeveloperView.Lastname;
            IsHidden = fullDeveloperView.IsHidden;
            AccountRole = fullDeveloperView.AccountRole;
            ConfirmationStatus = fullDeveloperView.ConfirmationStatus;
            RegistrationTime = fullDeveloperView.RegistrationTime;
            Avatar = new ImageView(fullDeveloperView.Avatar.BigPhotoUri, fullDeveloperView.Avatar.SmallPhotoUri);
            VkProfileUri = fullDeveloperView.VkProfileUri;
            GithubProfileUri = fullDeveloperView.GithubProfileUri;
            StudentAccessionYear = fullDeveloperView.StudentAccessionYear;
            IsGraduated = fullDeveloperView.IsGraduated;
            StudyingDirection = fullDeveloperView.StudyingDirection;
            InstituteName = fullDeveloperView.InstituteName;
            Specialization = fullDeveloperView.Specialization;
            Projects = fullDeveloperView.Projects;
        }

        public string Firstname { get; }
        public string Lastname { get; }
        public bool IsHidden { get; }
        public AccountRole AccountRole { get; }
        public ConfirmationStatus ConfirmationStatus { get; }
        public DateTimeOffset RegistrationTime { get; }
        public ImageView Avatar { get; }
        public string VkProfileUri { get; }
        public string GithubProfileUri { get; }
        public int StudentAccessionYear { get; }
        public bool IsGraduated { get; }
        public string StudyingDirection { get; }
        public string InstituteName { get; }
        public string Specialization { get; }
        public List<DeveloperProjectView> Projects { get; }
    }
}