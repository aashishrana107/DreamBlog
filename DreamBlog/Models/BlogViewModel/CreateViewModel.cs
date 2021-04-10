using DreamBlog.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamBlog.Models.BlogViewModel
{
    public class CreateViewModel
    {
        [Required,Display(Name ="Header Image")]
        public IFormFile BlogHeaderImage { get; set; }
        public Blog Blog { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
