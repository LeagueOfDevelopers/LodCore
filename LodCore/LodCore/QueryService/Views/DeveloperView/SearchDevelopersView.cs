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
        public SearchDevelopersView(IEnumerable<AccountDto> developers)
        {
            Developers = developers.Select(d => new MinDeveloperView(d));
            AllDevelopersCount = Developers.Count();
            _rawResult = developers;
        }

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

        public void CutOff(int count, int offset)
        {
            Developers = Developers.Skip(offset).Take(count);
        }

        public IEnumerable<MinDeveloperView> Developers { get; private set; }
        public int AllDevelopersCount { get; private set; }
        private IEnumerable<AccountDto> _rawResult;
    }
}
