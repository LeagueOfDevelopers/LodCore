using NHibernate;
using System;

namespace LodCoreLibraryOld.Common
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
