using DreamBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DreamBlog.Service.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> Update(ApplicationUser applicationUser);
        ApplicationUser Get(string id);
    }
}
