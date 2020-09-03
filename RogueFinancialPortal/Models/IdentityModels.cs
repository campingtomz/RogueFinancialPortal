using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace RogueFinancialPortal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        #region Parents/Childrem
        private ApplicationDbContext db = new ApplicationDbContext();

        public virtual ICollection<Budget> Budgets { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }

        #endregion
        #region Actuall Properties
        [Required]
        [Display(Name= "First Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name Must be between 2 and 50 characters")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name Must be between 2 and 50 characters")]
        public string LastName { get; set; }
        public string AvatarPath { get; set; }
        public int? HouseHoldId { get; set; }
        public virtual HouseHold HouseHold { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
        [NotMapped]
        public string HouseHoldName
        {
            get
            {
                var hhId = HouseHoldId;
                if (hhId == null)
                {
                    return "";
                }
                else
                {
                    return db.HouseHolds.Find(hhId).HouseHoldName;

                }
            }
        }
        #endregion

        #region Constructor
        public ApplicationUser()
        {
            Budgets = new HashSet<Budget>();
            Notifications = new HashSet<Notification>();
            Transactions = new HashSet<Transaction>();
            BankAccounts = new HashSet<BankAccount>();
            AvatarPath = "/Avatar/default.png";
            
        }
        #endregion

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            var hhId = HouseHoldId != null ? HouseHoldId.ToString() : "";
            userIdentity.AddClaim(new Claim("HouseHoldId", hhId));
            userIdentity.AddClaim(new Claim("AvatarPath", AvatarPath));
            userIdentity.AddClaim(new Claim("FirstName", FirstName));
            userIdentity.AddClaim(new Claim("LastName", LastName));

            userIdentity.AddClaim(new Claim("HouseHoldId", hhId));

            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<HouseHold> HouseHolds { get; set; }

    }
}