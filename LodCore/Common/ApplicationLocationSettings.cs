namespace Common
{
    public class ApplicationLocationSettings
    {
        public ApplicationLocationSettings(string backendAdress)
        {
            BackendAdress = backendAdress;
        }

        public string BackendAdress { get; private set; }
    }
}