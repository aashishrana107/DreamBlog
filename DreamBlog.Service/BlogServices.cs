using DreamBlog.Data;
using DreamBlog.Data.Models;
using DreamBlog.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<Blog> GetBlogs(ApplicationUser applicationUser)
        {
            return applicationDbContext.Blogs
                .Include(blog => blog.Creator)
                .Include(blog => blog.Approver)
                .Include(blog => blog.Comments)
                .Where(x => x.Creator == applicationUser);
        }

        public Blog GetBlog(int blogId)
        {
            return applicationDbContext.Blogs
                .Include(x=>x.Creator)
                .Include(x=>x.Comments)
                    .ThenInclude(Comment=>Comment.PostBy)
                .Include(x => x.Comments)
                    .ThenInclude(Comment => Comment.Comments)
                        .ThenInclude(reply=>reply.Parent)
                .FirstOrDefault(x => x.Id == blogId);
        }


        public async Task<Blog> Update(Blog blog)
        {
            applicationDbContext.Update(blog);
            await applicationDbContext.SaveChangesAsync();
            return blog;
        }

        public IEnumerable<Blog> GetBlogs(string searchString)
        {
            return applicationDbContext.Blogs
                .OrderByDescending(blog=>blog.UpdatedOn)
                .Include(blog => blog.Creator)
                .Include(blog => blog.Comments)
                .Where(x => x.Title.Contains(searchString)||x.Content.Contains(searchString));
        }
        public async Task<Comment> Add(Comment comment)
        {
            applicationDbContext.Add(comment);
            await applicationDbContext.SaveChangesAsync();
            return comment;
        }

        public Comment GetComment(int commentId)
        {
            return applicationDbContext.Comments
                .Include(comment => comment.PostBy)
                .Include(comment => comment.Blog)
                .Include(comment => comment.Parent)
                .FirstOrDefault(comment => comment.Id == commentId);
        }
    }
}
