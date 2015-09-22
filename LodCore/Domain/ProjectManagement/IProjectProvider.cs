﻿using System;
using System.Threading.Tasks;

namespace ProjectManagement
{
    public interface IProjectProvider
    {
        Task<Project[]> GetProjects();
        
        Task<Project> GetProject(Guid projectId);

        Task CreateProject(Project project);

        Task UpdateProject(Project project);

        Task AddUserToProject(Guid projectId, uint userId);
    }
}
