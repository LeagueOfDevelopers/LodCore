using Journalist;

namespace LodCoreLibrary.Domain.ProjectManagment
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

        public string Header { get; private set; }

        public string Descripton { get; private set; }

        public IssueType IssueType { get; private set; }
    }
}