using System;
using Common;

namespace FrontendServices.Models
{
    public class RegisterNewOrderRequest
    {
        public string Header { get; set; }
        public string CustomerName { get; set; }
        public DateTime DeadLine { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string[] Attachments { get; set; }
        public ProjectType ProjectType { get; set; }
    }
}