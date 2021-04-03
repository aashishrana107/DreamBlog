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
    }
}
