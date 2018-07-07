using NHibernate;
using System;

namespace LodCoreLibrary.Common
{
    public interface IDatabaseSessionProvider
    {
        ISession GetCurrentSession();

        void OpenSession();

        void CloseSession();

        void DropSession();

        void ProcessInNHibernateSession(Action action);
    }
}
