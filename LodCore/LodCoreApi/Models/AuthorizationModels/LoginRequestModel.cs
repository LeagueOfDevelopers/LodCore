using System.ComponentModel.DataAnnotations;

namespace LodCoreApi.Models.AuthorizationModels
{
    public class LoginRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}