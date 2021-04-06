using DreamBlog.Models.HomeViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamBlog.BusinessManagers.Interfaces
{
    public interface IHomeBusinessManager
    {
        ActionResult<AuthorViewModel> GetAuthorViewModel(string authorId, string searchString, int? page);
    }
}
