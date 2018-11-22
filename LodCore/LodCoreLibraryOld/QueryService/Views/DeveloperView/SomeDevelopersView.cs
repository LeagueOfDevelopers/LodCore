﻿using LodCoreLibraryOld.Domain.UserManagement;
using LodCoreLibraryOld.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibraryOld.QueryService.Views.DeveloperView
{
    public class SomeDevelopersView
    {
        public SomeDevelopersView(IEnumerable<AccountDto> developers, int allDevelopersCount)
        {
            Developers = developers.ToList().Select(d => new MinDeveloperView(d));

            AllDevelopersCount = allDevelopersCount;
            _rawResult = developers;
        }

        public IEnumerable<MinDeveloperView> Developers { get; private set; }
        public int AllDevelopersCount { get; }

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
