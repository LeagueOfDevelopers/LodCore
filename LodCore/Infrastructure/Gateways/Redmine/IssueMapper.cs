using System.Collections.Generic;
using System.Security.Principal;
using Journalist;
using Journalist.Extensions;
using ProjectManagement.Domain;

namespace Gateways.Redmine
{
    public static class IssueMapper
    {
        private static readonly Dictionary<int, IssueType> RedmineTrackerIdToIssueType = new Dictionary<int, IssueType>
        {
            {1, IssueType.Bug},
            {4, IssueType.Research},
            {2, IssueType.Task}
        };

        public static Issue ToLodIssue(this global::Redmine.Net.Api.Types.Issue issue)
        {
            Require.NotNull(issue, nameof(issue));

            IssueType issueType;
            var isKnownType = RedmineTrackerIdToIssueType.TryGetValue(issue.Tracker.Id, out issueType);
            if (!isKnownType)
            {
                throw new IdentityNotMappedException(
                    "Could not map IssueType for issue {0}".FormatString(issue.Id));
            }

            return new Issue(
                issue.Subject,
                issue.Description,
                issueType);
        }

        public static global::Redmine.Net.Api.Types.Issue ToRedmineIssue(this Issue issue)
        {
            Require.NotNull(issue, nameof(issue));

            return new global::Redmine.Net.Api.Types.Issue
            {
                Subject = issue.Header,
                Description = issue.Descripton
            };
        }
    }
}