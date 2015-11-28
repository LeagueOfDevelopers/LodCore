using System;
using System.Threading.Tasks;

namespace UserPresentaton
{
    public interface IUserPresentationManager
    {
        Task<UserSettings> GetUserSettings(Guid userId);

        Task UpdateUserSettings(UserSettings userSettings);
    }
}