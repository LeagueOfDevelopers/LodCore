namespace ProjectManagement.Domain
{
    public class IssuePaginationSettings
    {
        public int NumberOfIssues;

        public IssuePaginationSettings(int numberOfIssues)
        {
            NumberOfIssues = numberOfIssues;
        }
    }
}