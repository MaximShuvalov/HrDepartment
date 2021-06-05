using System;
using System.IO;
using AutoMapper;
using HRDepartment.Core.Context;
using HRDepartment.Core.Repositories;
using HRDepartment.Core.Services;
using HRDepartment.Core.UnitOfWork;
using HRDepartment.Impl.Mapping;
using HRDepartment.Impl.Repositories;
using HRDepartment.Impl.Services;
using HRDepartment.Model;
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

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddDbContext<HrDepartmentContext>(
                options => options.UseSqlite(config.GetConnectionString("ConnectionDb"))
            );

            serviceCollection.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile(provider.GetService<IDepartmentService>()));
            }).CreateMapper());

            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();

            return serviceCollection;
        }
    }
}