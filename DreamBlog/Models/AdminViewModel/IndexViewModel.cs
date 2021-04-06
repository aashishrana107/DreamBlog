using DreamBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamBlog.Models.AdminViewModel
{
    public class IndexViewModel
    {
        public IEnumerable<Blog> Blogs { get; set; }
    }
}
