using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotificationService;
using Ploeh.AutoFixture;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using ProjectManagement.Domain.Events;
using ProjectManagement.Infrastructure;
using RabbitMQEventBus;

namespace ProjectManagementTests
{
    [TestClass]
    public class ProjectProviderTests
    {
        private Mock<IEventSink> _eventSinkMock;
        private Fixture _fixture;
        private ProjectProvider _projectProvider;
        private Mock<IProjectRepository> _projectRepository;
        private Mock<IEventBus> _eventBus;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _projectRepository = new Mock<IProjectRepository>();
            _eventSinkMock = new Mock<IEventSink>();
            _eventBus = new Mock<IEventBus>();
            var paginationSettings = new ProjectManagement.Domain.PaginationSettings(10);

            _projectRepository
                .Setup(repo => repo.SaveProject(It.IsAny<Project>()))
                .Returns(1);
            _projectProvider = new ProjectProvider(
                _projectRepository.Object,
                _eventSinkMock.Object,
                paginationSettings,
                new IssuePaginationSettings(25),
                _eventBus.Object);
        }

        [TestMethod]
        public void ProjectHasToBeAddedSuccessfully()
        {
            var createRequest = _fixture.Create<CreateProjectRequest>();

            _projectProvider.CreateProject(createRequest);

            _projectRepository.Verify(
                repo => repo.SaveProject(It.Is<Project>(
                    project => project.Name == createRequest.Name
                               || project.Info == createRequest.Info
                               || project.AccessLevel == createRequest.AccessLevel
                               || project.LandingImage.BigPhotoUri == createRequest.LandingImage.BigPhotoUri)),
                Times.Once);

            _eventBus.Verify(mock => mock.PublishEvent(
                "Notification", "new_project_created",
                It.IsAny<NewProjectCreated>()), Times.Once);
        }
    }
}