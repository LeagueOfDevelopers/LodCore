using LodCoreLibrary.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Views
{
    public class MinDeveloperView
    {
        public MinDeveloperView(AccountDto accountDto)
        {
            Firstname = accountDto.Firstname;
            Lastname = accountDto.Lastname;
            Specialization = accountDto.Specialization;
            RegistrationTime = accountDto.RegistrationTime;
            NumberOfProjects = accountDto.ProjectMemberships.Count;
            VkProfileUri = accountDto.VkProfileUri;
        }

        public string Firstname { get; }
        public string Lastname { get; }
        public string Specialization { get; }
        public DateTimeOffset RegistrationTime { get; }
        public int NumberOfProjects { get; }
        public string VkProfileUri { get; }
    }
}
