using DreamBlog.BusinessManagers.Interfaces;
using DreamBlog.Models.AdminViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamBlog.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminBusinessManager adminBusinessManager;
        public AdminController(IAdminBusinessManager adminBusinessManager)
        {
            this.adminBusinessManager = adminBusinessManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await adminBusinessManager.GetAdminDashBoard(User));
        }
        public async Task<IActionResult> About()
        {
            return View(await adminBusinessManager.GetAboutViewModel(User));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAbout(AboutViewModel aboutViewModel)
        {
            await adminBusinessManager.UpdateAbout(aboutViewModel, User);
            return RedirectToAction("About");
        }
    }
}
