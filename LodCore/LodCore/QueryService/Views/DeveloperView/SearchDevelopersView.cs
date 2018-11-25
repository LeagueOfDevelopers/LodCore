using LodCore.Domain.UserManagement;
using LodCore.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LodCore.QueryService.Views.DeveloperView
{
    public class SearchDevelopersView
    {
        public SearchDevelopersView(List<AccountDto> developers)
        {
            Developers = developers.Select(d => new MinDeveloperView(d));
            _rawResult = developers;
        }

        public IEnumerable<MinDeveloperView> Developers { get; private set; }

        public void FilterResult(AccountRole callingUser)
        {
            if (callingUser == AccountRole.Administrator)
                Developers = _rawResult.Where(d => d.ConfirmationStatus != ConfirmationStatus.Unconfirmed)
                    .Select(d => new MinDeveloperView(d));
            else
                Developers = _rawResult.Where(d => d.ConfirmationStatus == ConfirmationStatus.FullyConfirmed &&
                !d.IsHidden).Select(d => new MinDeveloperView(d));
        }

        private IEnumerable<AccountDto> _rawResult;
    }
}
