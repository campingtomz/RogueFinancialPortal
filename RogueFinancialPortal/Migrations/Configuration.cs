namespace RogueFinancialPortal.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using RogueFinancialPortal.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<RogueFinancialPortal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
        private Tuple<string, string> generateRandomName()
        {
            var firstNames = new List<string>() { "Brandon", "Andrew", "Jason", "Glen", "Peter", "Wade", "Richard", "Jackson", "Adam", "Khoa", "Douglas", "Angelica", "Beth", "Jaylin", "Jeremy", "Kayla", "Kodi", "Thomas" };
            var lastNames = new List<string>() { "Russell", "Gallagher", "Velez", "Nguyen", "Olmo", "Swaney", "Dolteren", "Campbell", "Stewart", "Cooper", "Twichell", "Dennis", "McGraw", "Kane", "Gutherie", "Cranford", "Zanis" };
            var rand = new Random();
            return Tuple.Create(firstNames[rand.Next(firstNames.Count)], lastNames[rand.Next(lastNames.Count)]);
        }
        private string RandomPhoneNumber()
        {
            var rand = new Random();
            return $"({rand.Next(100, 1000)})-{rand.Next(100, 1000)}-{rand.Next(1000, 10000)}";
        }
        protected override void Seed(RogueFinancialPortal.Models.ApplicationDbContext context)
        {
            var rand = new Random();
            var adminEmail = WebConfigurationManager.AppSettings["AdminEmail"];
            var adminPassword = WebConfigurationManager.AppSettings["AdminPassword"];
            var demoPassword = WebConfigurationManager.AppSettings["DemoPassword"];
            var DemoHomeUser = WebConfigurationManager.AppSettings["DH"];
            var DemoMeberUser = WebConfigurationManager.AppSettings["DM"];

            #region User Roles

            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Head"))
            {
                roleManager.Create(new IdentityRole { Name = "Head" });
            }
            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                roleManager.Create(new IdentityRole { Name = "Member" });
            }
            if (!context.Roles.Any(r => r.Name == "Default"))
            {
                roleManager.Create(new IdentityRole { Name = "Default" });
            }

            #endregion
            #region user seed
            var userManager = new UserManager<ApplicationUser>(
               new UserStore<ApplicationUser>(context));



            if (!context.Users.Any(u => u.Email == "thomas.j.zanis@gmail.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "thomas.j.zanis@gmail.com",
                    UserName = "thomas.j.zanis@gmail.com",
                    FirstName = "Thomas",
                    LastName = "Zanis",
                    PhoneNumber = RandomPhoneNumber(),


                },
                "Tobeornot123!");
            }
            var userId = userManager.FindByEmail("thomas.j.zanis@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");


            if (!context.Users.Any(u => u.Email == "DemoAdmin@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoAdmin@mailinator.com",
                    UserName = "DemoAdmin@mailinator.com",
                    FirstName = "Admin",
                    LastName = "Demo",
                    PhoneNumber = RandomPhoneNumber(),
                },
                demoPassword);
            }
            userId = userManager.FindByEmail("DemoAdmin@mailinator.com").Id;
            userManager.AddToRole(userId, "Admin");


            if (!context.Users.Any(u => u.Email == DemoMeberUser))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = DemoMeberUser,
                    UserName = DemoMeberUser,
                    FirstName = "Member",
                    LastName = "Demo",
                    PhoneNumber = RandomPhoneNumber(),


                },
                demoPassword);
            }
            userId = userManager.FindByEmail(DemoMeberUser).Id;
            userManager.AddToRole(userId, "Member");


            if (!context.Users.Any(u => u.Email == DemoHomeUser))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = DemoHomeUser,
                    UserName = DemoHomeUser,
                    FirstName = "Home",
                    LastName = "Demo",
                    PhoneNumber = RandomPhoneNumber(),
                },
                demoPassword);
            }
            userId = userManager.FindByEmail(DemoHomeUser).Id;
            userManager.AddToRole(userId, "Head");


           


            for (int i = 0; i < 10; i++)
            {
                var name = generateRandomName();
                var emailCurr = $"{name.Item1}{name.Item2}@mailinator.com";
                if (!context.Users.Any(u => u.Email == emailCurr))
                {
                    userManager.Create(new ApplicationUser()
                    {
                        Email = emailCurr,
                        UserName = emailCurr,
                        FirstName = $"{name.Item1}",
                        LastName = $"{name.Item2}",
                        PhoneNumber = RandomPhoneNumber()

                    },
                    demoPassword);


                }
                userId = userManager.FindByEmail(emailCurr).Id;
                userManager.AddToRole(userId, "Head");
            }

            for (int i = 0; i < 10; i++)
            {
                var name = generateRandomName();
                var emailCurr = $"{name.Item1}{name.Item2}@mailinator.com";
                if (!context.Users.Any(u => u.Email == emailCurr))
                {
                    userManager.Create(new ApplicationUser()
                    {
                        Email = emailCurr,
                        UserName = emailCurr,
                        FirstName = $"{name.Item1}",
                        LastName = $"{name.Item2}",
                        PhoneNumber = RandomPhoneNumber()

                    },
                    demoPassword);


                }
                userId = userManager.FindByEmail(emailCurr).Id;
                userManager.AddToRole(userId, "Member");
            }

            #endregion
        }
    }
}
