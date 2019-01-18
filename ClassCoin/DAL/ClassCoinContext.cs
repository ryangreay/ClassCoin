using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ClassCoin.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ClassCoin.DAL
{
    public class ClassCoinContext : IdentityDbContext<User>  //DbContext
    {
        public ClassCoinContext() : base("ClassCoinContext")
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Store> Stores { get; set; }
        //public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("AspNetUserRoles");

            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable("AspNetUserLogins");
        }

        public static ClassCoinContext Create()
        {
            return new ClassCoinContext();
        }

    }
}