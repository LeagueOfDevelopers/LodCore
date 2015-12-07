using System;
using System.Net.Mail;
using Common;

namespace OrderManagement.Domain
{
    public class Order
    {
        public Order(int id, string header, DateTime createdOnDateTime, MailAddress email, string description,
            string[] attachment, ProjectType projectType)
        {
            Id = id;
            Header = header;
            CreatedOnDateTime = createdOnDateTime;
            Email = email;
            Description = description;
            Attachment = attachment;
            ProjectType = projectType[];
        }

        protected Order()
        {
            
        }

        public virtual int Id { get; protected set; }
        public virtual string Header { get; protected set; }
        public virtual DateTime CreatedOnDateTime { get; protected set; }
        public virtual MailAddress Email { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string[] Attachment { get; protected set; }
        public virtual ProjectType ProjectType { get; protected set; }
    }
}