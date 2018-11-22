using Dapper;
using LodCoreLibraryOld.QueryService.DTOs;
using LodCoreLibraryOld.QueryService.Queries;
using LodCoreLibraryOld.QueryService.Queries.DeveloperQuery;
using LodCoreLibraryOld.QueryService.Views;
using LodCoreLibraryOld.QueryService.Views.DeveloperView;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibraryOld.QueryService.Handlers
{
    public class DeveloperQueryHandler : 
        IQueryHandler<GetSomeDevelopersQuery, SomeDevelopersView>,
        IQueryHandler<GetDeveloperQuery, FullDeveloperView>,
        IQueryHandler<AllAccountsQuery, AllAccountsView>
    {
        public DeveloperQueryHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SomeDevelopersView Handle(GetSomeDevelopersQuery query)
        {
            List<AccountDto> result;

            using (var connection = new SqlConnection(_connectionString))
            {
                var resultDictionary = new Dictionary<int, AccountDto>();

                result = connection.Query<AccountDto, ProjectMembershipDto, AccountDto>(query.Sql,
                    (account, projectMembership) =>
                    {
                        AccountDto accountEntry;

                        if (!resultDictionary.TryGetValue(account.UserId, out accountEntry))
                        {
                            accountEntry = account;
                            accountEntry.ProjectMemberships = new List<ProjectMembershipDto>();
                            resultDictionary.Add(accountEntry.UserId, accountEntry);
                        }

                        if (projectMembership != null && !accountEntry.ProjectMemberships.Any(m => m.ProjectId == projectMembership.ProjectId))
                            accountEntry.ProjectMemberships.Add(projectMembership);

                        return accountEntry;

                    }, splitOn: "membershipId").Distinct().ToList();
            }

            return query.FormResult(result);
        }

        public FullDeveloperView Handle(GetDeveloperQuery query)
        {
            AccountWithProjectsDto result;

            using (var connection = new SqlConnection(_connectionString))
            {
                var resultDictionary = new Dictionary<int, AccountWithProjectsDto>();

                result = connection.Query<AccountWithProjectsDto, ProjectMembershipDto, ProjectDto, AccountWithProjectsDto>(query.Sql,
                    (account, projectMembership, project) =>
                    {
                        AccountWithProjectsDto accountEntry;

                        if (!resultDictionary.TryGetValue(account.UserId, out accountEntry))
                        {
                            accountEntry = account;
                            accountEntry.Projects = new List<DeveloperProjectDto>();
                            resultDictionary.Add(accountEntry.UserId, accountEntry);
                        }

                        if (projectMembership != null && !accountEntry.Projects.Any(m => m.Membership.ProjectId == projectMembership.ProjectId))
                            accountEntry.Projects.Add(new DeveloperProjectDto(project, projectMembership));

                        return accountEntry;

                    }, splitOn: "membershipId,projectId").Distinct().ToList().First();
            }

            return new FullDeveloperView(result);
        }

        public AllAccountsView Handle(AllAccountsQuery query)
        {
            List<AccountDto> result;

            using (var connection = new SqlConnection(_connectionString))
            {
                var resultDictionary = new Dictionary<int, AccountDto>();

                result = connection.Query<AccountDto>(query.Sql).ToList();
            }

            return query.FormResult(result);
        }

        private string _connectionString;
    }
}
