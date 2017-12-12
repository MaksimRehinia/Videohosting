using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Videohosting.Models
{
    // Set this class as initializer of database, if it's not created yet.
    public class AppDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            
            var roleUser = new IdentityRole {Name = "User"};
            var roleAdmin = new IdentityRole {Name = "Admin"};
            
            roleManager.Create(roleUser);
            roleManager.Create(roleAdmin);

            var admin_1 = new ApplicationUser { Email = "maksimr1998@mail.ru", UserName = "Admin_1" };
            string password_1 = "m_reginya";

            var admin_2 = new ApplicationUser { Email = "dgasyul@gmail.com", UserName = "Admin_2" };            
            string password_2 = "gasyulchik";

            var result = userManager.Create(admin_1, password_1);
            
            if (result.Succeeded)
            {
                userManager.AddToRole(admin_1.Id, roleUser.Name);
                userManager.AddToRole(admin_1.Id, roleAdmin.Name);
            }

            result = userManager.Create(admin_2, password_2);
            if (result.Succeeded)
            {
                userManager.AddToRole(admin_2.Id, roleUser.Name);
                userManager.AddToRole(admin_2.Id, roleAdmin.Name);
            }

            base.Seed(context);
        }
    }
}