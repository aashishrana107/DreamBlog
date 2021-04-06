using DreamBlog.BusinessManagers.Interfaces;
using DreamBlog.Data.Models;
using DreamBlog.Models.HomeViewModel;
using DreamBlog.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamBlog.BusinessManagers
{
    public class HomeBusinessManager: IHomeBusinessManager
    {
        private readonly IBlogServices blogServices;
        private readonly IUserService userService;
        public HomeBusinessManager(IBlogServices blogServices, IUserService userService)
        {
            this.blogServices = blogServices;
            this.userService = userService;
        }
        public ActionResult<AuthorViewModel> GetAuthorViewModel(string authorId, string searchString, int? page)
        {
            if (authorId is null)
                return new BadRequestResult();
            var applicationUser = userService.Get(authorId);
            if (applicationUser is null)
                return new NotFoundResult();
            int pageSize = 20;
            int pageNumber = page ?? 1;
            var blogs = blogServices.GetBlogs(searchString   ?? string.Empty).Where(x=>x.Published && x.Creator==applicationUser);
            return new AuthorViewModel
            {
                Author=applicationUser,
                Blogs = new StaticPagedList<Blog>(blogs.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, blogs.Count()),
                SearchString=searchString,
                PageNumber=pageNumber
            };
        }
    }
}
