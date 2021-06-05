using System;
using System.IO;
using HRDepartment.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HRDepartment.Core.Context
{
    public class HrDepartmentContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeLog> EmployeeLog { get; set; }

        public HrDepartmentContext(DbContextOptions<HrDepartmentContext> dbContextOptions)
        {
            //Database.EnsureDeleted();
           // Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            optionsBuilder.UseSqlite(config.GetConnectionString("ConnectionDb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeLog>()
                .HasKey(t => new {t.EmployeeId, t.DepartmentId});

            modelBuilder.Entity<EmployeeLog>()
                .HasOne(sc => sc.Department)
                .WithMany(c => c.EmployeeLogs)
                .HasForeignKey(t => t.DepartmentId);
            
            modelBuilder.Entity<EmployeeLog>()
                .HasOne(sc => sc.Employee)
                .WithMany(c => c.EmployeeLogs)
                .HasForeignKey(t => t.EmployeeId);
        }
    }
}