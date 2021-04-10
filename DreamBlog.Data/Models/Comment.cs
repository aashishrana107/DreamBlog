using System;
using System.Collections.Generic;
using System.Text;

namespace DreamBlog.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public Blog Blog { get; set; }
        public ApplicationUser PostBy { get; set; }
        public string Content { get; set; }
        public Comment Parent { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
    }
}
