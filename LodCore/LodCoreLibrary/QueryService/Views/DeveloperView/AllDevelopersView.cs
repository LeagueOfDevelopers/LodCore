using LodCoreLibrary.Common;
using LodCoreLibrary.Domain.UserManagement;
using LodCoreLibrary.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Views.DeveloperView
{
    public class AllDevelopersView
    {
        public AllDevelopersView(IEnumerable<AccountDto> developers)
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

        private IEnumerable<AccountDto> _rawResult;
    }
}
