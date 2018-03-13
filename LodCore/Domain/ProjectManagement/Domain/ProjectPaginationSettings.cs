namespace ProjectManagement.Domain
{
    public class ProjectPaginationSettings
    {
        public int NumberOfProjects;

        public ProjectPaginationSettings(int numberOfProjects)
        {
            NumberOfProjects = numberOfProjects;
        }
    }
}