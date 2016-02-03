using System;
using System.Collections.Generic;
using System.Net.Mail;
using Common;
using Journalist;

namespace OrderManagement.Domain
{
    public class Order
    {
        public Order(
            string header,
            string customerName, 
            DateTime createdOnDateTime,
            DateTime deadLine, 
            MailAddress email, 
            string description,
            ISet<Uri> attachments, 
            ProjectType projectType)
        {
            Require.NotEmpty(header, nameof(header));
            Require.NotEmpty(customerName, nameof(customerName));
            Require.NotNull(email, nameof(email));
            Require.NotNull(description, nameof(description));
            Require.NotNull(attachments, nameof(attachments));

            Header = header;
            CustomerName = customerName;
            CreatedOnDateTime = createdOnDateTime;
            DeadLine = deadLine;
            Email = email;
            Description = description;
            Attachments = attachments;
            ProjectType = projectType;
        }

        protected Order()
        {
        }

        public virtual int Id { get; protected set; }
        public virtual string Header { get; protected set; }
        public virtual string CustomerName { get; protected set; }
        public virtual DateTime CreatedOnDateTime { get; protected set; }
        public virtual DateTime DeadLine { get; protected set; }
        public virtual MailAddress Email { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual ISet<Uri> Attachments { get; protected set; }
        public virtual ProjectType ProjectType { get; protected set; }
    }
}