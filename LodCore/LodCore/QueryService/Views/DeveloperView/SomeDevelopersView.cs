﻿using LodCore.Domain.UserManagement;
using LodCore.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCore.QueryService.Views.DeveloperView
{
    public class SomeDevelopersView
    {
        public SomeDevelopersView(IEnumerable<AccountDto> developers, int allDevelopersCount)
        {
            Developers = developers.Select(d => new MinDeveloperView(d));

            AllDevelopersCount = allDevelopersCount;
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

        public SomeDevelopersView Take(int count, int offset)
        {
            return new SomeDevelopersView(
                _rawResult
                    .Skip(offset)
                    .Take(count),
                AllDevelopersCount);
        }

        public IEnumerable<MinDeveloperView> Developers { get; private set; }
        public int AllDevelopersCount { get; private set; }

        private IEnumerable<AccountDto> _rawResult;
    }
}
