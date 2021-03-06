using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorwebapp.models;

namespace App.Admin.Role
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(RoleManager<IdentityRole> roleManager, MyWebContext myWebContext) : base(roleManager, myWebContext)
        {
        }

        

        public IdentityRole role { get; set; }

        public async Task<IActionResult> OnGet(string roleid)
        {
            if(roleid == null) return NotFound("Khong tim thay role");


            role = await _roleManager.FindByIdAsync(roleid);

            if(role == null)
            {
                return NotFound("Khong tim thay role");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
             if(roleid == null) return NotFound("Khong tim thay role");
             role = await _roleManager.FindByIdAsync(roleid);
                if(role == null)  return NotFound("Khong tim thay role");

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                StatusMessage = $"Ban vua xoa sua role {role.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                result.Errors.ToList().ForEach(error =>{
                    ModelState.AddModelError(string.Empty, error.Description);
                });
            }


            return Page();

        }
    }
}
