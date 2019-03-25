using System.Collections.Generic;
using System.Linq;
using LodCore.Domain.UserManagement;
using LodCore.QueryService.DTOs;

namespace LodCore.QueryService.Views.DeveloperView
{
    public class SomeDevelopersView
    {
        private readonly IEnumerable<AccountDto> _rawResult;

        public SomeDevelopersView(IEnumerable<AccountDto> developers, int allDevelopersCount)
        {
            Developers = developers.Select(d => new MinDeveloperView(d));

            AllDevelopersCount = allDevelopersCount;
            _rawResult = developers;
        }

        public IEnumerable<MinDeveloperView> Developers { get; private set; }
        public int AllDevelopersCount { get; private set; }

        public void FilterResult(AccountRole callingUser)
        {
            if (callingUser == AccountRole.Administrator)
                Developers = _rawResult.Where(d => d.ConfirmationStatus != ConfirmationStatus.Unconfirmed)
                    .Select(d => new MinDeveloperView(d));
            else
                Developers = _rawResult.Where(d => d.ConfirmationStatus == ConfirmationStatus.FullyConfirmed &&
                                                   !d.IsHidden).Select(d => new MinDeveloperView(d));
            AllDevelopersCount = Developers.Count();
        }

        public SomeDevelopersView Take(int count, int offset)
        {
            return new SomeDevelopersView(
                _rawResult
                    .Skip(offset)
                    .Take(count),
                AllDevelopersCount);
        }
    }
}