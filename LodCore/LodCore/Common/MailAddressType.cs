using System;
using System.Data;
using System.Net.Mail;

namespace LodCore.Common
{
    public class MailAddressType //: IUserType
    {
        public Type ReturnedType => typeof(MailAddress);

        public bool IsMutable => false;

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
            /* TODO: Fix it
            var property0 = NHibernateUtil.String.NullSafeGet(rs, names[0]);

            if (property0 == null)
            {
                return null;
            }

            var mailAddress = new MailAddress(property0.ToString());

            return mailAddress;*/
            return null;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                ((IDataParameter) cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                var state = (MailAddress) value;
                ((IDataParameter) cmd.Parameters[index]).Value = state.Address;
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
    }
}