using System.ComponentModel.DataAnnotations;

namespace LodCoreApi.Models
{
    public class LoginRequest
    {
        [EmailAddress] public string Email { get; set; }

        public string Password { get; set; }
    }
}