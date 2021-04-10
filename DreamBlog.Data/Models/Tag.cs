using System;
using System.Collections.Generic;
using System.Text;

namespace DreamBlog.Data.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlSlug { get; set; }
        public string Description { get; set; }
        public virtual IList<Blog> Blogs { get; set; }
    }
}
