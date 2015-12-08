using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Common
{
    public class Password
    {
        public Password(string pass)
        {

            if (Regex.IsMatch(pass, "^.{6,18}$"))
                Pass = MD5.Create(pass).Hash.ToString();
            else
            {
                throw new ArgumentException();
            }
        }

        public Password()
        {
            
        }

        public virtual string Pass { get; protected set; }
    }
}