using DreamBlog.BusinessManagers;
using DreamBlog.Data.Models;
using DreamBlog.Models.BlogViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamBlog.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly IBlogBusinessManager blogBusinessManager;
        public BlogController(IBlogBusinessManager blogBusinessManager)
        {
            this.blogBusinessManager = blogBusinessManager;
        }

        [Route("Post/{id}"),AllowAnonymous]
        public async Task<IActionResult> Index(int? id)
        {
            var actionResult = await blogBusinessManager.GetBlogViewModel(id, User);
            if (actionResult.Result is null)
            {
                return View(actionResult.Value);
            }
            return actionResult.Result;
        }
        public IActionResult Create()
        {
           
            return View(new CreateViewModel() { Categories=blogBusinessManager.GetCategories()});
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateViewModel createBlogViewModel)
        {
            await blogBusinessManager.CreateBlog(createBlogViewModel,User);
            return RedirectToAction("Create");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            
            var actionResult = await blogBusinessManager.GetEditViewModel(id, User);
            if(actionResult.Result is null)
            {
                return View(actionResult.Value);
            }
            return actionResult.Result;
        }
        [HttpPost]
        public async Task<IActionResult> Update(EditViewModel editViewModel)
        {
            var actionResult=await blogBusinessManager.UpdateBlog(editViewModel, User);
            if (actionResult.Result is null)
                return RedirectToAction("Edit", new { editViewModel.Blog.Id });
            return actionResult.Result;
        }
        [HttpPost]
        public async Task<IActionResult> Comment(BlogViewModel blogViewModel)
        {
            var actionResult = await blogBusinessManager.CreateComment(blogViewModel, User);

            if (actionResult.Result is null)
                return RedirectToAction("Index", new { blogViewModel.Blog.Id });

            return actionResult.Result;
        }
    }
}
