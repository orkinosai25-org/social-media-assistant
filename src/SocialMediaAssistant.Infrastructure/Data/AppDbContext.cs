using Microsoft.EntityFrameworkCore;
using SocialMediaAssistant.Core.Entities;

namespace SocialMediaAssistant.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Seller> Sellers => Set<Seller>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<SocialAccount> SocialAccounts => Set<SocialAccount>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
            entity.Property(x => x.SKU).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Price).HasColumnType("decimal(18,2)");
            entity.HasOne(x => x.Seller).WithMany(x => x.Products).HasForeignKey(x => x.SellerId);
            entity.HasIndex(x => new { x.SellerId, x.SKU }).IsUnique();
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.ExternalConversationId).IsRequired().HasMaxLength(200);
            entity.HasOne(x => x.Seller).WithMany(x => x.Conversations).HasForeignKey(x => x.SellerId);
            entity.HasIndex(x => new { x.ExternalConversationId, x.Channel });
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Content).IsRequired();
            entity.HasOne(x => x.Conversation).WithMany(x => x.Messages).HasForeignKey(x => x.ConversationId);
        });

        modelBuilder.Entity<SocialAccount>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Seller).WithMany(x => x.SocialAccounts).HasForeignKey(x => x.SellerId);
            entity.HasIndex(x => new { x.SellerId, x.Channel }).IsUnique();
        });
    }
}
