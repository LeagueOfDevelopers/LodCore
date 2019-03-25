namespace LodCoreApi.Models
{
    public class PasswordChangeRequest
    {
        public PasswordChangeRequest(string newPassword, string token)
        {
            NewPassword = newPassword;
            Token = token;
        }

        public string NewPassword { get; }
        public string Token { get; }
    }
}