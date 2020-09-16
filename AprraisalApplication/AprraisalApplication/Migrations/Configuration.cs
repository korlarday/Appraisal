namespace AprraisalApplication.Migrations
{
    using AprraisalApplication.Models.Constants;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AprraisalApplication.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AprraisalApplication.Models.ApplicationDbContext context)
        {
            foreach (var role in RoleModel.roles)
            {
                context.Roles.AddOrUpdate(x => x.Name, new IdentityRole
                {
                    Name = role
                });
            }
        }
    }
}
