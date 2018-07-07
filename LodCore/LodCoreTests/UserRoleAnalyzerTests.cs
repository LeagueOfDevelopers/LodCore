using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LodCoreLibrary.Infrastructure.DataAccess.Repositories;
using LodCoreLibrary.Domain.ProjectManagment;

namespace ProjectManagementTests
{
    [TestClass]
    public class UserRoleAnalyzerTests
    {
        private Mock<IProjectRepository> _projectRepositoryStub;
        private UserRoleAnalyzerSettings _userRoleAnalyzerSettings;

        [TestInitialize]
        public void Initialize()
        {
            _projectRepositoryStub = new Mock<IProjectRepository>();
            _userRoleAnalyzerSettings = new UserRoleAnalyzerSettings(4, "Developer");
        }

        [TestMethod]
        public void EqualRoles_ReturnedAsUserRole()
        {
            var userId = 1;
            var roles = new[] {"Frontend-developer", "Frontend-developer", "C#-developer", "Web-designer"};
            SetupMockReturnThisRolesList(roles, userId);
            var analyzer = new UserRoleAnalyzer(_projectRepositoryStub.Object, _userRoleAnalyzerSettings);

            var userRole = analyzer.GetUserCommonRole(userId);

            Assert.AreEqual("Frontend-developer", userRole);
        }

        [TestMethod]
        public void NoRoles_ReturnDefaultRole()
        {
            var userId = 1;
            var roles = new string[] {};
            SetupMockReturnThisRolesList(roles, userId);
            var analyzer = new UserRoleAnalyzer(_projectRepositoryStub.Object, _userRoleAnalyzerSettings);

            var userRole = analyzer.GetUserCommonRole(userId);

            Assert.AreEqual("Developer", userRole);
        }

        [TestMethod]
        public void ApproximatelyEqualRoles_ReturnedAsUserRole()
        {
            var userId = 1;
            var roles = new[] {"Frontend-developer", "Backend-developer", "Backend developer", "Web-designer"};
            SetupMockReturnThisRolesList(roles, userId);
            var analyzer = new UserRoleAnalyzer(_projectRepositoryStub.Object, _userRoleAnalyzerSettings);

            var userRole = analyzer.GetUserCommonRole(userId);

            Assert.IsTrue("Backend-developer" == userRole || "Backend developer" == userRole);
        }

        private void SetupMockReturnThisRolesList(IEnumerable<string> roles, int userId)
        {
            var projectMemberships =
                roles.Select(role => new HashSet<ProjectMembership>
                {
                    new ProjectMembership(userId, role)
                });
            var projects =
                projectMemberships.Select(
                    memberships =>
                    {
                        var stub = new Mock<Project>();
                        stub.Setup(project => project.ProjectMemberships).Returns(memberships);
                        return stub;
                    });
            _projectRepositoryStub
                .Setup(repo => repo.GetUserRoles(userId))
                .Returns(projects.SelectMany(project => project.Object.ProjectMemberships
                .Where(memberships => memberships.DeveloperId == userId)
                .Select(memberships => memberships.Role)));
        }
    }
}