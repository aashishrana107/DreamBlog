using DreamBlog.Data;
using DreamBlog.Data.Models;
using DreamBlog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamBlog.Service
{
    public class UserService: IUserService
    {
        private readonly ApplicationDbContext applicationDbContext;
        public UserService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<ApplicationUser> Update(ApplicationUser applicationUser)
        {
            applicationDbContext.Update(applicationUser);
            await applicationDbContext.SaveChangesAsync();
            return applicationUser;
        }

        public ApplicationUser Get(string id)
        {
            return applicationDbContext.Users.FirstOrDefault(x => x.Id == id);
        }
    }
}
