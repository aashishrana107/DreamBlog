using DreamBlog.Data.Models;
using DreamBlog.Models.BlogViewModel;
using DreamBlog.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DreamBlog.BusinessManagers
{
    public class BlogBusinessManager:IBlogBusinessManager
    {
        private readonly IBlogServices blogServices;
        private readonly UserManager<ApplicationUser> userManager;
        public BlogBusinessManager(UserManager<ApplicationUser> userManager, IBlogServices blogServices)
        {
            this.blogServices = blogServices;
            this.userManager = userManager;
        }
        public async Task<Blog> CreateBlog(CreateBlogViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal)
        {
            Blog blog = createBlogViewModel.Blog;
            blog.Creator = await userManager.GetUserAsync(claimsPrincipal);
            blog.CreatedOn = DateTime.Now;
            blog=await blogServices.Add(blog);
            return blog;
            //return await blogServices.Add(blog);
        }
    }
}
