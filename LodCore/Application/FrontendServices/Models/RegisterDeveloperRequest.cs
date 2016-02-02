namespace FrontendServices.Models
{
    public class RegisterDeveloperRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string VkProfileUri { get; set; }
        public string PhoneNumber { get; set; }
        public string StudyingProfile { get; set; }
        public string InstituteName { get; set; }
        public string Department { get; set; }
        public int AccessionYear { get; set; }
    }
}