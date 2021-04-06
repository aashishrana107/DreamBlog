using DreamBlog.BusinessManagers.Interfaces;
using DreamBlog.Data.Models;
using DreamBlog.Models.AdminViewModel;
using DreamBlog.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DreamBlog.BusinessManagers
{
    public class AdminBusinessManager: IAdminBusinessManager
    {
        //remove readonly
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBlogServices blogServices;
        private readonly IUserService userService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AdminBusinessManager(UserManager<ApplicationUser> userManager, IBlogServices blogServices, IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.blogServices = blogServices;
            this.userService = userService;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IndexViewModel> GetAdminDashBoard(ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await userManager.GetUserAsync(claimsPrincipal);
            return new IndexViewModel { 
            Blogs=blogServices.GetBlogs(applicationUser)
            };
        }
        public async Task<AboutViewModel> GetAboutViewModel(ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await userManager.GetUserAsync(claimsPrincipal);
            return new AboutViewModel
            {
                ApplicationUser=applicationUser,
                SubHeader=applicationUser.SubHeader,
                Content=applicationUser.AboutContent
            };
        }
        public async Task UpdateAbout(AboutViewModel aboutViewModel,ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await userManager.GetUserAsync(claimsPrincipal);
            applicationUser.SubHeader=aboutViewModel.SubHeader;
            applicationUser.AboutContent = aboutViewModel.Content;
            if (aboutViewModel.HeaderImage != null)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}\UserFiles\Users\{applicationUser.Id}\HeaderImage.jpg";
                EnsureFolder(pathToImage);
                using (var filestream = new FileStream(pathToImage, FileMode.Create))
                {
                    await aboutViewModel.HeaderImage.CopyToAsync(filestream);
                }
            }
            await userService.Update(applicationUser);
        }
        private void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName.Length > 0)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }
    }
}
