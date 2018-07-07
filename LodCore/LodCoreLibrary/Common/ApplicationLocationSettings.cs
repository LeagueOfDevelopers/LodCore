namespace LodCoreLibrary.Common
{
    public class ApplicationLocationSettings
    {
        public ApplicationLocationSettings(string backendAdress, string frontendAdress)
        {
            BackendAdress = backendAdress;
            FrontendAdress = frontendAdress;
        }

        public string BackendAdress { get; private set; }
        public string FrontendAdress { get; private set; }
    }
}