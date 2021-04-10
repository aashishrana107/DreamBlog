using DreamBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DreamBlog.Service.Interfaces
{
    public interface IBlogServices
    {
        Task<Blog> Add(Blog blog);
        IEnumerable<Blog> GetBlogs(ApplicationUser applicationUser);
        Blog GetBlog(int blogId);
        Task<Blog> Update(Blog blog);
        IEnumerable<Blog> GetBlogs(string searchString);
        Comment GetComment(int commentId);
        Task<Comment> Add(Comment comment);
        List<Category> GetCategory();
        Category GetCategoryById(int Id);
    }
}
