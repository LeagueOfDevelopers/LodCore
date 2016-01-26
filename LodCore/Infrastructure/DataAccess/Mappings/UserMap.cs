using DataAccess.Mappings.Application;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using UserManagement.Domain;

namespace DataAccess.Mappings
{
    public class UserMap : ClassMapping<Account>
    {
        public UserMap()
        {
            Table("Accounts");
            Id(user => user.UserId, mapper => mapper.Generator(Generators.Identity));
            Property(user => user.Email, mapper =>
            {
                mapper.Column("Email");
                mapper.Unique(true);
                mapper.Type<MailAddressType>();
            });
            Property(user => user.Firstname, mapper => mapper.Column("Firstname"));
            Property(user => user.Lastname, mapper => mapper.Column("Lastname"));
            Property(user => user.Role, mapper => mapper.Column("AccountRole"));
            Property(user => user.ConfirmationStatus, mapper => mapper.Column("ConfirmationStatus"));
            Property(user => user.Password, mapper =>
            {
                mapper.Column("Password");
                mapper.Type<PasswordType>();
            });

            ManyToOne(user => user.Profile, mapper =>
            {
                mapper.Cascade(Cascade.All);
                mapper.Class(typeof (Profile));
            });
        }
    }
}