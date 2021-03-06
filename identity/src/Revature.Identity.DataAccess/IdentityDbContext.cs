using Microsoft.EntityFrameworkCore;
using Revature.Identity.DataAccess.Entities;

namespace Revature.Identity.DataAccess
{
  /// <summary>
  /// Context class that acts to define the structure of the database.
  /// </summary>
  public class IdentityDbContext : DbContext
  {
    public IdentityDbContext()
    {
    }

    // Constructor with options and inheritance from its parent class.
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
      : base(options)
    {
    }

    // All tables found in the database defined here
    public virtual DbSet<Notification> Notification { get; set; }

    public virtual DbSet<UpdateAction> UpdateAction { get; set; }

    public virtual DbSet<ProviderAccount> ProviderAccount { get; set; }

    public virtual DbSet<CoordinatorAccount> CoordinatorAccount { get; set; }

    public virtual DbSet<TenantAccount> TenantAccount { get; set; }

    /// <summary>
    /// Defines the features for each and every table.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<TenantAccount>(entity =>
      {
        entity.HasKey(e => e.TenantId);
        entity.Property(e => e.Name)
          .IsRequired()
          .HasMaxLength(100);
        entity.Property(e => e.Email)
          .IsRequired()
          .HasMaxLength(100);
        entity.HasData(new TenantAccount[]
        {
          new TenantAccount()
          {
            TenantId = System.Guid.Parse("5a64ba93-5f78-4972-9e80-ad3cb4678923"),
            Name = "Test Four",
            Email = "revtestfour2020@gmail.com"
          },
          new TenantAccount()
          {
            TenantId = System.Guid.Parse("9f055e5f-92be-4ccc-85f4-b98288981822"),
            Name = "Test Seven",
            Email = "revtestfour2020+1@gmail.com"
          },
          new TenantAccount()
          {
            TenantId = System.Guid.Parse("f4154445-51eb-4303-998c-38e9fb817063"),
            Name = "Test Eight",
            Email = "revtestfour2020+2@gmail.com"
          }
        });
      });

      modelBuilder.Entity<ProviderAccount>(entity =>
      {
        entity.HasKey(e => e.ProviderId);
        entity.HasOne(e => e.Coordinator)
          .WithMany(e => e.Providers)
          .HasForeignKey(p => p.ProviderId)
          .IsRequired(false);
        entity.Property(e => e.Name)
          .IsRequired()
          .HasMaxLength(100);
        entity.Property(e => e.Email)
          .IsRequired()
          .HasMaxLength(100);
        entity.Property(e => e.StatusText);
        entity.Property(e => e.AccountCreatedAt)
          .IsRequired();
        entity.HasData(new ProviderAccount[]
        {
          new ProviderAccount()
          {
            ProviderId = System.Guid.Parse("dfc872fc-b708-4caf-b3f1-3c842c8d3078"),
            Name = "Test_Three",
            StatusText = "Approved",
            Email = "revtestthree2020@gmail.com",
            AccountCreatedAt = System.DateTime.Now
          },
          new ProviderAccount()
          {
            ProviderId = System.Guid.Parse("68b95c3a-af06-44ac-8ba0-4d3ea2d53c39"),
            Name = "Test_Two",
            StatusText = "Pending",
            Email = "revtesttwo2020@gmail.com",
            AccountCreatedAt = System.DateTime.Now
          },
          new ProviderAccount()
          {
            ProviderId = System.Guid.Parse("f7631719-b74b-46c4-bb89-6b3a3681ac06"),
            Name = "Test_Six",
            StatusText = "Approved",
            Email = "revtesttwo2020@gmail.com",
            AccountCreatedAt = System.DateTime.Now
          }
        });
      });

      modelBuilder.Entity<CoordinatorAccount>(entity =>
      {
        entity.HasKey(e => e.CoordinatorId);
        entity.Property(e => e.Name)
          .IsRequired()
          .HasMaxLength(100);
        entity.Property(e => e.Email)
          .IsRequired()
          .HasMaxLength(100);
        entity.Property(e => e.TrainingCenterName)
          .IsRequired()
          .HasMaxLength(100);
        entity.Property(e => e.TrainingCenterAddress)
          .IsRequired()
          .HasMaxLength(100);
        entity.HasMany(e => e.Notifications)
          .WithOne(n => n.Coordinator)
          .HasForeignKey(n => n.NotificationId);
        entity.HasMany(e => e.Providers)
          .WithOne(p => p.Coordinator)
          .HasForeignKey(p => p.CoordinatorId);

        entity.HasData(new CoordinatorAccount[]
        {
          new CoordinatorAccount()
          {
            CoordinatorId = System.Guid.Parse("7d673549-be58-41d2-9ac0-0ecede8b27be"),
            Name = "Test_Five",
            Email = "revtestone2020+1@gmail.com",
            TrainingCenterName = "UTA",
            TrainingCenterAddress = "300 W Martin Luther King Jr Blvd, Austin, TX 78705"
          },
          new CoordinatorAccount()
          {
            CoordinatorId = System.Guid.Parse("5d7189a2-5f16-461a-a81e-27bb1c8b4074"),
            Name = "Test_One",
            Email = "revtestone2020@gmail.com",
            TrainingCenterName = "UTA",
            TrainingCenterAddress = "300 W Martin Luther King Jr Blvd, Austin, TX 78705"
          },
        });
      });

      modelBuilder.Entity<Notification>(entity =>
      {
        entity.HasKey(e => e.NotificationId);
        entity.Property(e => e.ProviderId)
          .IsRequired();
        entity.Property(e => e.CoordinatorId)
          .IsRequired();
        entity.Property(e => e.UpdateActionId)
          .IsRequired();
        entity.Property(e => e.CreatedAt)
          .IsRequired();
        entity.HasOne(e => e.Coordinator)
          .WithMany(c => c.Notifications)
          .HasForeignKey(c => c.CoordinatorId)
          .IsRequired();
        entity.HasOne(e => e.Provider)
          .WithMany(p => p.Notifications)
          .HasForeignKey(p => p.ProviderId)
          .IsRequired();
        entity.HasOne(e => e.UpdateAction)
          .WithOne(u => u.Notification)
          .HasForeignKey<UpdateAction>(u => u.UpdateActionId)
          .IsRequired();
        entity.Property(e => e.StatusText);
      });

      modelBuilder.Entity<UpdateAction>(entity =>
      {
        entity.HasKey(e => e.UpdateActionId);
        entity.Property(e => e.UpdateType)
          .IsRequired();
        entity.Property(e => e.SerializedTarget)
          .IsRequired();
        entity.HasOne(e => e.Notification)
          .WithOne(n => n.UpdateAction)
          .HasForeignKey<Notification>(n => n.UpdateActionId);
      });
    }
  }
}
