using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorwebapp.models;

namespace App.Admin.User
{
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Editor")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        public IndexModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public class UserAndRole :AppUser
        {
            public string RoleNames { get; set; }
        }

        public List<UserAndRole> user { get; set; }

        public const int  ITEM_PER_PAGE = 10;
        
        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }
        public int countPages { get; set; }

        public int totalUsers { get; set; }

        public async Task OnGet()
        {
            // user = await _userManager.Users.OrderBy(u => u.UserName).ToListAsync();
            var qr = _userManager.Users.OrderBy(u => u.UserName);

            totalUsers = await qr.CountAsync();
            countPages = (int)Math.Ceiling((double)totalUsers/ ITEM_PER_PAGE);

            if(currentPage <1)
            currentPage=1;
            if (currentPage > countPages)
            currentPage=countPages;  

            var qr1 = qr.Skip((currentPage-1)*ITEM_PER_PAGE)
                            .Take(ITEM_PER_PAGE).Select(u => new UserAndRole() {
                                Id= u.Id,
                                UserName =u.UserName
                            });

            user = await qr1.ToListAsync();

            foreach (var users in user)
            {
                var roles = await _userManager.GetRolesAsync(users);
                users.RoleNames = string.Join(",", roles);
            }
        }

        public void OnPost() => RedirectToPage();
    }
}
