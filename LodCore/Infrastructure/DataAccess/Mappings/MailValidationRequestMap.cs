using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using UserManagement.Domain;

namespace DataAccess.Mappings
{
    class MailValidationRequestMap : ClassMapping<MailValidationRequest>
    {
        public MailValidationRequestMap()
        {
            Property(model => model.UserId, mapper => mapper.Column("UserId"));
            Property(model => model.Token, mapper => mapper.Column("VerificationToken"));
        }
    }
}
