using System;
using System.Linq.Expressions;
using UserManagement.Domain;

namespace NotificationService
{
    public interface IUsersRepository
    {
        int[] GetAllUsersIds();

        int[] GetAllAdminIds();

        int[] GetAllIdsByCriteria(Expression<Func<Account, bool>> criteria);
    }
}