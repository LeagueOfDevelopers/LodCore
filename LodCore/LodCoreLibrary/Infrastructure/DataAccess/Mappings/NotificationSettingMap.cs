using LodCoreLibrary.Domain.UserManagement;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace LodCoreLibrary.Infrastructure.DataAccess.Mappings
{
    public class NotificationSettingMap : ClassMapping<NotificationSetting>
    {
        public NotificationSettingMap()
        {
            Table("NotificationSettings");

            Id(setting => setting.SettingId, mapper =>
            {
                mapper.Column("SettingId");
                mapper.Generator(Generators.Identity);
            });

            Property(setting => setting.UserId, mapper => mapper.Column("UserId"));
            Property(setting => setting.NotificationType, mapper => mapper.Column("NotificationType"));
            Property(setting => setting.Value, mapper => mapper.Column("Value"));
        }
    }
}