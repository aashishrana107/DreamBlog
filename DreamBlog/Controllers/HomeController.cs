using DreamBlog.BusinessManagers;
using DreamBlog.BusinessManagers.Interfaces;
using DreamBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DreamBlog.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IBlogBusinessManager blogBusinessManager;
        private readonly IHomeBusinessManager homeBusinessManager;

        public HomeController(ILogger<HomeController> logger, IBlogBusinessManager blogBusinessManager, IHomeBusinessManager homeBusinessManager)
        {
            _logger = logger;
            this.blogBusinessManager = blogBusinessManager;
            this.homeBusinessManager = homeBusinessManager;
        }

        public IActionResult Index(string searchString,int? page)
        {
            return View(blogBusinessManager.GetIndexViewModel(searchString,page));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Author(string authorId, string searchString,int? page)
        {
            var actionResult = homeBusinessManager.GetAuthorViewModel(authorId, searchString, page);
            if (actionResult.Result is null)
            {
                return View(actionResult.Value);
            }
            return actionResult.Result; 
        }

    }
}
