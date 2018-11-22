using LodCore.Domain.NotificationService;
using LodCore.Domain.ProjectManagment;
using LodCore.Facades;
using LodCore.Infrastructure.DataAccess.Repositories;
using LodCore.Infrastructure.EventBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;

namespace ProjectManagementTests
{
    [TestClass]
    public class ProjectProviderTests
    {
        private Fixture _fixture;
        private ProjectProvider _projectProvider;
        private Mock<IProjectRepository> _projectRepository;
        private Mock<IEventPublisher> _eventBus;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _projectRepository = new Mock<IProjectRepository>();
            _eventBus = new Mock<IEventPublisher>();

            _projectRepository
                .Setup(repo => repo.SaveProject(It.IsAny<Project>()))
                .Returns(1);
            _projectProvider = new ProjectProvider(
                _projectRepository.Object,
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
                               || project.LandingImage.BigPhotoUri == createRequest.LandingImage.BigPhotoUri)),
                Times.Once);

            _eventBus.Verify(mock => mock.PublishEvent(It.IsAny<NewProjectCreated>()), Times.Once);
        }
    }
}