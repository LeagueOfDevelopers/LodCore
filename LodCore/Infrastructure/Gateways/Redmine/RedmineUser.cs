using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Gateways.Redmine
{
    [Serializable]
    [XmlRoot("user")]
    public class RedmineUser
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("mail")]
        public string Email { get; set; }

        [XmlElement("firstname")]
        public string FirstName { get; set; }

        [XmlElement("lastname")]
        public string LastName { get; set; }

        [XmlElement("password")]
        public string Password { get; set; }

        [XmlElement("must_change_passwd")]
        public bool MustChangePassword { get; set; }

        [XmlElement("login")]
        public string Login { get; set; }
    }
}