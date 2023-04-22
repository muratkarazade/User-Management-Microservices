using Enoca.DatabaseManagment.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;

namespace Enoca.DatabaseManagment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.Id).UseIdentityColumn(1,1);
           

            // Örnek verileri ekleyin
            modelBuilder.Entity<User>().HasData(
                 new User { Id = 1, Username = "user1", Password = "password1", Email = "user1@example.com", Name = "Gandalf", Surname = "The Gray" },
                new User { Id = 2, Username = "user2", Password = "password2", Email = "user2@example.com", Name = "Frodo", Surname = "Baggins" },
                new User { Id = 3, Username = "user3", Password = "password3", Email = "user3@example.com", Name = "Elrond", Surname = "Smith" }
            );

          
        }
       
    }
}
