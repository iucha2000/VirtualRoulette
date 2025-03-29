using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Domain.Entities;
using VirtualRoulette.Domain.ValueObjects;

namespace VirtualRoulette.Infrastructure.Persistence
{
    public class VirtualRouletteDbContext : DbContext
    {
        public VirtualRouletteDbContext(DbContextOptions<VirtualRouletteDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Jackpot> Jackpots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //User entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Username).IsRequired().HasMaxLength(255);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(512);
                entity.Property(u => u.LastActivity).IsRequired();
                entity.Property(u => u.IsActive).IsRequired();

                entity.HasIndex(u => u.Username).IsUnique();
            });

            //Bet entity configuration
            modelBuilder.Entity<Bet>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(b => b.SpinId).IsRequired();
                entity.Property(b => b.BetDetails).IsRequired();
                entity.Property(b => b.BetAmount).IsRequired();
                entity.Property(b => b.IpAddress).IsRequired().HasMaxLength(45);
                entity.Property(b => b.CreatedAt).IsRequired();
                entity.Property(b => b.Status).IsRequired();
                entity.Property(b => b.WonAmount).IsRequired();

                entity.Property(b => b.Status).HasConversion<int>();
            });

            //Jackpot entity configuration
            modelBuilder.Entity<Jackpot>(entity =>
            {
                entity.HasKey(j => j.Id);

                entity.Property(j => j.Amount).IsRequired();
                entity.Property(j => j.UpdatedAt).IsRequired();
            });

            modelBuilder.Entity<User>()
                .Property(b => b.Balance)
                .HasConversion(v => v.Amount, v => new Money(v));
            modelBuilder.Entity<Bet>()
                .Property(b => b.BetAmount)
                .HasConversion(v => v.Amount, v => new Money(v));
            modelBuilder.Entity<Bet>()
                .Property(b => b.WonAmount)
                .HasConversion(v => v.Amount, v => new Money(v));
            modelBuilder.Entity<Jackpot>()
                .Property(j => j.Amount)
                .HasConversion(v => v.Amount, v => new Money(v));
        }
    }
}
