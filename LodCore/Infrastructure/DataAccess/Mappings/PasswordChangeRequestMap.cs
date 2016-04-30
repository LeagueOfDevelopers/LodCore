using NHibernate.Mapping.ByCode.Conformist;
using UserManagement.Domain;

namespace DataAccess.Mappings
{
    internal class PasswordChangeRequestMap : ClassMapping<PasswordChangeRequest>
    {
        public PasswordChangeRequestMap()
        {
            Id(model => model.Token, mapper => mapper.Column("ResetToken"));
            Property(model => model.UserId, mapper => mapper.Column("UserId"));
        }
    }

}