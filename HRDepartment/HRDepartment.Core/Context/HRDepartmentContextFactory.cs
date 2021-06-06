using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HRDepartment.Core.Context
{
    public class HrDepartmentContextFactory : IDesignTimeDbContextFactory<HrDepartmentContext>
    {
        public HrDepartmentContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            var builder = new DbContextOptionsBuilder<HrDepartmentContext>();
            builder.UseNpgsql(config.GetConnectionString("ConnectionDb"));
            return new HrDepartmentContext(builder.Options);
        }
    }
}