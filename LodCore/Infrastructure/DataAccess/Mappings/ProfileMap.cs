using NHibernate.Mapping.ByCode.Conformist;
using UserManagement.Domain;

namespace DataAccess.Mappings
{
    public class ProfileMap : ClassMapping<Profile>
    {
        public ProfileMap()
        {
            Table("Profiles");
            Id(user => user.UserId);
            Property(user => user.PhoneNumber, mapper => mapper.Column("PhoneNumber"));
            Property(user => user.BigPhotoUri, mapper => mapper.Column("BigPhotoUri"));
            Property(user => user.SmallPictureUri, mapper => mapper.Column("SmallPictureUri"));
            Property(user => user.VkProfileUri, mapper => mapper.Column("VkProfileUri"));
            Property(user => user.InstituteName, mapper => mapper.Column("InstituteName"));
            Property(user => user.Specialization, mapper => mapper.Column("Specialization"));
            Property(user => user.RegistrationTime, mapper => mapper.Column("RegistrationTime"));
            Property(user => user.StudyingDirection, mapper => mapper.Column("StudyingDirection"));
            Property(user => user.StudentAccessionYear, mapper => mapper.Column("AccessionYear"));
        }
    }
}