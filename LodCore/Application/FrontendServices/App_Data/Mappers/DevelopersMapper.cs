using FrontendServices.Models;
using Journalist;
using ProjectManagement.Application;
using UserManagement.Domain;

namespace FrontendServices.App_Data.Mappers
{
    public class DevelopersMapper
    {
        public DevelopersMapper(IUserRoleAnalyzer userRoleAnalyzer)
        {
            Require.NotNull(userRoleAnalyzer, nameof(userRoleAnalyzer));

            _userRoleAnalyzer = userRoleAnalyzer;
        }

        public IndexPageDeveloper FromDomainEntity(Account account)
        {
            Require.NotNull(account, nameof(account));

            var role = _userRoleAnalyzer.GetUserCommonRole(account.UserId);
            return new IndexPageDeveloper(
                account.UserId,
                account.Firstname,
                account.Lastname,
                account.Profile.SmallPictureUri,
                role);
        }

        private readonly IUserRoleAnalyzer _userRoleAnalyzer;
    }
}