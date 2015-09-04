using System;
using Journalist;

namespace UserManagement
{
    public class UserSummary
    {
        public UserSummary(uint userId, string firstname, string lastname, Uri smallPictureUri)
        {
            Require.NotEmpty(firstname, nameof(firstname));
            Require.NotEmpty(lastname, nameof(lastname));
            UserId = userId;
            Firstname = firstname;
            Lastname = lastname;
            SmallPictureUri = smallPictureUri;
        }

        public uint UserId { get; private set; }  

        public string Firstname { get; private set; }

        public string Lastname { get; private set; }

        public Uri SmallPictureUri { get; private set; }
    }
}