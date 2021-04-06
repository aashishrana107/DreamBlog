using DreamBlog.Data.Models;
using DreamBlog.Models.BlogViewModel;
using DreamBlog.Models.HomeViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DreamBlog.BusinessManagers
{
    public interface IBlogBusinessManager
    {
        Task<Blog> CreateBlog(CreateViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditViewModel>> UpdateBlog(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal);
        IndexViewModel GetIndexViewModel(string searchString, int? page);
        Task<ActionResult<BlogViewModel>> GetBlogViewModel(int? id, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<Comment>> CreateComment(BlogViewModel blogViewModel, ClaimsPrincipal claimsPrincipal);
    }
}
