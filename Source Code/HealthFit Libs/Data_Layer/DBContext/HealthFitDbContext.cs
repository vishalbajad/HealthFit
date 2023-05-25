﻿using HealthFit.Object_Provider.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Data_Layer.DBContext
{
    public class HealthFitDbContext : DbContext
    {
        public DbSet<Journal> Journals { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSubscriptionsDetails> UserSubscriptionsDetails { get; set; }

        public HealthFitDbContext(string connectionString) : base(GetOptions(connectionString))
        {
            this.Database.EnsureCreated();
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSubscriptionsDetails>().HasKey(usd => new { usd.JournalId, usd.UserId });
            modelBuilder.Entity<UserSubscriptionsDetails>().HasOne(usd => usd.Journals).WithMany(b => b.Subscribers).HasForeignKey(bs => bs.JournalId);
            modelBuilder.Entity<UserSubscriptionsDetails>().HasOne(bs => bs.Users).WithMany(s => s.Journals).HasForeignKey(bs => bs.UserId);

            modelBuilder.Entity<UserSubscriptionsDetails>().HasIndex(e => new { e.UserId, e.JournalId }).IsUnique();
        }
    }
}
