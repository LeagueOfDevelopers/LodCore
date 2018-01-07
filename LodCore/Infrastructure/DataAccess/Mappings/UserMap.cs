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
            Property(user => user.RegistrationTime, mapper => mapper.Column("RegistrationTime"));
            Property(user => user.IsHidden, mapper => mapper.Column("IsHidden"));
            Property(user => user.Password, mapper =>
            {
                mapper.Column("Password");
                mapper.Type<PasswordType>();
            });

            Component(x => x.Profile, m =>
            {
                m.Component(
                    account => account.Image,
                    mapper =>
                    {
                        mapper.Lazy(false);
                        mapper.Property(image => image.BigPhotoUri, propertyMapper => propertyMapper.Column("BigPhotoUri"));
                        mapper.Property(image => image.SmallPhotoUri, propertyMapper => propertyMapper.Column("SmallPhotoUri"));
                    });
                m.Property(profile => profile.InstituteName, mapper => mapper.Column("InstituteName"));
                m.Property(profile => profile.PhoneNumber, mapper => mapper.Column("PhoneNumber"));
                m.Property(profile => profile.Specialization, mapper => mapper.Column("Specialization"));
                m.Property(profile => profile.StudentAccessionYear, mapper => mapper.Column("StudentAccessionYear"));
                m.Property(profile => profile.StudyingDirection, mapper => mapper.Column("StudyingDirection"));
                m.Property(profile => profile.VkProfileUri, mapper => mapper.Column("VkProfileUri"));
                m.Property(profile => profile.LinkToGithubProfile, mapper => mapper.Column("GitHubProfileUri"));
            });
        }
    }
}