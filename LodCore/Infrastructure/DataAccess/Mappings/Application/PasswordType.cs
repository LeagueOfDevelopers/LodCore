using System;
using System.Data;
using Common;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace DataAccess.Mappings.Application
{
    public class PasswordType : IUserType
    {
        public new bool Equals(object x, object y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.GetType() == y.GetType();
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var property0 = NHibernateUtil.String.NullSafeGet(rs, names[0]);

            if (property0 == null)
            {
                return null;
            }

            Password password = new Password(property0.ToString());

            return password;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                var state = (Password)value;
                ((IDataParameter)cmd.Parameters[index]).Value = state.GetType().Name;
            }
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public SqlType[] SqlTypes { get { return new[] { NHibernateUtil.String.SqlType }; } }
        public Type ReturnedType { get { return typeof(Password); } }
        public bool IsMutable { get { return false; } }
    }
}