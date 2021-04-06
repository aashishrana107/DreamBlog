using DreamBlog.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamBlog.Models.BlogViewModel
{
    public class EditViewModel
    {
        [Display(Name = "Header Image")]
        public IFormFile BlogHeaderImage { get; set; }
        public Blog Blog { get; set; }
    }
}
