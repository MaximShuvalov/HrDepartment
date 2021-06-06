using System;
using System.IO;
using HRDepartment.Core.Context;
using HRDepartment.Core.Services;
using HRDepartment.Core.UnitOfWork;
using HRDepartment.Impl.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRDepartment.Impl.IoC
{
    public static class HrDepartmentServiceCollectionExt
    {
        public static IServiceCollection AddImpl(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDepartmentService, DepartmentService>();
            serviceCollection.AddTransient<IEmployeeService, EmployeeService>();
            serviceCollection.AddTransient<IEmployeeLogService, EmployeeLogService>();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddDbContext<HrDepartmentContext>(
                options => options.UseNpgsql(config.GetConnectionString("ConnectionDb"))
            );

            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();

            return serviceCollection;
        }
    }
}