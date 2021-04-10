using System;
using System.Collections.Generic;
using System.Text;

namespace DreamBlog.Data.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public ApplicationUser Creator { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Meta { get; set; }
        public string UrlSlug { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Published { get; set; }
        public bool Approved { get; set; }
        public ApplicationUser Approver { get; set; }
        public virtual Category Category { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
        //public virtual IEnumerable<Tag> Tags { get; set; }
    }
}
