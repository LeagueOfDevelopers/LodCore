namespace LodCoreLibraryOld.QueryService.Views.ProjectView
{
    public class ProjectLinkView
    {
        public ProjectLinkView(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }

        public string Name { get; }
        public string Uri { get; }
    }
}