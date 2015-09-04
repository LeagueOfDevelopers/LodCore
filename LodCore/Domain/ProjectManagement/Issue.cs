using System;

namespace ProjectManagement
{
    public class Issue
    {
        public Guid IssueId { get; private set; }

        public string Header { get; private set; }
        
        public string Descripton { get; private set; }

        public IssueType IssueType { get; private set; }
    }
}