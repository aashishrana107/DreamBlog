using DreamBlog.Data;
using DreamBlog.Data.Models;
using DreamBlog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DreamBlog.Service
{
    public class BlogServices: IBlogServices
    {
        private readonly ApplicationDbContext applicationDbContext;
        public BlogServices(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<Blog> Add(Blog blog)
        {
            applicationDbContext.Add(blog);
            await applicationDbContext.SaveChangesAsync();
            return blog;
        }
    }
}
