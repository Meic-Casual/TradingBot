using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Models.Scaffolded;

public partial class DcaBotContext : DbContext
{
    public DcaBotContext(DbContextOptions<DcaBotContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bot> Bots { get; set; }

    public virtual DbSet<Botstatesnapshot> Botstatesnapshots { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Bot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("bots");

            entity.HasIndex(e => e.OwnerId, "Bots_fk1");

            entity.HasIndex(e => e.Id, "id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CurrentAllocation)
                .HasPrecision(8, 2)
                .HasColumnName("current_allocation");
            entity.Property(e => e.MaxPriceBasePaddingPercent)
                .HasPrecision(2, 2)
                .HasColumnName("max_price_base_padding_percent");
            entity.Property(e => e.OverallAllowance)
                .HasPrecision(8, 2)
                .HasColumnName("overall_allowance");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");

            entity.HasOne(d => d.Owner).WithMany(p => p.Bots)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Bots_fk1");
        });

        modelBuilder.Entity<Botstatesnapshot>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("botstatesnapshots");

            entity.HasIndex(e => e.BotId, "BotStateSnapshots_fk0");

            entity.Property(e => e.AveragePurchasePrice)
                .HasPrecision(9, 2)
                .HasColumnName("average_purchase_price");
            entity.Property(e => e.BotId).HasColumnName("bot_id");
            entity.Property(e => e.LowestPurchasePrice)
                .HasPrecision(9, 2)
                .HasColumnName("lowest_purchase_price");
            entity.Property(e => e.RemainingAllowance)
                .HasPrecision(8, 2)
                .HasColumnName("remaining_allowance");

            entity.HasOne(d => d.Bot).WithMany()
                .HasForeignKey(d => d.BotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BotStateSnapshots_fk0");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("purchases");

            entity.HasIndex(e => e.BotId, "Purchases_fk1");

            entity.HasIndex(e => e.Id, "id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BotId).HasColumnName("bot_id");
            entity.Property(e => e.Cost)
                .HasPrecision(9, 2)
                .HasColumnName("cost");
            entity.Property(e => e.Price)
                .HasPrecision(9, 2)
                .HasColumnName("price");
            entity.Property(e => e.PurchasedAt)
                .HasColumnType("datetime")
                .HasColumnName("purchased_at");
            entity.Property(e => e.Quantity)
                .HasPrecision(9, 8)
                .HasColumnName("quantity");

            entity.HasOne(d => d.Bot).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.BotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Purchases_fk1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.EmailHash, "email_hash").IsUnique();

            entity.HasIndex(e => e.Id, "id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmailHash)
                .HasMaxLength(128)
                .HasColumnName("email_hash");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(256)
                .HasColumnName("password_hash");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
