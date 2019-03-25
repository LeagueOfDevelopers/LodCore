using System;
using LodCoreLibrary.QueryService.Views.ProjectView;
using LodCoreLibraryOld.QueryService.DTOs;

namespace LodCoreLibraryOld.QueryService.Views.DeveloperView
{
    public class MinDeveloperView
    {
        public MinDeveloperView(AccountDto accountDto)
        {
            Id = accountDto.UserId;
            Firstname = accountDto.Firstname;
            Lastname = accountDto.Lastname;
            Specialization = accountDto.Specialization;
            RegistrationTime = accountDto.RegistrationTime;
            Avatar = new ImageView(accountDto.BigPhotoUri, accountDto.SmallPhotoUri);
            NumberOfProjects = accountDto.ProjectMemberships.Count;
            VkProfileUri = accountDto.VkProfileUri;
        }

        public int Id { get; }
        public string Firstname { get; }
        public string Lastname { get; }
        public string Specialization { get; }
        public DateTimeOffset RegistrationTime { get; }
        public ImageView Avatar { get; }
        public int NumberOfProjects { get; }
        public string VkProfileUri { get; }
    }
}