using System.Collections.Generic;
using System.Security.Principal;
using Journalist.Extensions;
using ProjectManagement.Domain;

namespace Gateways
{
    public static class IssueMapper
    {
        public static Issue ToLodIssue(this Redmine.Net.Api.Types.Issue issue)
        {
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

        public static Redmine.Net.Api.Types.Issue ToRedmineIssue(this Issue issue)
        {
            return new Redmine.Net.Api.Types.Issue
            {
                Subject = issue.Header,
                Description = issue.Descripton
            };
        }
        private static readonly Dictionary<int, IssueType> RedmineTrackerIdToIssueType = new Dictionary<int, IssueType>
        {
            {1, IssueType.Bug },
            {2, IssueType.Task },
            {3, IssueType.ChangeRequest },
            {5, IssueType.Research }
        };  
    }
}