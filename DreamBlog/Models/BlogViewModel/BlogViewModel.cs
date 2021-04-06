using DreamBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamBlog.Models.BlogViewModel
{
    public class BlogViewModel
    {
        public Blog Blog { get; set; }
        public Comment Comment { get; set; }
    }
}
