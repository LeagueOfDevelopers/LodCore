using LodCore.QueryService.Views.DeveloperView;
using System;
using System.Collections.Generic;
using System.Text;

namespace LodCore.QueryService.Queries.ProjectQuery
{
    public class SearchDevelopersQuery : IQuery<SearchDevelopersView>
    {
        public SearchDevelopersQuery(string searchString)
        {
            SearchString = searchString;
        }

        public string SearchString { get; }
        public string Sql { get; }
    }
}
