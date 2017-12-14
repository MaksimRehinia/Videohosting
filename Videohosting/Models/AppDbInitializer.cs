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
            
            roleManager.Create(roleUser);                       

            base.Seed(context);
        }
    }
}