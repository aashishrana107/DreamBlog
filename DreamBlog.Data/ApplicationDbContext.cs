using DreamBlog.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

//using System;
//using System.Collections.Generic;
//using System.Text;

using Microsoft.EntityFrameworkCore;


namespace DreamBlog.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
