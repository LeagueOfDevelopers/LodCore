namespace LodCoreApi.Models.AuthorizationModels
{
    public class LoginResponseModel
    {
        public LoginResponseModel(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }
}
