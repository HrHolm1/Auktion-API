using Auktion_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Auktion_API.DataAccess;

public class AuctionContext : DbContext
{
    public AuctionContext(DbContextOptions<AuctionContext> options) : base(options) { }

    public DbSet<Models.Auction> Auctions => Set<Auction>();
    public DbSet<Models.Lot> Lots => Set<Lot>();
    public DbSet<Models.Bid> Bids => Set<Bid>();
    public DbSet<Models.User> Users => Set<User>();

    public DbSet<LotImage> LotImages => Set<LotImage>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("auctions");

        // --- Auction ---
        modelBuilder.Entity<Auction>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Title).IsRequired().HasMaxLength(200);
            entity.Property(a => a.Description).HasMaxLength(2000);

            entity
                .HasMany(a => a.Lots)
                .WithOne()
                .HasForeignKey(l => l.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // --- Lot ---
        modelBuilder.Entity<Lot>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Title).IsRequired().HasMaxLength(200);
            entity.Property(l => l.Description).HasMaxLength(2000);
            entity.Property(l => l.StartingPrice).HasDefaultValue(1);
            entity.Property(l => l.EndingPrice);

            entity.Property(l => l.EndTime)
                .IsRequired();

            entity.Property(l => l.IsClosed)
                .HasDefaultValue(false);
            
            entity.HasIndex(l => new { l.AuctionId, l.LotNumber }).IsUnique();

            // Winner stuff
            entity
                .HasOne(l => l.Winner)
                .WithMany(u => u.WonLots)
                .HasForeignKey(l => l.WinnerUserId)
                .OnDelete(DeleteBehavior.SetNull); // if user is deleted, keep lot but clear winner

            // Image stuff
            entity
                .HasMany(l => l.Images)
                .WithOne(i => i.Lot)
                .HasForeignKey(i => i.LotId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // --- LotImage ---
        modelBuilder.Entity<LotImage>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.FileName).IsRequired().HasMaxLength(255);
            entity.Property(i => i.Url).IsRequired().HasMaxLength(500);
        });

        // --- Bid ---
        modelBuilder.Entity<Bid>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Amount).IsRequired();
            entity.Property(b => b.PlacedAt).IsRequired();
            entity.Property(b => b.UserId).IsRequired();

            entity
                .HasOne<Lot>() // no navigation property
                .WithMany() // no reverse navigation either
                .HasForeignKey(b => b.LotId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(b => b.User)
                .WithMany(u => u.Bids)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // --- User ---
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("user");

            entity.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(500);
        });
    }
}