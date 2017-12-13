using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotificationService;
using Ploeh.AutoFixture;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using ProjectManagement.Domain.Events;
using ProjectManagement.Infrastructure;

namespace ProjectManagementTests
{
    [TestClass]
    public class ProjectProviderTests
    {
        private Mock<IEventSink> _eventSinkMock;
        private Fixture _fixture;
        private ProjectProvider _projectProvider;
        private Mock<IProjectRepository> _projectRepository;
        private Mock<IUserRepository> _userRepository;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _projectRepository = new Mock<IProjectRepository>();
            _eventSinkMock = new Mock<IEventSink>();
            _userRepository = new Mock<IUserRepository>();
            var paginationSettings = new ProjectManagement.Domain.PaginationSettings(10);

            _projectRepository
                .Setup(repo => repo.SaveProject(It.IsAny<Project>()))
                .Returns(1);
            _userRepository.Setup(repo => repo.GetUserRedmineId(It.IsAny<int>())).Returns(1);
            _userRepository.Setup(repo => repo.GetUserGitlabId(It.IsAny<int>())).Returns(1);
            _projectProvider = new ProjectProvider(
                _projectRepository.Object,
                _eventSinkMock.Object,
                _userRepository.Object,
                paginationSettings,
                new IssuePaginationSettings(25));
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
            
            _eventSinkMock.Verify(
                sink => sink.ConsumeEvent(
                    It.Is<IEventInfo>(
                        eventInfo => eventInfo.GetEventType() == typeof (NewProjectCreated).Name)),
                Times.Once);
        }
    }
}