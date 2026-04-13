using CrmProject.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.DataAccess.Context
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<AppTask> AppTasks => Set<AppTask>();
        public DbSet<TaskLog> TaskLogs => Set<TaskLog>();
        public DbSet<Income> Incomes => Set<Income>();
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Role => Users 1-many
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            //apptask => project many-one
            modelBuilder.Entity<AppTask>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            //apptask => assignedByUser Many-One
            modelBuilder.Entity<AppTask>()
                .HasOne(t => t.AssignedByUser)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.AssignedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            //apptask => assignToUser many-many
            modelBuilder.Entity<AppTask>()
                .HasMany(t => t.AssignedUsers)
                .WithMany(u => u.AssignedTasks)
                .UsingEntity(j => j.ToTable("AppTaskAssignedUsers"));


            //tasklog => appTask many-one
            modelBuilder.Entity<TaskLog>()
                .HasOne(tl => tl.TaskItem)
                .WithMany(t => t.TaskLogs)
                .HasForeignKey(tl => tl.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);

            //Income=> Project Many-One
            modelBuilder.Entity<Income>()
                .HasOne(i => i.Project)
                .WithMany(p => p.Incomes)
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            //Expense=> Project Many-One
            modelBuilder.Entity<Expense>()
                .HasOne(i => i.Project)
                .WithMany(p => p.Expenses)
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            //Notification=> User Many-One
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(p => p.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //decimal precision
            modelBuilder.Entity<Income>()
                .Property(i => i.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- DATA SEED ---
            // GAP-B4: Admin seeded with BCrypt hash of "password"
            // Generated via: BCrypt.Net.BCrypt.HashPassword("password")
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "Admin" },
                new Role { Id = 2, RoleName = "Personel" }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@crm.com",
                    PasswordHash = "$2a$12$3DXZPBQJT35SYomkVoXwZO6u4EdT.dDSHYLdpLFLApTnij55ioHOa",
                    RoleId = 1,
                    IsActive = true,
                    RegistrationDate = new DateTime(2026, 1, 1)
                }
            );



        }
    }
}
