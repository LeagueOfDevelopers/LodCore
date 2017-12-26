using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain;

namespace EventHandlers
{
    public interface IMailValidationHandler
    {
        void ValidateMail(MailValidationRequest mailValidationRequest);
    }
}
