using DreamBlog.Authorization;
using DreamBlog.Data.Models;
using DreamBlog.Models.BlogViewModel;
using DreamBlog.Models.HomeViewModel;
using DreamBlog.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DreamBlog.BusinessManagers
{
    public class BlogBusinessManager:IBlogBusinessManager
    {
        private readonly IBlogServices blogServices;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IAuthorizationService authorizationService;
       
        public BlogBusinessManager(UserManager<ApplicationUser> userManager, IBlogServices blogServices, IWebHostEnvironment webHostEnvironment,
            IAuthorizationService authorizationService)
        {
            this.blogServices = blogServices;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
            this.authorizationService = authorizationService;
        }

        public IndexViewModel GetIndexViewModel(string searchString, int? page)
        {
            int pageSize = 12;
            int pageNumber = page ?? 1;
            var blogs = blogServices.GetBlogs(searchString ?? string.Empty).Where(x=>x.Published);
            return new IndexViewModel
            {
                Blogs=new StaticPagedList<Blog>(blogs.Skip((pageNumber-1)*pageSize).Take(pageSize),pageNumber,pageSize,blogs.Count()),
                SearchString=searchString,
                PageNumber=pageNumber
            };
        }

        public async Task<ActionResult<BlogViewModel>> GetBlogViewModel(int? id,ClaimsPrincipal claimsPrincipal)
        {
            if (id is null)
                return new BadRequestResult();
            var blogId = id.Value;
            var blog = blogServices.GetBlog(blogId);
            if (blog is null)
                return new NotFoundResult();
            if (!blog.Published)
            {
                var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Read);
                if (!authorizationResult.Succeeded)
                    return DetermineActionResult(claimsPrincipal);
            }
            return new BlogViewModel {
                Blog=blog
            };

        }

        public async Task<Blog> CreateBlog(CreateViewModel createViewModel, ClaimsPrincipal claimsPrincipal)
        {
            Blog blog = createViewModel.Blog;
            blog.Creator = await userManager.GetUserAsync(claimsPrincipal);
            blog.CreatedOn = DateTime.Now;
            blog.UpdatedOn = DateTime.Now;
            blog =await blogServices.Add(blog);
            string webRootPath = webHostEnvironment.WebRootPath;
            string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}\HeaderImage.jpg";
            EnsureFolder(pathToImage);
            using (var filestream=new FileStream(pathToImage, FileMode.Create))
            {
                await createViewModel.BlogHeaderImage.CopyToAsync(filestream);
            }

                return blog;
            //return await blogServices.Add(blog);
        }


        public async Task<ActionResult<EditViewModel>> UpdateBlog(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal)
        {
            var blog = blogServices.GetBlog(editViewModel.Blog.Id);
            if (blog is null)
                return new NotFoundResult();
            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Update);
            if (!authorizationResult.Succeeded)
                return DetermineActionResult(claimsPrincipal);
            blog.Published = editViewModel.Blog.Published;
            blog.Title = editViewModel.Blog.Title;
            blog.Content = editViewModel.Blog.Content;
            blog.UpdatedOn = DateTime.Now;
            if(editViewModel.BlogHeaderImage!=null)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}\HeaderImage.jpg";
                EnsureFolder(pathToImage);
                using (var filestream = new FileStream(pathToImage, FileMode.Create))
                {
                    await editViewModel.BlogHeaderImage.CopyToAsync(filestream);
                }
            }
            return new EditViewModel
            {
                Blog = await blogServices.Update(blog)
            };
        }

        public async Task<ActionResult<EditViewModel>> GetEditViewModel(int? id,ClaimsPrincipal claimsPrincipal)
        {
            if (id is null)
                return new BadRequestResult();
            var blogId = id.Value;
            var blog = blogServices.GetBlog(blogId);
            if (blog is null)
                return new NotFoundResult();
            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Update);
            //if(!authorizationResult.Succeeded)
            //{
            //    if (claimsPrincipal.Identity.IsAuthenticated)
            //    {
            //        return new ForbidResult();
            //    }
            //    else
            //    {
            //        return new ChallengeResult();
            //    }
            //}
            if (!authorizationResult.Succeeded)
                return DetermineActionResult(claimsPrincipal);

            return new EditViewModel
            {
                Blog = blog
            };
        }

        private ActionResult DetermineActionResult(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
        }


        private void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName.Length > 0)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }
        public async Task<ActionResult<Comment>> CreateComment(BlogViewModel blogViewModel, ClaimsPrincipal claimsPrincipal)
        {
            if (blogViewModel.Blog is null || blogViewModel.Blog.Id == 0)
                return new BadRequestResult();

            var blog = blogServices.GetBlog(blogViewModel.Blog.Id);

            if (blog is null)
                return new NotFoundResult();

            var comment = blogViewModel.Comment;

            comment.PostBy = await userManager.GetUserAsync(claimsPrincipal);
            comment.Blog = blog;
            comment.CreatedOn = DateTime.Now;

            if (comment.Parent != null)
            {
                comment.Parent = blogServices.GetComment(comment.Parent.Id);
            }

            return await blogServices.Add(comment);
        }
    }
}
