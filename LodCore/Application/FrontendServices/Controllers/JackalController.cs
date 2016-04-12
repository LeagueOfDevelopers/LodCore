using System;
using System.Net.Mail;
using System.Web.Http;
using FilesManagement;
using FrontendServices.Authorization;
using ProjectManagement.Application;
using ProjectManagement.Infrastructure;
using UserManagement.Application;

namespace FrontendServices.Controllers
{
    public class JackalController : ApiController
    {
        [HttpPost]
        [Route("jackal")]
        public IHttpActionResult JackalAllImages()
        {
            if (!Equals(_userRepository.GetAccount(User.Identity.GetId()).Email, new MailAddress("boris.valdman@live.ru")))
            {
                throw new AccessViolationException("Wrong number, bitch!");
            }

            var allAccounts = _userRepository.GetAllAccounts(account => account.Profile.Image != null);

            allAccounts.ForEach(
                    account =>
                        account.Profile.Image.SmallPhotoUri =
                            _imageResizer.ResizeImageByLengthOfLongestSide(account.Profile.Image.BigPhotoUri,
                                _imageResizer.ReadLengthOfLongestSideOfResized()));

            allAccounts.ForEach(_userManager.UpdateUser);

            var allProjects = _projectProvider.GetProjects(project => project.LandingImage != null);

            allProjects.ForEach(
                project =>
                    project.LandingImage.SmallPhotoUri =
                        _imageResizer.ResizeImageByLengthOfLongestSide(project.LandingImage.BigPhotoUri,
                            _imageResizer.ReadLengthOfLongestSideOfResized()));

            allProjects.ForEach(_projectProvider.UpdateProject);

            return Ok("All image were successfuly jackaled!");
        }

        private readonly UserManagement.Infrastructure.IUserRepository _userRepository;
        private readonly IUserManager _userManager;

        private readonly IProjectRepository _projectRepository;
        private readonly IProjectProvider _projectProvider;

        private readonly IImageResizer _imageResizer;

        public JackalController(UserManagement.Infrastructure.IUserRepository userRepository, IProjectRepository projectRepository, IUserManager userManager, IProjectProvider projectProvider, IImageResizer imageResizer)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _userManager = userManager;
            _projectProvider = projectProvider;
            _imageResizer = imageResizer;
        }
    }
}