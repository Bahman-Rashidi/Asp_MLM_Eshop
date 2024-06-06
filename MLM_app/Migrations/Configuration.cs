using MLM_app.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MLM_app.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MLM_app.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            // Manual application requires a lot of coding
            // The automatic mode is responsive in 99 percent of cases

            AutomaticMigrationsEnabled = true;
            ContextKey = "MLM_app.Models.ApplicationDbContext";

            // For certain reasons, we add this command manually
            // If this option is false, when we want to rename an item in the model, it causes issues and tries to drop the table entirely, which results in an error
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MLM_app.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //

            //

            ApplicationUserManager userMgr = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            ApplicationRoleManager roleMgr = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            string roleName = "Administrator";
            string userName = "Admin@yahoo.com";
            string password = "M11111111@";
            string email = "Admin@yahoo.com";
            string EnglishName = "Bahman";
            string ShomareKartBanki = "1234567891234567";
            //  add  reciption method

            // addd gropu

            if (!roleMgr.RoleExists(roleName))
            {
                roleMgr.Create(new ApplicationRole(roleName));
            }
            ApplicationUser user = userMgr.FindByName(userName);
            if (user == null)
            {
                userMgr.Create(new ApplicationUser { UserName = userName, Email = email, EmailConfirmed = true, EnglishName= EnglishName, kartNumber = ShomareKartBanki,PhoneNumber = "09122109965"},
                password);
                user = userMgr.FindByName(userName);
            }
            if (!userMgr.IsInRole(user.Id, roleName))
            {
                userMgr.AddToRole(user.Id, roleName);
            }


       
        MLM_app.Models.language l1 = new language() { lanId = 1, LanName = "English" };
            MLM_app.Models.ProductGroup p1 = new ProductGroup() { ProductGroupTitle="IT",NameInSystem= "Product1" };
            MLM_app.Models.ProductGroup p2 = new ProductGroup() { ProductGroupTitle = "CAR", NameInSystem = "Product2" };



            context.languages.Add(l1);
            context.ProductGroups.Add(p1);
            context.ProductGroups.Add(p2);

            context.SaveChanges();
        }
    }
}
