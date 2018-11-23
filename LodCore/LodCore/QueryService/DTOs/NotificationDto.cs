using System;
using System.Collections.Generic;
using System.Text;

namespace LodCore.QueryService.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Text { get; set; }
    }
}
