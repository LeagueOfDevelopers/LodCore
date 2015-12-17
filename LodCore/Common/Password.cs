using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public class Password
    {
        public Password(string pass)
        {

            if (Regex.IsMatch(pass, "^.{6,18}$"))
            {
                MD5 md5Hasher = MD5.Create();

                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(pass));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                Pass = sBuilder.ToString();
            }
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