﻿// <auto-generated />
using System;
using HRDepartment.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HRDepartment.Core.Migrations
{
    [DbContext(typeof(HrDepartmentContext))]
    partial class HrDepartmentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.15");

            modelBuilder.Entity("HRDepartment.Model.Department", b =>
                {
                    b.Property<long>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<long>("BossId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.HasIndex("BossId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("HRDepartment.Model.Employee", b =>
                {
                    b.Property<long>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Fio")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("HRDepartment.Model.EmployeeLog", b =>
                {
                    b.Property<long>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("DepartmentId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateOfDismissal")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Fired")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Key")
                        .HasColumnType("INTEGER");

                    b.HasKey("EmployeeId", "DepartmentId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("EmployeeLog");
                });

            modelBuilder.Entity("HRDepartment.Model.Department", b =>
                {
                    b.HasOne("HRDepartment.Model.Employee", "Boss")
                        .WithMany()
                        .HasForeignKey("BossId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HRDepartment.Model.EmployeeLog", b =>
                {
                    b.HasOne("HRDepartment.Model.Department", "Department")
                        .WithMany("EmployeeLogs")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HRDepartment.Model.Employee", "Employee")
                        .WithMany("EmployeeLogs")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
