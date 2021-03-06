using System;
using Microsoft.EntityFrameworkCore;

namespace Revature.Tenant.DataAccess.Entities
{
  public class TenantContext : DbContext
  {
    public TenantContext()
    {
    }

    public TenantContext(DbContextOptions<TenantContext> options)
      : base(options)
    {
    }

    public virtual DbSet<Tenant> Tenant { get; set; }

    public virtual DbSet<Car> Car { get; set; }

    public virtual DbSet<Batch> Batch { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Tenant>(entity =>
      {
        entity.HasKey(t => t.Id);
        entity.Property(t => t.Email).IsRequired();
        entity.Property(t => t.Gender).IsRequired();
        entity.Property(t => t.FirstName).IsRequired().HasMaxLength(100);
        entity.Property(t => t.LastName).IsRequired().HasMaxLength(100);
        entity.Property(t => t.TrainingCenter).IsRequired();
        entity.Property(t => t.RoomId);
        entity.HasOne(t => t.Car).WithOne(c => c.Tenant).HasForeignKey<Tenant>(t => t.CarId);
        entity.HasOne(t => t.Batch).WithMany(b => b.Tenant).HasForeignKey(t => t.BatchId);

        entity.HasData(
          new Tenant()
          {
            AddressId = Guid.Parse("1a4d6c6e-9640-44c9-8c6b-5aebd3f9a67e"),
            BatchId = 1,
            CarId = null,
            Email = "revtestfour2020@gmail.com",
            FirstName = "Test",
            Gender = "Male",
            Id = Guid.Parse("5a64ba93-5f78-4972-9e80-ad3cb4678923"),
            LastName = "Four",
            RoomId = Guid.Parse("fa1d6c6e-9650-44c9-8c6b-5aebd3f9a671"),
            TrainingCenter = Guid.Parse("837c3248-1685-4d08-934a-0f17a6d1836a")
          },
          new Tenant()
          {
            AddressId = Guid.Parse("50b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
            BatchId = 2,
            CarId = -2,
            Email = "revtestfour2020+1@gmail.com",
            FirstName = "Test",
            Gender = "Female",
            Id = Guid.Parse("9f055e5f-92be-4ccc-85f4-b98288981822"),
            LastName = "Seven",
            RoomId = Guid.Parse("0a4d6c61-9650-44c9-8c6b-5aebd3f9a676"),
            TrainingCenter = Guid.Parse("837c3248-1685-4d08-934a-0f17a6d1836a")
          },
          new Tenant()
          {
            AddressId = Guid.Parse("52b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
            BatchId = 2,
            CarId = -1,
            Email = "revtestfour2020+2@gmail.com",
            FirstName = "Test",
            Gender = "Female",
            Id = Guid.Parse("f4154445-51eb-4303-998c-38e9fb817063"),
            LastName = "Eight",
            RoomId = Guid.Parse("249e5358-169a-4bc6-aa0f-c054952456ee"),
            TrainingCenter = Guid.Parse("837c3248-1685-4d08-934a-0f17a6d1836a")
          });
      });

      modelBuilder.Entity<Car>(entity =>
      {
        entity.HasKey(c => c.Id);
        entity.Property(c => c.Id).UseIdentityColumn();
        entity.Property(c => c.LicensePlate).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Make).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Model).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Color).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Year).IsRequired();
        entity.Property(c => c.State).IsRequired();

        entity.HasData(
          new Car()
          {
            Color = "White",
            Id = -1,
            LicensePlate = "ABC123",
            Make = "Ford",
            Model = "F150",
            State = "LA",
            Year = "1996"
          },
          new Car()
          {
            Color = "Orange",
            Id = -2,
            LicensePlate = "DEF456",
            Make = "Honda",
            Model = "VTX1300",
            State = "TX",
            Year = "2006"
          });
      });

      modelBuilder.Entity<Batch>(entity =>
     {
       entity.HasKey(b => b.Id);
       entity.Property(b => b.Id).UseIdentityColumn();
       entity.Property(b => b.BatchCurriculum).IsRequired().HasMaxLength(100);
       entity.Property(b => b.StartDate).IsRequired();
       entity.Property(b => b.EndDate).IsRequired();
       entity.Property(b => b.TrainingCenter).IsRequired();

       entity.HasData(
         new Batch()
         {
           BatchCurriculum = "C#",
           EndDate = new DateTime(2019, 12, 30),
           Id = 1,
           StartDate = new DateTime(2019, 09, 30),
           TrainingCenter = Guid.Parse("837c3248-1685-4d08-934a-0f17a6d1836a")
         },
         new Batch()
         {
           BatchCurriculum = "Java",
           EndDate = new DateTime(2019, 11, 30),
           Id = 2,
           StartDate = new DateTime(2019, 08, 30),
           TrainingCenter = Guid.Parse("837c3248-1685-4d08-934a-0f17a6d1836a")
         });
     });
    }
  }
}
