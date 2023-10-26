using FoodOrderingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Controllers
{
    [AllowAnonymous]

    //this controller is only for one time use to create admin 
    //enter this controller's create action url into browser then it will create admin
    // generated -> admin-mail : admin@adm.com    password:Adm@012
    public class CreateAdminOneTimeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public CreateAdminOneTimeController(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task<IActionResult> create()
        {
            var role = await roleManager.FindByNameAsync("Admin");
            if (role == null)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = "Admin"
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                //can check for success of result
                var user = new ApplicationUser
                {
                    UserName = "admin@adm.com",
                    Email = "admin@adm.com",
                    FullName = "Admin"
                };
                var res = await userManager.CreateAsync(user, "Adm@012");

                var user_c = await userManager.FindByNameAsync("admin@adm.com");

                var res_final = await userManager.AddToRoleAsync(user_c, "Admin");

               
            }
            return RedirectToRoute(new { controller = "Menu", action = "Index" });
        }
    }
}
