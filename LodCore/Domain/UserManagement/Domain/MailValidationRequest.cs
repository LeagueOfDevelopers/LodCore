using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain
{
    public class MailValidationRequest
    {
        public virtual int UserId { get; private set; }
        public virtual string Token { get; private set; }

        public MailValidationRequest(int userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }
}
