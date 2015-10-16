using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotificationService;
using ProjectManagement.Domain;
using ProjectManagement.Domain.Events;
using ProjectManagement.Infrastructure;

namespace ProjectManagementTests
{
    [TestClass]
    public class ProjectProviderTests
    {
        [TestInitialize]
        public void Setup()
        {
            _pmGateway = new Mock<IProjectManagerGateway>();
            _repository = new Mock<IProjectRepository>();
            _vcsGateway = new Mock<IVersionControlSystemGateway>();

            _projectProvider = new ProjectProvider(
                _pmGateway.Object, 
                _vcsGateway.Object, 
                _repository.Object, 
                new ProjectsEventSink(
                    Mock.Of<IEventRepository>(), 
                    Mock.Of<IDistributionPolicyFactory>()));

        }

        [TestMethod]
        public void ProjectHasToBeAddedSuccessfully()
        {
        }

        private Mock<IProjectManagerGateway> _pmGateway;
        private Mock<IProjectRepository> _repository;
        private Mock<IVersionControlSystemGateway> _vcsGateway;
        private ProjectProvider _projectProvider;
    }
}
