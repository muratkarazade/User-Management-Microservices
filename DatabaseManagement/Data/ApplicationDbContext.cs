using DatabaseManagement.Controllers;
using DatabaseManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;

namespace DatabaseManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<AuthenticationCredential> AuthenticationCredentials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.Id).UseIdentityColumn(1,1);
            modelBuilder.Entity<AuthenticationCredential>().Property(ac => ac.Id).UseIdentityColumn(1,1);
            // İlişkileri yapılandırın
            modelBuilder.Entity<AuthenticationCredential>()
                .HasOne(ac => ac.User)
                .WithOne()
                .HasForeignKey<AuthenticationCredential>(ac => ac.UserId);

            // Örnek verileri ekleyin
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "user1@example.com", Name = "User", Surname = "One" },
                new User { Id = 2, Email = "user2@example.com", Name = "User", Surname = "Two" },
                new User { Id = 3, Email = "user3@example.com", Name = "User", Surname = "Three" }
            );

            modelBuilder.Entity<AuthenticationCredential>().HasData(
                new AuthenticationCredential { Id = 1, UserId = 1, Username = "user1", Password = "password1" },
                new AuthenticationCredential { Id = 2, UserId = 2, Username = "user2", Password = "password2" },
                new AuthenticationCredential { Id = 3, UserId = 3, Username = "user3", Password = "password3" }
            );
        }
       
    }
}
