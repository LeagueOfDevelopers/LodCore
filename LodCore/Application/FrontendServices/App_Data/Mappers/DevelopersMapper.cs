using FrontendServices.Models;
using Journalist;
using UserManagement.Domain;

namespace FrontendServices.App_Data.Mappers
{
    public class DevelopersMapper
    {
        public IndexPageDeveloper FromDomainEntity(Account account)
        {
            Require.NotNull(account, nameof(account));
            //todo: вычисление роли разработчика, 29 задача
            return new IndexPageDeveloper(
                account.UserId, 
                account.Firstname, 
                account.Lastname, 
                account.Profile.SmallPictureUri,
                "TODO: NOT CALCULATED NOW");
        }
    }
}