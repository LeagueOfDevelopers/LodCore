using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Journalist;

namespace Common
{
    public class Password
    {
        public Password(string pass)
        {
            if (Regex.IsMatch(pass, "^.{6,18}$"))
            {
                var md5Hasher = MD5.Create();

                var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(pass));

                var sBuilder = new StringBuilder();

                for (var i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                Pass = sBuilder.ToString();
            }
            else
            {
                throw new ArgumentException("Password does not satisfy security requirements");
            }
        }

        public Password()
        {
        }

        public virtual string Pass { get; protected set; }

        public static Password FromHash(string passwordHash)
        {
            Require.NotEmpty(passwordHash, nameof(passwordHash));
            return new Password { Pass = passwordHash };
        }

        protected bool Equals(Password other)
        {
            return string.Equals(Pass, other.Pass);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Password) obj);
        }

        public override int GetHashCode()
        {
            return (Pass != null ? Pass.GetHashCode() : 0);
        }
    }
}