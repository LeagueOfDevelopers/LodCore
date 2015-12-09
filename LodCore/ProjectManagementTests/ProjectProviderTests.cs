using Journalist.Collections;
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
        private Fixture _fixture;
        private Mock<IProjectManagerGateway> _pmGateway;
        private ProjectProvider _projectProvider;
        private Mock<IProjectRepository> _repository;
        private Mock<IVersionControlSystemGateway> _vcsGateway;
        private Mock<IEventSink> _eventSinkMock;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _pmGateway = new Mock<IProjectManagerGateway>();
            _repository = new Mock<IProjectRepository>();
            _vcsGateway = new Mock<IVersionControlSystemGateway>();
            _eventSinkMock = new Mock<IEventSink>();

            _pmGateway
                .Setup(pm => pm.CreateProject(It.IsAny<CreateProjectRequest>()))
                .Returns(_fixture.Create<int>());
            _vcsGateway
                .Setup(vcs => vcs.CreateRepositoryForProject(It.IsAny<CreateProjectRequest>()))
                .Returns(_fixture.Create<int>());
            _repository
                .Setup(repo => repo.SaveProject(It.IsAny<Project>()))
                .Returns(1);_projectProvider = new ProjectProvider(
                _pmGateway.Object,
                _vcsGateway.Object,
                _repository.Object,
                _eventSinkMock.Object);
        }

        [TestMethod]
        public void ProjectHasToBeAddedSuccessfully()
        {
            var createRequest = _fixture.Create<CreateProjectRequest>();

            _projectProvider.CreateProject(createRequest);

            _repository.Verify(
                repo => repo.SaveProject(It.Is<Project>(
                    project => project.Name == createRequest.Name
                               || project.Info == createRequest.Info
                               || project.AccessLevel == createRequest.AccessLevel
                               || project.ProjectType == createRequest.ProjectType
                               || project.LandingImageUri == createRequest.LandingImageUri)),
                Times.Once);
            _vcsGateway.Verify(
                vsc => vsc.CreateRepositoryForProject(It.Is<CreateProjectRequest>(
                    request => request.Equals(createRequest))),
                Times.Once);
            _pmGateway.Verify(pm => pm.CreateProject(It.Is<CreateProjectRequest>(
                request => request.Equals(createRequest))),
                Times.Once);
            _eventSinkMock.Verify(
                sink => sink.ConsumeEvent(
                    It.Is<IEventInfo>(
                        eventInfo => eventInfo.GetEventType() == typeof(NewProjectCreated).Name)), 
                Times.Once);
        }
    }
}