using System;
using System.IO;
using HRDepartment.Model;
using HRDepartment.Model.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HRDepartment.Core.Context
{
    public class HrDepartmentContext : DbContext
    {
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeLog> EmployeeLog { get; set; }

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
            optionsBuilder.UseNpgsql(config.GetConnectionString("ConnectionDb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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