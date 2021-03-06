using AspNetCore.SEOHelper;
using AspNetCore.SEOHelper.Sitemap;
using DreamBlog.Authorization;
using DreamBlog.Data.Models;
using DreamBlog.Models.BlogViewModel;
using DreamBlog.Models.HomeViewModel;
using DreamBlog.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            int pageSize = 20;
            int pageNumber = page ?? 1;
            var blogs = blogServices.GetBlogs(searchString ?? string.Empty).Where(x=>x.Published);

            //https://github.com/esty-c/AspNetCore.SEOHelper/tree/e66b8f24b02e53019bf895871aad5d429d35d316
            //var obj = new SitemapDocument();
            //List<SitemapNode> list = new List<SitemapNode>();
            //foreach (var blog in blogs)
            //{
            //    list.Add(new SitemapNode { LastModified = blog.UpdatedOn, Priority = 0.8, Url = $@"http://garibbro.com/Post/{blog.Id}", Frequency = SitemapFrequency.Yearly });
            //}
            //var baseDirectroy = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            //obj.CreateSitemapXML(list, baseDirectroy);
            //var items = obj.LoadFromFile(baseDirectroy);

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
            blog.UrlSlug = UrlSlug(createViewModel.Blog.Title);
            blog.Category = blogServices.GetCategoryById(createViewModel.Blog.Category.Id);
            blog =await blogServices.Add(blog);
            if (createViewModel.BlogHeaderImage != null)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}\HeaderImage.jpg";
                EnsureFolder(pathToImage);
                using (var filestream = new FileStream(pathToImage, FileMode.Create))
                {
                    await createViewModel.BlogHeaderImage.CopyToAsync(filestream);
                }
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
            blog.Meta = PascalCaseExtensionMethod.ToPascalCase(editViewModel.Blog.Title);
            blog.UrlSlug = SEOFriendlyURLExtension.ToSEOQueryString(editViewModel.Blog.Title);

            //var obj = new SitemapDocument();
            //List<SitemapNode> list = new List<SitemapNode>();
            //list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "http://codingwithesty.com/serilog-mongodb-in-asp-net-core", Frequency = SitemapFrequency.Daily });
            //list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "http://codingwithesty.com/logging-in-asp-net-core", Frequency = SitemapFrequency.Yearly });
            //list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.7, Url = "http://codingwithesty.com/robots-txt-in-asp-net-core", Frequency = SitemapFrequency.Monthly });
            //list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.5, Url = "http://codingwithesty.com/versioning-asp.net-core-apiIs-with-swagger", Frequency = SitemapFrequency.Weekly });
            //list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.4, Url = "http://codingwithesty.com/configuring-swagger-asp-net-core-web-api", Frequency = SitemapFrequency.Never });
            //var baseDirectroy = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            //obj.CreateSitemapXML(list, baseDirectroy);
            //var items = obj.LoadFromFile(baseDirectroy);

            blog.Category= blogServices.GetCategoryById(editViewModel.Blog.Category.Id);
            if (editViewModel.BlogHeaderImage!=null)
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
                Blog = blog,
                //Category= GetCategoriesById(blog.Category.Id)
                Category= GetCategories()
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

        public List<Category> GetCategoryList()
        {
            var category = blogServices.GetCategory();
            return category;
        }

        public IEnumerable<SelectListItem> GetCategories()
        {
            
            List<SelectListItem> categories = blogServices.GetCategory()
                .OrderBy(n => n.Name)
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.Name
                    }).ToList();
            var item = new SelectListItem()
            {
                Value = null,
                Text = "--- select Category ---"
            };
            categories.Insert(0, item);
            return new SelectList(categories, "Value", "Text");
            
        }
        public IEnumerable<SelectListItem> GetCategoriesById(int Id)
        {
            IEnumerable<SelectListItem> regions = blogServices.GetCategory()
                        .OrderBy(n => n.Name)
                        .Where(n => n.Id == Id)
                        .Select(n =>
                           new SelectListItem
                           {
                               Value = n.Id.ToString(),
                               Text = n.Name
                           }).ToList();
            return new SelectList(regions, "Value", "Text");

        }
        
        public string UrlSlug(string title)
        {
            string localtitle;
            localtitle = title.Replace(' ', '_');
            return localtitle;
        }
    }
}
