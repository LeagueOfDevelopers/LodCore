using System.ComponentModel.DataAnnotations;

namespace LodCoreApiOld.Models
{
    public class Credentials
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}