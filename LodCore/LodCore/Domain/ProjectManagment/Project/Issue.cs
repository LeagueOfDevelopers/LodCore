using Journalist;

namespace LodCore.Domain.ProjectManagment
{
    public class Issue
    {
        public Issue(string header, string descripton, IssueType issueType)
        {
            Require.NotEmpty(header, nameof(header));
            Require.NotNull(descripton, nameof(descripton));

            Header = header;
            Descripton = descripton;
            IssueType = issueType;
        }

        public string Header { get; }

        public string Descripton { get; }

        public IssueType IssueType { get; }
    }
}