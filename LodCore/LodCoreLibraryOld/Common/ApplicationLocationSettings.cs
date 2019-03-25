namespace LodCoreLibraryOld.Common
{
    public class ApplicationLocationSettings
    {
        public ApplicationLocationSettings(string backendAdress, string frontendAdress)
        {
            BackendAdress = backendAdress;
            FrontendAdress = frontendAdress;
        }

        public string BackendAdress { get; }
        public string FrontendAdress { get; }
    }
}