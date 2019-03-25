using System;

namespace LodCoreApi.Security
{
    public class SecuritySettings
    {
        public SecuritySettings(string encryptionKey, string issue, TimeSpan expirationPeriod)
        {
            EncryptionKey = encryptionKey;
            Issue = issue;
            ExpirationPeriod = expirationPeriod;
        }

        public string EncryptionKey { get; }
        public string Issue { get; }
        public TimeSpan ExpirationPeriod { get; }
    }
}