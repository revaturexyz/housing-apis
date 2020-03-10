using Microsoft.EntityFrameworkCore;
using System;

namespace Revature.Lodging.DataAccess.Entities
{
  public class LodgingDbContext : DbContext
  {
    public LodgingDbContext() { }

    public LodgingDbContext(DbContextOptions<LodgingDbContext> options)
    : base(options) { }

    public virtual DbSet<Complex> Complex { get; set; }
    public virtual DbSet<ComplexAmenity> ComplexAmenity { get; set; }
    public virtual DbSet<RoomAmenity> RoomAmenity { get; set; }
    public virtual DbSet<Amenity> Amenity { get; set; }
    public virtual DbSet<Gender> Gender { get; set; }
    public virtual DbSet<Room> Room { get; set; }
    public virtual DbSet<RoomType> RoomType { get; set; }

    private Guid cId1 = Guid.Parse("b5e050aa-6bfc-46ad-9a69-90b1f99ed606");
    //cId1 equals to room service seed data: complex Id
    private Guid cId2 = Guid.Parse("68b7eadd-30ce-49a7-9b8c-bae1d47f46a6");
    private Guid cId3 = Guid.Parse("78b7eadd-30ce-49a7-9b8c-bae1d47f46a6");
    private Guid cId4 = Guid.Parse("88b7eadd-30ce-49a7-9b8c-bae1d47f46a6");
    private Guid rId1 = Guid.Parse("249e5358-169a-4bc6-aa0f-c054952456fd");
    private Guid rId2 = Guid.Parse("fa1d6c6e-9650-44c9-8c6b-5aebd3f9a671");
    //rId1, rId2 equals to room service: room id#1 & room id#2
    private Guid amId1 = Guid.Parse("b8b7eadd-30ce-49a7-9b8c-bae1d47f46a6");
    private Guid amId2 = Guid.Parse("c8b7eadd-30ce-49a7-9b8c-bae1d47f46a6");
    private Guid amId3 = Guid.Parse("d8b7eadd-30ce-49a7-9b8c-bae1d47f46a6");
    private Guid amId4 = Guid.Parse("e8b7eadd-30ce-49a7-9b8c-bae1d47f46a6");
    private Guid amId5 = Guid.Parse("f8b7eadd-30ce-49a7-9b8c-bae1d47f46a6");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      //Amenity
      modelBuilder.Entity<Amenity>(entity =>
      {
        entity.Property(e => e.Id)
          .IsRequired();

        entity.HasIndex(e => e.Id)
          .IsUnique();

        entity.Property(e => e.AmenityType)
          .IsRequired()
          .HasMaxLength(50);

        entity.Property(e => e.Description)
          .HasMaxLength(100);

        entity.HasData
        (
          new Amenity { Id = amId1, AmenityType = "fridge", Description = "to keep food fresh" },
          new Amenity { Id = amId2, AmenityType = "microwave", Description = "" },
          new Amenity { Id = amId3, AmenityType = "pool", Description = "swimming" },
          new Amenity { Id = amId4, AmenityType = "kitchen", Description = "cook" },
          new Amenity { Id = amId5, AmenityType = "gym", Description = "work out" }
        );
      });

      //Amenity Complex
      modelBuilder.Entity<ComplexAmenity>(entity =>
      {
        entity.Property(e => e.Id)
          .IsRequired();

        entity.HasIndex(c => c.Id)
          .IsUnique();

        entity.HasOne(e => e.Amenity)
          .WithMany(d => d.ComplexAmenity)
          .HasForeignKey(p => p.AmenityId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Complex)
          .WithMany(d => d.ComplexAmenity)
          .HasForeignKey(p => p.ComplexId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        entity.HasData
        (
          new ComplexAmenity { Id = Guid.Parse("58b7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = amId1, ComplexId = cId1 },
          new ComplexAmenity { Id = Guid.Parse("59b7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = amId2, ComplexId = cId1 },
          new ComplexAmenity { Id = Guid.Parse("5ab7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = amId2, ComplexId = cId2 },
          new ComplexAmenity { Id = Guid.Parse("5bb7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = amId4, ComplexId = cId2 }
        );
      });

      modelBuilder.Entity<RoomAmenity>(entity =>
      {
        entity.Property(e => e.Id)
          .IsRequired();

        entity.HasIndex(c => c.Id)
          .IsUnique();

        entity.HasOne(e => e.Amenity)
          .WithMany(d => d.RoomAmenity)
          .HasForeignKey(p => p.AmenityId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Room)
          .WithMany(d => d.RoomAmenity)
          .HasForeignKey(p => p.RoomId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);


        entity.HasData
        (
          new RoomAmenity { Id = Guid.Parse("5cb7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = amId1, RoomId = rId1 },
          new RoomAmenity { Id = Guid.Parse("5db7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = amId4, RoomId = rId1 },
          new RoomAmenity { Id = Guid.Parse("5eb7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = amId5, RoomId = rId2 },
          new RoomAmenity { Id = Guid.Parse("5fb7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = amId2, RoomId = rId2 }
        );
      });

      modelBuilder.Entity<Complex>(entity =>
      {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.AddressId)
          .IsRequired();

        entity.Property(e => e.ProviderId)
          .IsRequired();

        entity.Property(e => e.ComplexName)
          .IsRequired()
          .HasMaxLength(50);

        entity.Property(e => e.ContactNumber)
          .HasMaxLength(20);

        entity.HasData(
          new Complex
          {
            Id = cId1,
            AddressId = Guid.Parse("50b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
            ProviderId = Guid.Parse("51b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
            ComplexName = "Liv+",
            ContactNumber = "8177517911"
          },
          new Complex
          {
            Id = cId2,
            AddressId = Guid.Parse("52b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
            ProviderId = Guid.Parse("53b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
            ComplexName = "SampleComplex",
            ContactNumber = "4445550506"
          },
          new Complex
          {
            Id = cId3,
            AddressId = Guid.Parse("54b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
            ProviderId = Guid.Parse("55b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
            ComplexName = "Complex",
            ContactNumber = "7771112222"
          },
          new Complex
          {
            Id = cId4,
            AddressId = Guid.Parse("56b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
            ProviderId = Guid.Parse("57b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
            ComplexName = "ComplexNearMe",
            ContactNumber = "3332221111"
          }
        );
      });

      //Gender
      modelBuilder.Entity<Gender>(entity =>
      {
        entity.HasKey(e => e.Id);

        entity.HasIndex(e => e.Id)
          .IsUnique();

        entity.Property(e => e.Type)
          .IsRequired()
          .HasMaxLength(50);

        entity.HasData(
         new Gender() { Id = 1, Type = "Male" },
         new Gender() { Id = 2, Type = "Female" }
         );

      });

      //Room
      modelBuilder.Entity<Room>(entity =>
      {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.RoomNumber)
          .IsRequired()
          .HasMaxLength(50);

        entity.Property(e => e.NumberOfBeds)
          .IsRequired();

        entity.Property(e => e.NumberOfOccupants)
          .IsRequired();

        entity.Property(e => e.LeaseStart);

        entity.Property(e => e.LeaseEnd);

        entity.HasOne(e => e.Complex)
          .WithMany(d => d.Room)
          .HasForeignKey(p => p.ComplexId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Gender)
          .WithMany(d => d.Room)
          .HasForeignKey(p => p.GenderId);

        entity.HasOne(e => e.RoomType)
          .WithMany(d => d.Room)
          .HasForeignKey(p => p.RoomTypeId)
          .IsRequired();

        entity.HasData(
          new Room()
          {
            RoomTypeId = 1,
            GenderId = 2,
            LeaseEnd = DateTime.Today.AddMonths(3),
            LeaseStart = DateTime.Now,
            Id = Guid.Parse("249e5358-169a-4bc6-aa0f-c054952456fd"),
            ComplexId = Guid.Parse("b5e050aa-6bfc-46ad-9a69-90b1f99ed606"),
            NumberOfBeds = 4,
            RoomNumber = "2428B",
            NumberOfOccupants = 0
          },
          new Room()
          {
            RoomTypeId = 1,
            GenderId = 2,
            LeaseEnd = DateTime.Today.AddMonths(3),
            LeaseStart = DateTime.Now,
            Id = Guid.Parse("249e5358-169a-4bc6-aa0f-c054952456ee"),
            ComplexId = Guid.Parse("b5e050aa-6bfc-46ad-9a69-90b1f99ed606"),
            NumberOfBeds = 4,
            RoomNumber = "2127E",
            NumberOfOccupants = 4
          },
          new Room()
          {
            RoomTypeId = 1,
            GenderId = 1,
            LeaseEnd = DateTime.Today.AddMonths(1),
            LeaseStart = DateTime.Now.AddDays(1),
            Id = Guid.Parse("fa1d6c6e-9650-44c9-8c6b-5aebd3f9a671"),
            ComplexId = Guid.Parse("b5e050aa-6bfc-46ad-9a69-90b1f99ed606"),
            NumberOfBeds = 2,
            RoomNumber = "2422",
            NumberOfOccupants = 1
          },
          new Room()
          {
            RoomTypeId = 1,
            GenderId = 2,
            LeaseEnd = DateTime.Today.AddMonths(4),
            LeaseStart = DateTime.Now.AddDays(1),
            Id = Guid.Parse("0a4d6c61-9650-44c9-8c6b-5aebd3f9a676"),
            ComplexId = Guid.Parse("b5e050aa-6bfc-46ad-9a69-90b1f99ed606"),
            NumberOfBeds = 3,
            RoomNumber = "2421",
            NumberOfOccupants = 1
          }
        );
      });

      //Room Type
      modelBuilder.Entity<RoomType>(entity =>
      {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Type)
          .IsRequired()
          .HasMaxLength(50);

        entity.HasData(
           new RoomType() { Id = 1, Type = "Apartment" },
           new RoomType() { Id = 2, Type = "Dormitory" },
           new RoomType() { Id = 3, Type = "TownHouse" },
           new RoomType() { Id = 4, Type = "Hotel/Motel" }
        );
      });

    }

  }

      
}
