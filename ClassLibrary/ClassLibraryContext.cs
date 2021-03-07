using TasksSystem.Models;
using Microsoft.EntityFrameworkCore;
using ClassLibrary.Models;

namespace ClassLibrary.Data
{

    public class ClassLibraryContext : DbContext
    {
        public ClassLibraryContext() : base() { }

        public ClassLibraryContext(DbContextOptions<ClassLibraryContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDb>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<ProjectDb>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<ProjectUsersDb>()
                .HasKey(x => new { x.ProjectId, x.UserId });
            modelBuilder.Entity<ProjectUsersDb>()
                .HasOne(x => x.User)
                .WithMany(m => m.Projects)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<ProjectUsersDb>()
                .HasOne(x => x.Project)
                .WithMany(m => m.Users)
                .HasForeignKey(x => x.ProjectId);

            //__________________________________


            string AdminRoleName = "Admin";
            string UserRoleName = "User";
            string GuestRoleName = "Guest";

            RoleDb AdminRole = new RoleDb { Name = AdminRoleName };
            RoleDb UserRole = new RoleDb { Name = UserRoleName };
            RoleDb GuestRole = new RoleDb { Name = GuestRoleName };

            modelBuilder.Entity<RoleDb>().HasData(new RoleDb[] { AdminRole, UserRole, GuestRole });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TaskDb> Task { get; set; }
        public DbSet<UserDb> User { get; set; }
        public DbSet<RoleDb> Role { get; set; }
        public DbSet<ProjectDb> Project { get; set; }
        public DbSet<CommentDb> Comments { get; set; }
        public DbSet<ProjectUsersDb> ProjectUsers { get; set; }
    }
}
