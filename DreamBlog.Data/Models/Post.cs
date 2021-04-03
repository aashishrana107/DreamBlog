using System;
using System.Collections.Generic;
using System.Text;

namespace DreamBlog.Data.Models
{
    public class Post
    {
        public int Id { get; set; }
        public Blog Blog { get; set; }
        public ApplicationUser PostBy { get; set; }
        public string Content { get; set; }
        public Post Parent { get; set; }
        public DateTime CreatedOn { get; set; }
        
        public bool Approved { get; set; }
        public ApplicationUser Approver { get; set; }
    }
}
