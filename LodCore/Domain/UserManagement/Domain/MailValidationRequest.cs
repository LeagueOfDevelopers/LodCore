namespace UserManagement.Domain
{
    public class MailValidationRequest
    {
        public MailValidationRequest(int userId, string token)
        {
            UserId = userId;
            Token = token;
        }

        protected MailValidationRequest()
        {
        }

        public virtual int UserId { get; set; }
        public virtual string Token { get; set; }
    }
}