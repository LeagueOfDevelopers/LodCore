using LodCore.Domain.UserManagement;

namespace LodCore.Infrastructure.DataAccess.Repositories
{
    public class PasswordChangeRequestRepository : IPasswordChangeRequestRepository
    {
        public void SavePasswordChangeRequest(PasswordChangeRequest request)
        {
            /*
            Require.NotNull(request, nameof(request));

            var session = _sessionProvider.GetCurrentSession();

            session.Save(request);*/
        }

//    No needness in the function because of queue
        public PasswordChangeRequest GetPasswordChangeRequest(string token)
        {
            /*
            Require.NotNull(token, nameof(token));

            var session = _sessionProvider.GetCurrentSession();

            return session.Get<PasswordChangeRequest>(token);*/
            return null;
        }

        public PasswordChangeRequest GetPasswordChangeRequest(int userId)
        {
            /*
            Require.Positive(userId, nameof(userId));

            var session = _sessionProvider.GetCurrentSession();

            return session.QueryOver<PasswordChangeRequest>().Where(request => request.UserId == userId).List().SingleOrDefault();
            */
            return null;
        }

        public void DeletePasswordChangeRequest(PasswordChangeRequest request)
        {
            /*
            Require.NotNull(request, nameof(request));

            var session = _sessionProvider.GetCurrentSession();

            session.Delete(request);*/
        }
    }
}