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
            if (System.Diagnostics.Debugger.IsAttached == false)
            {
                System.Diagnostics.Debugger.Launch();
            }
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
            var DemoDefaultUser = WebConfigurationManager.AppSettings["DD"];

            var DemoMemberUser = WebConfigurationManager.AppSettings["DM"];

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


            if (!context.Users.Any(u => u.Email == DemoMemberUser))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = DemoMemberUser,
                    UserName = DemoMemberUser,
                    FirstName = "Member",
                    LastName = "Demo",
                    PhoneNumber = RandomPhoneNumber(),


                },
                demoPassword);
            }
            userId = userManager.FindByEmail(DemoMemberUser).Id;
            userManager.AddToRole(userId, "Member");

            if (!context.Users.Any(u => u.Email == DemoDefaultUser))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = DemoDefaultUser,
                    UserName = DemoDefaultUser,
                    FirstName = "Default",
                    LastName = "Demo",
                    PhoneNumber = RandomPhoneNumber(),


                },
                demoPassword);
            }
            userId = userManager.FindByEmail(DemoDefaultUser).Id;
            userManager.AddToRole(userId, "Default");


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
                userManager.AddToRole(userId, "Member");
            }



            #endregion
            #region seed households
            //List<ApplicationUser> HeadUsers = roleHelper.UsersInRole("Head").ToList();
            //List<string> householdNames = new List<string>() { "HouseHold1", "TheEageles", "CoolGuyAppartment", "FamilyHouseHold", "CoderFoundryHousehold", "WinterIsComing", "TheWho", "TestHousehold", "Avangers", "FreeMoney", "Batman", "Amazon", "MuchHouseSoWow" };
            //var countName = 0;
            //foreach (var head in HeadUsers)
            //{
            HouseHold newHouseHold = new HouseHold(true);
            newHouseHold.OwnerId = userManager.FindByEmail(DemoHomeUser).Id;
            newHouseHold.HouseHoldName = "RogueHouseHold";
            newHouseHold.Greeting = $"Welcome to the {newHouseHold.HouseHoldName} HouseHold!";
            context.HouseHolds.AddOrUpdate(newHouseHold);

            //}

            #endregion
            context.SaveChanges();
            //#region assign HouseHoldId to user
            //foreach(var houseHold in context.HouseHolds.ToList())
            //{
            //     context.Users.Find(houseHold.OwnerId).HouseHoldId = houseHold.Id;
            //}
            //#endregion
            //context.SaveChanges();

            //#region Add members to Household
            //List<ApplicationUser> MemberUsers = roleHelper.UsersInRole("Member").ToList();
            //foreach (var houseHold in context.HouseHolds.ToList())
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        var name = generateRandomName();
            //        var emailCurr = $"{name.Item1}{name.Item2}@mailinator.com";
            //        if (!context.Users.Any(u => u.Email == emailCurr))
            //        {
            //            userManager.Create(new ApplicationUser()
            //            {
            //                Email = emailCurr,
            //                UserName = emailCurr,
            //                FirstName = $"{name.Item1}",
            //                LastName = $"{name.Item2}",
            //                PhoneNumber = RandomPhoneNumber(),
            //                HouseHoldId = houseHold.Id,

            //            },
            //            demoPassword);


            //        }
            //        userId = userManager.FindByEmail(emailCurr).Id;
            //        userManager.AddToRole(userId, "Member");
            //    }
            //    context.SaveChanges();

            //}

            //#endregion
            #region Assign users to household
            var owner = userManager.FindByEmail(DemoHomeUser);
            var household = context.HouseHolds.FirstOrDefault(h => h.OwnerId == owner.Id);
            owner.HouseHoldId = household.Id;
            foreach (var member in context.Users.ToList())
            {
                member.HouseHoldId = household.Id;  
                

            }
                context.SaveChanges();
            #endregion
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
            List<ApplicationUser> MemberUsers = roleHelper.UsersInRole("Member").ToList();

            #region seed budgets
            foreach (var bankAccount in context.BankAccounts.ToList())
            {

                if (bankAccount.AccountType == AccountType.Savings)
                {
                    Budget newBudget = new Budget(true)
                    {
                        BankAccountId = bankAccount.Id,
                        Created = DateTime.Now,
                        HouseHoldId = bankAccount.HouseHoldId,
                        OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,

                        BudgetName = "Saving for Vacation",
                        Items = new HashSet<BudgetItem>()
                    };
                    context.Budgets.AddOrUpdate(newBudget);

                    Budget newBudget1 = new Budget(true)
                    {
                        BankAccountId = bankAccount.Id,
                        Created = DateTime.Now,
                        HouseHoldId = bankAccount.HouseHoldId,
                        OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                        BudgetName = "Save for new house repairs",
                        Items = new HashSet<BudgetItem>()
                    };
                    context.Budgets.AddOrUpdate(newBudget1);

                }
                else if (bankAccount.AccountType == AccountType.Checking)

                {
                    Budget newBudget = new Budget(true)
                    {
                        BankAccountId = bankAccount.Id,
                        Created = DateTime.Now,
                        HouseHoldId = bankAccount.HouseHoldId,
                        OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,

                        BudgetName = "Utilites",
                        Items = new HashSet<BudgetItem>()
                    };
                    context.Budgets.AddOrUpdate(newBudget);

                    Budget newBudget1 = new Budget(true)
                    {
                        BankAccountId = bankAccount.Id,
                        Created = DateTime.Now,
                        HouseHoldId = bankAccount.HouseHoldId,
                        OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,

                        BudgetName = "Food",
                        Items = new HashSet<BudgetItem>()
                    };
                    context.Budgets.AddOrUpdate(newBudget1);
                    Budget newBudget2 = new Budget(true)
                    {
                        BankAccountId = bankAccount.Id,
                        Created = DateTime.Now,
                        HouseHoldId = bankAccount.HouseHoldId,
                        OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,

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

                if (budget.BudgetName == "Utilites")
                {
                    BudgetItem newBudgetItem = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 100,
                        CurrentAmount = 50,
                        OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                        ItemName = "Electric",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem);

                    BudgetItem newBudgetItem1 = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 100,
                        CurrentAmount = 25,
                        OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                        ItemName = "Water",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem1);
                    BudgetItem newBudgetItem2 = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 100,
                        CurrentAmount = 60,
                        OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
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
                        CurrentAmount = 250,
                        OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                        ItemName = "Groceries",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem);
                    BudgetItem newBudgetItem2 = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 100,
                        CurrentAmount = 50,
                        OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                        ItemName = "TakeOut",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem2);
                    BudgetItem newBudgetItem3 = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 80,
                        CurrentAmount = 75,
                        OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                        ItemName = "DineOut",
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
                        CurrentAmount = 150,
                        OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                        ItemName = "CarPayment",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem);
                    BudgetItem newBudgetItem1 = new BudgetItem(true)
                    {
                        Created = DateTime.Now,
                        BudgetId = budget.Id,
                        TargetAmount = 800,
                        CurrentAmount = 150,
                        OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
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
                        CurrentAmount = 170,
                        OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                        ItemName = "Roof",
                        Transactions = new HashSet<Transaction>()
                    };
                    context.BudgetItems.AddOrUpdate(newBudgetItem);

                }
            }



            #endregion
            context.SaveChanges();

            #region seed transactions
            var electricBudgetItem = context.BudgetItems.FirstOrDefault(bi => bi.ItemName == "Electric");
            Transaction newTransactionElectric1 = new Transaction(true)
            {
                OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                BankAccountId = electricBudgetItem.Budget.BankAccountId,
                BudgetItemId = electricBudgetItem.Id,
                TransactionType = TransactionType.Withdrawal,
                Created = DateTime.Now.AddDays(-90),
                Amount = 135,
                Memo = "Electric Bill"

            };
            context.Transactions.AddOrUpdate(newTransactionElectric1);
            Transaction newTransactionElectric2 = new Transaction(true)
            {
                OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                BankAccountId = electricBudgetItem.Budget.BankAccountId,
                BudgetItemId = electricBudgetItem.Id,
                TransactionType = TransactionType.Withdrawal,
                Created = DateTime.Now.AddDays(-60),
                Amount = 110,
                Memo = "Electric Bill"

            };
            context.Transactions.AddOrUpdate(newTransactionElectric2);
            Transaction newTransactionElectric3 = new Transaction(true)
            {
                OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                BankAccountId = electricBudgetItem.Budget.BankAccountId,
                BudgetItemId = electricBudgetItem.Id,
                TransactionType = TransactionType.Withdrawal,
                Created = DateTime.Now.AddDays(-30),
                Amount = 115,
                Memo = "Electric Bill"

            };
            context.Transactions.AddOrUpdate(newTransactionElectric3);
            //----------------------------------------------------------------------------------------------------
            var WaterBudgetItem = context.BudgetItems.FirstOrDefault(bi => bi.ItemName == "Water");
            Transaction newTransactionWater1 = new Transaction(true)
            {
                OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                BankAccountId = WaterBudgetItem.Budget.BankAccountId,
                BudgetItemId = WaterBudgetItem.Id,
                TransactionType = TransactionType.Withdrawal,
                Created = DateTime.Now.AddDays(-97),
                Amount = 80,
                Memo = "Water Bill"

            };
            context.Transactions.AddOrUpdate(newTransactionWater1);
            Transaction newTransactionWater2 = new Transaction(true)
            {
                OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                BankAccountId = WaterBudgetItem.Budget.BankAccountId,
                BudgetItemId = WaterBudgetItem.Id,
                TransactionType = TransactionType.Withdrawal,
                Created = DateTime.Now.AddDays(-54),
                Amount = 90,
                Memo = "Water Bill"

            };
            context.Transactions.AddOrUpdate(newTransactionWater2);
            Transaction newTransactionWater3 = new Transaction(true)
            {
                OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                BankAccountId = WaterBudgetItem.Budget.BankAccountId,
                BudgetItemId = WaterBudgetItem.Id,
                TransactionType = TransactionType.Withdrawal,
                Created = DateTime.Now.AddDays(-24),
                Amount = 90,
                Memo = "Water Bill"

            };
            context.Transactions.AddOrUpdate(newTransactionWater3);
            //----------------------------------------------------------------------------------------------------

            var GroceriesBudgetItem = context.BudgetItems.FirstOrDefault(bi => bi.ItemName == "Groceries");
            var date = -100;

            while (date <= 0)
            { 
                Transaction Groceries = new Transaction(true)
                {
                    OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                    BankAccountId = GroceriesBudgetItem.Budget.BankAccountId,
                    BudgetItemId = GroceriesBudgetItem.Id,
                    TransactionType = TransactionType.Withdrawal,
                    Created = DateTime.Now.AddDays(date),
                    Amount = rand.Next(300, 400) + (decimal)(rand.Next(100) / (decimal)100),
                    Memo = "Groceries"

                };
                date += rand.Next(7, 12);
                context.Transactions.AddOrUpdate(Groceries);
            }
            //----------------------------------------------------------------------------------------------------
            var TakeOutBudgetItem = context.BudgetItems.FirstOrDefault(bi => bi.ItemName == "TakeOut");

            //for (int i = 0; i < 14; i++)
            //{
            date = -100;
            while (date <= 0)
            {
                Transaction TakeOut = new Transaction(true)
                {
                    OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                    BankAccountId = TakeOutBudgetItem.Budget.BankAccountId,
                    BudgetItemId = TakeOutBudgetItem.Id,
                    TransactionType = TransactionType.Withdrawal,
                    Created = DateTime.Now.AddDays(date),
                    Amount = rand.Next(12, 40) + (decimal)(rand.Next(100) / (decimal)100),
                    Memo = "TakeOut"

                };
                date += rand.Next(3, 7);
                context.Transactions.AddOrUpdate(TakeOut);
            }
            
            //----------------------------------------------------------------------------------------------------
            var DineOutBudgetItem = context.BudgetItems.FirstOrDefault(bi => bi.ItemName == "DineOut");


            date = -100;
            while (date <= 0)
            {
                Transaction DineOut = new Transaction(true)
                {
                    OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                    BankAccountId = DineOutBudgetItem.Budget.BankAccountId,
                    BudgetItemId = DineOutBudgetItem.Id,
                    TransactionType = TransactionType.Withdrawal,
                    Created = DateTime.Now.AddDays(date),
                    Amount = rand.Next(35, 80) + (decimal)(rand.Next(100) / (decimal)100),
                    Memo = "DineOut"

                };
                date += rand.Next(7);
                context.Transactions.AddOrUpdate(DineOut);
            }
            //----------------------------------------------------------------------------------------------------

            date = -100;
            var CarPaymentBudgetItem = context.BudgetItems.FirstOrDefault(bi => bi.ItemName == "CarPayment");
            while (date <= 0)
            {
                Transaction CarPayment = new Transaction(true)
                {
                    OwnerId = MemberUsers[rand.Next(MemberUsers.Count)].Id,
                    BankAccountId = CarPaymentBudgetItem.Budget.BankAccountId,
                    BudgetItemId = CarPaymentBudgetItem.Id,
                    TransactionType = TransactionType.Withdrawal,
                    Created = DateTime.Now.AddDays(date),
                    Amount = rand.Next(35, 80) + (decimal)(rand.Next(100) / (decimal)100),
                    Memo = "CarPayment"

                };
                date += rand.Next(25);
                context.Transactions.AddOrUpdate(CarPayment);
            }
            //----------------------------------------------------------------------------------------------------

            #endregion
            context.SaveChanges();

        }
    }
}
