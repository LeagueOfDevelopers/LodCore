﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using LodCore.QueryService.DTOs;
using LodCore.QueryService.Queries.DeveloperQuery;
using LodCore.QueryService.Views.DeveloperView;
using MySql.Data.MySqlClient;

namespace LodCore.QueryService.Handlers
{
    public class DeveloperQueryHandler :
        IQueryHandler<GetSomeDevelopersQuery, SomeDevelopersView>,
        IQueryHandler<GetDeveloperQuery, FullDeveloperView>,
        IQueryHandler<AllAccountsQuery, AllAccountsView>,
        IQueryHandler<SearchDevelopersQuery, SearchDevelopersView>
    {
        private readonly string _connectionString;

        public DeveloperQueryHandler(string connectionString)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _connectionString = connectionString;
        }

        public AllAccountsView Handle(AllAccountsQuery query)
        {
            List<AccountDto> result;

            using (var connection = new MySqlConnection(_connectionString))
            {
                var resultDictionary = new Dictionary<int, AccountDto>();

                result = connection.Query<AccountDto>(query.Sql).ToList();
            }

            return query.FormResult(result);
        }

        public FullDeveloperView Handle(GetDeveloperQuery query)
        {
            AccountWithProjectsDto result;

            using (var connection = new MySqlConnection(_connectionString))
            {
                var resultDictionary = new Dictionary<int, AccountWithProjectsDto>();

                result = connection
                    .Query<AccountWithProjectsDto, ProjectMembershipDto, ProjectDto, AccountWithProjectsDto>(query.Sql,
                        (account, projectMembership, project) =>
                        {
                            AccountWithProjectsDto accountEntry;

                            if (!resultDictionary.TryGetValue(account.UserId, out accountEntry))
                            {
                                accountEntry = account;
                                accountEntry.Projects = new List<DeveloperProjectDto>();
                                resultDictionary.Add(accountEntry.UserId, accountEntry);
                            }

                            if (projectMembership != null && !accountEntry.Projects.Any(m =>
                                    m.Membership.ProjectId == projectMembership.ProjectId))
                                accountEntry.Projects.Add(new DeveloperProjectDto(project, projectMembership));

                            return accountEntry;
                        }, splitOn: "membershipId,projectId").Distinct().ToList().First();
            }

            return new FullDeveloperView(result);
        }

        public SomeDevelopersView Handle(GetSomeDevelopersQuery query)
        {
            List<AccountDto> result;

            using (var connection = new MySqlConnection(_connectionString))
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

                        if (projectMembership != null &&
                            !accountEntry.ProjectMemberships.Any(m => m.ProjectId == projectMembership.ProjectId))
                            accountEntry.ProjectMemberships.Add(projectMembership);

                        return accountEntry;
                    }, splitOn: "membershipId").Distinct().ToList();
            }

            return query.FormResult(result);
        }

        public SearchDevelopersView Handle(SearchDevelopersQuery query)
        {
            List<AccountDto> result;

            using (var connection = new MySqlConnection(_connectionString))
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

                        if (projectMembership != null &&
                            !accountEntry.ProjectMemberships.Any(m => m.ProjectId == projectMembership.ProjectId))
                            accountEntry.ProjectMemberships.Add(projectMembership);

                        return accountEntry;
                    }, splitOn: "MembershipId").Distinct().ToList();
            }

            return new SearchDevelopersView(SearchDevelopers(query.SearchString, result));
        }

        private IEnumerable<AccountDto> SearchDevelopers(string searchString, IEnumerable<AccountDto> accounts)
        {
            var words = searchString
                .Split(' ', ',', ';')
                .Select(w => w.ToLower())
                .ToArray();

            return accounts.Where(ac =>
            {
                foreach (var word in words)
                    if (ac.Firstname != null && ac.Firstname.ToLower().Contains(word)
                        || ac.Lastname != null && ac.Lastname.ToLower().Contains(word)
                        || ac.Email != null && ac.Email.ToLower().Contains(word)
                    )
                        return true;
                return false;
            });
        }
    }
}