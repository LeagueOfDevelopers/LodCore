using LodCoreLibraryOld.Domain.UserManagement;
using NHibernate.Mapping.ByCode.Conformist;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Mappings
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