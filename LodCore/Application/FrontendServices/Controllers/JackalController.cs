using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Http;
using Common;
using FilesManagement;
using FrontendServices.Authorization;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using ProjectManagement.Infrastructure;
using UserManagement.Application;
using UserManagement.Domain;

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
                            _imageResizer.ResizeImageByLengthOfLongestSide(account.Profile.Image.BigPhotoUri));

            allAccounts.ForEach(_userManager.UpdateUser);

            var allProjectsWithLandingImage = _projectProvider.GetProjects(project => project.LandingImage != null);

            allProjectsWithLandingImage.ForEach(
                project =>
                    project.LandingImage.SmallPhotoUri =
                        _imageResizer.ResizeImageByLengthOfLongestSide(project.LandingImage.BigPhotoUri));

            var allProjectsWithScreenshots =
                allProjectsWithLandingImage.Union(_projectProvider.GetProjects(project => project.LandingImage == null));

            foreach (var project in allProjectsWithScreenshots)
            {
                var newScreenshotSet = new HashSet<Image>();

                foreach (var screenshot in project.Screenshots)
                {
                    var smallPhotoUri = _imageResizer.ResizeImageByLengthOfLongestSide(screenshot.BigPhotoUri);

                    newScreenshotSet.Add(new Image(screenshot.BigPhotoUri, smallPhotoUri));
                }

                project.Screenshots = newScreenshotSet;
            }

            allProjectsWithLandingImage.ForEach(_projectProvider.UpdateProject);

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