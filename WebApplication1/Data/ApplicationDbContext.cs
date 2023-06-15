using System.Collections.Generic;
using System.Reflection.Emit;
using Eindopdrachtcnd2.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Eindopdrachtcnd2.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }

        public DbSet<Card> Cards { get; set; }
        public DbSet<CardTask> CardTasks { get; set; }
        public DbSet<CardUser> CardUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedRoles(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasAlternateKey(c => c.Name);

                entity.HasMany(u => u.GroupUsers)
                    .WithOne(gu => gu.User)
                    .HasForeignKey(gu => gu.UserId);

                entity.HasMany(u => u.CardUsers)
                    .WithOne(cu => cu.User)
                    .HasForeignKey(cu => cu.UserId);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasAlternateKey(c => c.Name);

                entity.HasMany(g => g.GroupUsers)
                    .WithOne(gu => gu.Group)
                    .HasForeignKey(gu => gu.GroupId);
            });

            modelBuilder.Entity<GroupUser>()
                .HasKey(gu => new { gu.UserId, gu.GroupId });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasOne(g => g.Group)
                    .WithMany(gu => gu.Cards)
                    .HasForeignKey(gu => gu.GroupId);

                entity.HasMany(c => c.CardUsers)
                    .WithOne(cu => cu.Card)
                    .HasForeignKey(cu => cu.CardId);
            });

            modelBuilder.Entity<CardTask>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasOne(g => g.Card)
                    .WithMany(gu => gu.CardTasks)
                    .HasForeignKey(gu => gu.CardId);;
            });

            modelBuilder.Entity<CardUser>()
                .HasKey(cu => new { cu.CardId, cu.UserId });

        }
        private static void SeedRoles(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<IdentityRole>().HasData(
                               new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                               new IdentityRole() { Name = "GroupAdmin", ConcurrencyStamp = "2", NormalizedName = "GroupAdmin" },
                               new IdentityRole() { Name = "User", ConcurrencyStamp = "3", NormalizedName = "User" }
        );
        }
    }
}
