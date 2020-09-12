namespace RogueFinancialPortal.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using RogueFinancialPortal.Enums;
    using RogueFinancialPortal.Helpers;
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
            UserRoleHelper roleHelper = new UserRoleHelper();
            HouseHoldHelper householdHelper = new HouseHoldHelper();
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
            context.SaveChanges();

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

       

            #endregion
            #region seed households
            List<ApplicationUser> HeadUsers = roleHelper.UsersInRole("Head").ToList();
            List<string> householdNames = new List<string>() { "HouseHold1", "TheEageles", "CoolGuyAppartment", "FamilyHouseHold", "CoderFoundryHousehold", "WinterIsComing", "TheWho", "TestHousehold", "Avangers", "FreeMoney", "Batman", "Amazon", "MuchHouseSoWow" };
            var countName = 0;
            foreach (var head in HeadUsers)
            {
                HouseHold newHouseHold = new HouseHold(true);
                newHouseHold.OwnerId = head.Id;
                newHouseHold.HouseHoldName = householdNames[countName];
                newHouseHold.Greeting = $"Welcome to the {newHouseHold.HouseHoldName} HouseHold!";
                countName++;
                context.HouseHolds.AddOrUpdate(newHouseHold);
                
            }

            #endregion
            context.SaveChanges();
            #region assign HouseHoldId to user
            foreach(var houseHold in context.HouseHolds.ToList())
            {
                 context.Users.Find(houseHold.OwnerId).HouseHoldId = houseHold.Id;
            }
            #endregion
            context.SaveChanges();

            #region Add members to Household
            List<ApplicationUser> MemberUsers = roleHelper.UsersInRole("Member").ToList();
            foreach (var houseHold in context.HouseHolds.ToList())
            {
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
                            PhoneNumber = RandomPhoneNumber(),
                            HouseHoldId = houseHold.Id,

                        },
                        demoPassword);


                    }
                    userId = userManager.FindByEmail(emailCurr).Id;
                    userManager.AddToRole(userId, "Member");
                }
                context.SaveChanges();

            }

            #endregion
            context.SaveChanges();
            #region seed bankAccounts
            foreach (var houseHold in context.HouseHolds.ToList())
            {
                BankAccount newSavingsAccount = new BankAccount
                {
                    StartingBalance = 10000,
                    WarningBalance = 300,
                    BankAccountName = "SavingsAccount",
                    AccountType = AccountType.Savings,
                    CurrentBalance = 10000,

                    Created = DateTime.Now,
                    HouseHoldId = houseHold.Id,
                    OwnerId = houseHold.OwnerId,
                    Transactions = new HashSet<Transaction>(),
                };
                context.BankAccounts.AddOrUpdate(newSavingsAccount);

                BankAccount newChcekingAccount = new BankAccount
                {
                    StartingBalance = 50000,
                    WarningBalance = 5000,
                    BankAccountName = "CheckingAccount",
                    AccountType = AccountType.Checking,
                    CurrentBalance = 50000,

                    Created = DateTime.Now,
                    HouseHoldId = houseHold.Id,
                    OwnerId = houseHold.OwnerId,
                    Transactions = new HashSet<Transaction>(),
                };
                context.BankAccounts.AddOrUpdate(newChcekingAccount);

            }
            #endregion
            context.SaveChanges();

            #region seed budgets
            foreach (var bankAccount in context.BankAccounts.ToList())
            {
                List<ApplicationUser> houseHoldMembers = householdHelper.GetHouseHoldMembers(bankAccount.HouseHoldId);

                if (bankAccount.AccountType == AccountType.Savings)
                {
                    Budget newBudget = new Budget(true)
                    {
                        BankAccontId= bankAccount.Id,
                        Created = DateTime.Now,
                        HouseHoldId = bankAccount.HouseHoldId,
                        OwnerId = houseHoldMembers[rand.Next(houseHoldMembers.Count)].Id,
                    
                        BudgetName = "Saving for Vacation",
                        Items = new HashSet<BudgetItem>()
                    };
                    context.Budgets.AddOrUpdate(newBudget);

                    Budget newBudget1 = new Budget(true)
                    {
                        BankAccontId = bankAccount.Id,
                        Created = DateTime.Now,
                        HouseHoldId = bankAccount.HouseHoldId,
                        OwnerId = houseHoldMembers[rand.Next(houseHoldMembers.Count)].Id,
                        BudgetName = "Save for new house repairs",
                        Items = new HashSet<BudgetItem>()
                    };
                    context.Budgets.AddOrUpdate(newBudget1);

                }
                else if (bankAccount.AccountType == AccountType.Checking)

                {
                    Budget newBudget = new Budget(true)
                    {
                        BankAccontId = bankAccount.Id,
                        Created = DateTime.Now,
                        HouseHoldId = bankAccount.HouseHoldId,
                        OwnerId = houseHoldMembers[rand.Next(houseHoldMembers.Count)].Id,

                        BudgetName = "Utilites",
                        Items = new HashSet<BudgetItem>()
                    };
                    context.Budgets.AddOrUpdate(newBudget);

                    Budget newBudget1 = new Budget(true)
                    {
                        BankAccontId = bankAccount.Id,
                        Created = DateTime.Now,
                        HouseHoldId = bankAccount.HouseHoldId,
                        OwnerId = houseHoldMembers[rand.Next(houseHoldMembers.Count)].Id,

                        BudgetName = "Food",
                        Items = new HashSet<BudgetItem>()
                    };
                    context.Budgets.AddOrUpdate(newBudget1);
                    Budget newBudget2 = new Budget(true)
                    {
                        BankAccontId = bankAccount.Id,
                        Created = DateTime.Now,
                        HouseHoldId = bankAccount.HouseHoldId,
                        OwnerId = houseHoldMembers[rand.Next(houseHoldMembers.Count)].Id,

                        BudgetName = "Bills",
                        Items = new HashSet<BudgetItem>()
                    };
                    context.Budgets.AddOrUpdate(newBudget2);

                }
            }
            #endregion
            context.SaveChanges();
            #region seed budget Items
            foreach (var budget in context.Budgets.ToList())
            {
                List<ApplicationUser> houseHoldMembers = householdHelper.GetHouseHoldMembers(budget.HouseHoldId);

                if (budget.BudgetName == "Utilites")
                {
                    BudgetItem newBudgetItem = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 100,
                        CurrnetAmount = 50,
                        OwnerId = houseHoldMembers[rand.Next(houseHoldMembers.Count)].Id,
                        ItemName = "Electric",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem);

                    BudgetItem newBudgetItem1 = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 100,
                        CurrnetAmount = 25,
                        OwnerId = houseHoldMembers[rand.Next(houseHoldMembers.Count)].Id,
                        ItemName = "Water",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem1);
                    BudgetItem newBudgetItem2 = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 100,
                        CurrnetAmount = 60,
                        OwnerId = houseHoldMembers[rand.Next(houseHoldMembers.Count)].Id,
                        ItemName = "Cell Phone",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem2);
                }

                if (budget.BudgetName == "Food")
                {
                    BudgetItem newBudgetItem = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 400,
                        CurrnetAmount = 250,
                        OwnerId = houseHoldMembers[rand.Next(houseHoldMembers.Count)].Id,
                        ItemName = "Groceries",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem);
                    BudgetItem newBudgetItem2 = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 100,
                        CurrnetAmount = 50,
                        OwnerId = houseHoldMembers[rand.Next(houseHoldMembers.Count)].Id,
                        ItemName = "TakeOut",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem2);
                    BudgetItem newBudgetItem3 = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 80,
                        CurrnetAmount = 75,
                        OwnerId = houseHoldMembers[rand.Next(houseHoldMembers.Count)].Id,
                        ItemName = "Pizza",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem3);
                }
                if (budget.BudgetName == "Bills")
                {
                    BudgetItem newBudgetItem = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 300,
                        CurrnetAmount = 150,
                        OwnerId = houseHoldMembers[rand.Next(houseHoldMembers.Count)].Id,
                        ItemName = "CarPayment",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem);
                    BudgetItem newBudgetItem1 = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 800,
                        CurrnetAmount = 150,
                        OwnerId = houseHoldMembers[rand.Next(houseHoldMembers.Count)].Id,
                        ItemName = "Rent",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem1);

                }
                if (budget.BudgetName == "Save for new house repairs")
                {
                    BudgetItem newBudgetItem = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 300,
                        CurrnetAmount = 170,
                        OwnerId = houseHoldMembers[rand.Next(houseHoldMembers.Count)].Id,
                        ItemName = "Roof",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem);

                }
            }



            #endregion
            context.SaveChanges();

            #region seed transactions
            #endregion
            context.SaveChanges();


        }
    }
}
