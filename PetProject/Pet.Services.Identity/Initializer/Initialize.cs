using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Pet.Services.Identity.DbContexts;
using Pet.Services.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pet.Services.Identity.Initializer
{

    public class Initialize : IDbInitializer
    {
        private readonly ApplicationDbContext _db;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Initialize(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initializer()
        {
            if (_roleManager.FindByNameAsync(SD.Admin).Result == null)
            {

                _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }
            else { return; }
            ApplicationUser AdminUser = new ApplicationUser
            {
               UserName="admin1@gmail.com",
               Email="admin1@gmail.com",
               EmailConfirmed=true,
               
               PhoneNumber="0663376944",
               
               FirstName="Mourad",
               LastName="Mek"


            };

            _userManager.CreateAsync(AdminUser,"Admin123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(AdminUser, SD.Admin).GetAwaiter().GetResult();
            var Result=_userManager.AddClaimsAsync(AdminUser, new Claim[] { new System.Security.Claims.Claim(JwtClaimTypes.Name, AdminUser.FirstName + " " + AdminUser.LastName), new Claim(JwtClaimTypes.GivenName, AdminUser.FirstName), new Claim(JwtClaimTypes.FamilyName, AdminUser.LastName), new Claim(JwtClaimTypes.Role, SD.Admin) }).Result;
            ApplicationUser CustomerUser = new ApplicationUser
            {
                UserName = "customer1@gmail.com",
                Email = "customer1@gmail.com",
                EmailConfirmed = true,

                PhoneNumber = "0663376944",

                FirstName = "Mourad",
                LastName = "Mek"


            };

            _userManager.CreateAsync(CustomerUser, "Customer123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(CustomerUser, SD.Customer).GetAwaiter().GetResult();
            var Result2 = _userManager.AddClaimsAsync(CustomerUser, new Claim[] { new System.Security.Claims.Claim(JwtClaimTypes.Name, CustomerUser.FirstName + " " + CustomerUser.LastName), new Claim(JwtClaimTypes.GivenName, CustomerUser.FirstName), new Claim(JwtClaimTypes.FamilyName, CustomerUser.LastName), new Claim(JwtClaimTypes.Role, SD.Customer) }).Result;

        }
    }
}
