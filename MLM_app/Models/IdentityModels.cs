using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MLM_app.Models
{
    #region User
    /*******************************************************************************/
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        #region Additional properties of the user table
        [StringLength(50)]
        [Display(Name = "English name")]
        public string EnglishName { get; set; }

        //[StringLength(50)]
        //[Display(Name = "Persian name")]
        //public string EnglishName { get; set; }

        [StringLength(200)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "SIN Number")]
        [StringLength(10, ErrorMessage = "SIN Number must be 10 digits", MinimumLength = 10)]
        public string NationalCode { get; set; }

        [Display(Name = "Postal code")]
        [StringLength(10, ErrorMessage = "Postal code must be 6 digits", MinimumLength = 6)]
        public string ZipCode { get; set; }

        [Display(Name = "Date of birth")]
        [UIHint("DatePicker")]
        public Nullable<DateTime> DateOfBirth { get; set; }

        [Display(Name = "Registration date")]
        public Nullable<DateTime> DateOfRegistration { get; set; }


        [StringLength(128)]
        [Display(Name ="Parent User")]
        public string ParentUserId { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [StringLength(128)]
        [Display(Name = "User  first left")]
        public string Left1 { get; set; }

        [StringLength(128)]
        [Display(Name = "User, second right hand")]
        public string Left2 { get; set; }

        [StringLength(128)]
        [Display(Name = "User  frist right ")]
        public string Right1 { get; set; }

        [StringLength(128)]
        [Display(Name = "User  second right")]
        public string Right2 { get; set; }

        [Display(Name = "Date Last purchase")]
        [UIHint("DatePicker")]
        public Nullable<DateTime> LatestDateOfPurchase { get; set; }


        [Display(Name = "Profit Amount")]
        public Nullable<long> Dividend { get; set; }

        [Display(Name = "Periodic interest")]
        public long PeriodicalDividend { get; set; }

        [Display(Name = "Permission to receive periodic interest")]
      
        public bool AllowGetPeriodicalDividend { get; set; }



        [Display(Name = "location")]
        public string LocationMLM { get; set; }

        [Display(Name = "Card number")]
        [StringLength(16, ErrorMessage = "The card code must be 16 digits", MinimumLength = 16)]

        public string kartNumber { get; set; }

        [Display(Name = "account number")]
        public string AccountNumber { get; set; }

        [Display(Name = "xxx")]
        [NotMapped]
        public bool IsCommingFromShopingCart { get; set; }


        [StringLength(128)]
        [Display(Name = "first parent")]
        public string P1 { get; set; }

        [StringLength(128)]
        [Display(Name = "second parent")]
        public string P2 { get; set; }

        [StringLength(128)]
        [Display(Name = "third parent")]
        public string P3 { get; set; }


        [StringLength(128)]
        [Display(Name = "fourth parent")]
        public string P4 { get; set; }

        [StringLength(128)]
        [Display(Name = "fifth parent")]
        public string P5 { get; set; }
        [StringLength(128)]
        [Display(Name = "sixth parent")]
        public string P6 { get; set; }

        [StringLength(128)]
        [Display(Name = "seventh parent")]
        public string P7 { get; set; }

        [StringLength(128)]
        [Display(Name = "eighth parent")]
        public string P8 { get; set; }

        [StringLength(128)]
        [Display(Name = "ninth parent")]
        public string P9 { get; set; }



        [StringLength(128)]
        [Display(Name = "tenth parent")]
        public string P10 { get; set; }


   
        #endregion

        #region // Relations  of tables
        //Navigation Properties
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
        #endregion
    }
    #endregion

    #region // Roles
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
    }
    #endregion

    #region Db Context class
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MLM_db", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, MLM_app.Migrations.Configuration>("MLM_db"));
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        #region Create Tables
        /// <summary>
        /// Create a generic collection of type Country with the name Countries.
        /// The type of this collection is from DbSet, but you can also create any other generic collection.
        /// Other collections such as HashTable, List, IEnumerable, Dictionary are also supported.
        /// </summary>
        // For creating tables
        public DbSet<Page> Pages { get; set; }

        public DbSet<language> languages { get; set; }
        public DbSet<PageGroup> PageGroups { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ReciptionMethod> ReciptionMethods { get; set; }

        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<SpecificPurchaseDay> SpecificPurchaseDays { get; set; }
        public DbSet<SubUserPassword> SubUserPassword { get; set; }

        public DbSet<PeriodOfProfit> PeriodOfProfits { get; set; }
        //PurchaseDay

        #endregion

        #region Relations  of tables 
        /// <summary>
        /// Override of the model creating method.
        /// In this method, we add configurations present in the model classes to the application.
        /// </summary>
        /// <param name="modelBuilder"></param>
        // I use Fluent API for internal one-to-many relationships.
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>().
              HasOptional(e => e.Parent).
              WithMany().
              HasForeignKey(m => m.ParentId);

            modelBuilder.Entity<ProductGroup>().
             HasOptional(e => e.Parent).
             WithMany().
             HasForeignKey(m => m.ParentId);
        }
    }
        #endregion
    #endregion
}