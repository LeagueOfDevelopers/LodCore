using LodCoreLibrary.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Views
{
    public class AllDevelopersView
    {
        public AllDevelopersView(IEnumerable<AccountDto> developers)
        {
            var result = new List<MinDeveloperView>();
            developers.ToList().ForEach(d => result.Add(new MinDeveloperView(d)));
            Developers = result;
        }

        public IEnumerable<MinDeveloperView> Developers { get; }
    }
}
