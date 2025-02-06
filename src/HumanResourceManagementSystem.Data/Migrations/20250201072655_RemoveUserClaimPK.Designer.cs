﻿// <auto-generated />
using System;
using HumanResourceManagementSystem.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HumanResourceManagementSystem.Data.Migrations
{
    [DbContext(typeof(HumanResourceDbContext))]
    [Migration("20250201072655_RemoveUserClaimPK")]
    partial class RemoveUserClaimPK
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HumanResourceManagementSystem.Data.Models.HumanResource.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("HumanResourceManagementSystem.Data.Models.HumanResource.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("HumanResourceManagementSystem.Data.Models.HumanResource.RolePermission", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("HumanResourceManagementSystem.Data.Models.HumanResource.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HumanResourceManagementSystem.Data.Models.HumanResource.UserClaim", b =>
                {
                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("HumanResourceManagementSystem.Data.Models.HumanResource.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("HumanResourceManagementSystem.Data.Models.HumanResource.RolePermission", b =>
                {
                    b.HasOne("HumanResourceManagementSystem.Data.Models.HumanResource.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HumanResourceManagementSystem.Data.Models.HumanResource.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("HumanResourceManagementSystem.Data.Models.HumanResource.UserRole", b =>
                {
                    b.HasOne("HumanResourceManagementSystem.Data.Models.HumanResource.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HumanResourceManagementSystem.Data.Models.HumanResource.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HumanResourceManagementSystem.Data.Models.HumanResource.Permission", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("HumanResourceManagementSystem.Data.Models.HumanResource.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("HumanResourceManagementSystem.Data.Models.HumanResource.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
