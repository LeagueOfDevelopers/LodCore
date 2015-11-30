using System;
using System.Linq;

namespace Common
{
    internal class StrongPassword
    {
        public StrongPassword(string passwordToStrong)
        {
            if (passwordToStrong.Any(char.IsLower) &&
                passwordToStrong.Any(char.IsUpper) &&
                passwordToStrong.Any(char.IsDigit))
            {
                Password = passwordToStrong;
            }
            else
            {
                throw new ArgumentException("Password is too weak");
            }
        }

        public string Password { get; }
    }
}