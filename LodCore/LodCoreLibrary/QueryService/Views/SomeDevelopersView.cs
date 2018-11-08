using LodCoreLibrary.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Views
{
    public class SomeDevelopersView
    {
        public SomeDevelopersView(IEnumerable<AccountDto> developers, int allDevelopersCount)
        {
            var result = new List<MinDeveloperView>();
            developers.ToList().ForEach(d => result.Add(new MinDeveloperView(d)));
            Developers = result;

            AllDevelopersCount = allDevelopersCount;
        }

        public IEnumerable<MinDeveloperView> Developers { get; }
        public int AllDevelopersCount { get; }
    }
}
