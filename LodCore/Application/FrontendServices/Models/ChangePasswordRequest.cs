using System.ComponentModel.DataAnnotations;
using UserManagement.Domain;

namespace FrontendServices.Models
{
    public class ChangePasswordRequest
    {
        [MaxLength(18)]
        public string NewPassword { get; set; }

        public int Id { get; set; }
    }
}