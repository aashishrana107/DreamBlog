using DreamBlog.Models.AdminViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DreamBlog.BusinessManagers.Interfaces
{
    public interface IAdminBusinessManager
    {
        Task<IndexViewModel> GetAdminDashBoard(ClaimsPrincipal claimsPrincipal);
        Task<AboutViewModel> GetAboutViewModel(ClaimsPrincipal claimsPrincipal);
        Task UpdateAbout(AboutViewModel aboutViewModel, ClaimsPrincipal claimsPrincipal);
    }
}
