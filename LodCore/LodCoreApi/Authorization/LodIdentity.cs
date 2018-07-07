using System.Security.Principal;

namespace LodCoreApi.Authorization
{
    public class LodIdentity : IIdentity
    {
        public LodIdentity(int userId, bool isAuthenticated)
        {
            UserId = userId;
            IsAuthenticated = isAuthenticated;
        }

        public int UserId { get; }

        public static LodIdentity EmptyIdentity => new LodIdentity(0, false);

        public string Name => UserId.ToString();

        public string AuthenticationType => "Token";

        public bool IsAuthenticated { get; }
    }
}