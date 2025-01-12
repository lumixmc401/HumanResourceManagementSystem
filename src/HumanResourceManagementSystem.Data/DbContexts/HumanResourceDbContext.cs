using HumanResourceManagementSystem.Data.Models.HumanResource;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace HumanResourceManagementSystem.Data.DbContexts
{
    public class HumanResourceDbContext : DbContext
    {
        public HumanResourceDbContext() { }
        public HumanResourceDbContext(DbContextOptions<HumanResourceDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId);

                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                entity.HasOne(rp => rp.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(rp => rp.RoleId);

                entity.HasOne(rp => rp.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(rp => rp.PermissionId);
            });

            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.HasKey(uc => new { uc.UserId, uc.ClaimId });

                entity.HasOne(uc => uc.User)
                    .WithMany(u => u.UserClaims)
                    .HasForeignKey(uc => uc.UserId);

                entity.HasOne(uc => uc.Claim)
                    .WithMany(c => c.UserClaims)
                    .HasForeignKey(uc => uc.ClaimId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(true);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasIndex(e => e.Email)
                   .IsUnique();
            });

            modelBuilder.Entity<Claim>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(true);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(true);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(true);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(true);
            });
        }
    }
}
