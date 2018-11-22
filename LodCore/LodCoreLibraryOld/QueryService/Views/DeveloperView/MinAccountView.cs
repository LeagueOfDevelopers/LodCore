using LodCoreLibrary.QueryService.Views.ProjectView;
using LodCoreLibraryOld.QueryService.DTOs;
using LodCoreLibraryOld.QueryService.Views.ProjectView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibraryOld.QueryService.Views.DeveloperView
{
    public class MinAccountView
    {
        public MinAccountView(AccountDto accountDto)
        {
            Id = accountDto.UserId;
            Firstname = accountDto.Firstname;
            Lastname = accountDto.Lastname;
            Specialization = accountDto.Specialization;
            Avatar = new ImageView(accountDto.BigPhotoUri, accountDto.SmallPhotoUri);
        }

        public int Id { get; }
        public string Firstname { get; }
        public string Lastname { get; }
        public string Specialization { get; }
        public ImageView Avatar { get; }
    }
}
