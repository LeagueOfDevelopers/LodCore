using System;
using System.ComponentModel.DataAnnotations;
using Common;

namespace FrontendServices.Models
{
    public class Order
    {
        [MaxLength(50)]
        public string Header { get; set; }

        [MaxLength(50)]
        public string CustomerName { get; set; }

        public DateTime DeadLine { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(330)]
        public string Description { get; set; }

        [MaxLength(3)]
        public string[] Attachments { get; set; }

        [EnumDataType(typeof (ProjectType))]
        public ProjectType ProjectType { get; set; }
    }
}