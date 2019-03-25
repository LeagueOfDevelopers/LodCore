using System.Collections.Generic;
using System.Linq;
using LodCore.Common;
using LodCore.Domain.UserManagement;
using LodCore.QueryService.DTOs;

namespace LodCore.QueryService.Views.DeveloperView
{
    public class AllAccountsView
    {
        private readonly IEnumerable<AccountDto> _rawResult;

        public AllAccountsView(IEnumerable<AccountDto> developers)
        {
            Developers = developers.Select(d => new MinAccountView(d));
            _rawResult = developers;
        }

        public IEnumerable<MinAccountView> Developers { get; private set; }

        public void SelectRandomDevelopers(int count, AccountRole callingUser)
        {
            if (callingUser == AccountRole.Administrator)
                Developers = _rawResult.Where(d => d.ConfirmationStatus != ConfirmationStatus.Unconfirmed)
                    .Select(d => new MinAccountView(d)).GetRandom(count);
            else
                Developers = _rawResult.Where(d => d.ConfirmationStatus == ConfirmationStatus.FullyConfirmed &&
                                                   !d.IsHidden).Select(d => new MinAccountView(d)).GetRandom(count);
        }
    }
}