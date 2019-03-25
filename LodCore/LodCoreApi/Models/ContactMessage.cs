using System.ComponentModel.DataAnnotations;

namespace LodCoreApi.Models
{
    public class ContactMessage
    {
        [MaxLength(30)] public string ClientName { get; set; }

        [EmailAddress] public string ClientEmail { get; set; }

        [MaxLength(50)] public string MessageTopic { get; set; }

        [MaxLength(300)] public string MessageBody { get; set; }
    }
}