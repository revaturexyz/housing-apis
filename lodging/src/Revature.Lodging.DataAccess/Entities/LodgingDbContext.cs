using System;
using Microsoft.EntityFrameworkCore;

namespace Revature.Lodging.DataAccess.Entities
{
  public class LodgingDbContext : DbContext
  {
    // complex IDs
    private static readonly Guid s_cId1 = Guid.Parse("b5e050aa-6bfc-46ad-9a69-90b1f99ed606");
    private static readonly Guid s_cId2 = Guid.Parse("68b7eadd-30ce-49a7-9b8c-bae1d47f46a6");
    private static readonly Guid s_cId3 = Guid.Parse("78b7eadd-30ce-49a7-9b8c-bae1d47f46a6");
    private static readonly Guid s_cId4 = Guid.Parse("88b7eadd-30ce-49a7-9b8c-bae1d47f46a6");

    // amenity IDs
    private static readonly Guid s_amId1 = Guid.Parse("b8b7eadd-30ce-49a7-9b8c-bae1d47f46a6");
    private static readonly Guid s_amId2 = Guid.Parse("c8b7eadd-30ce-49a7-9b8c-bae1d47f46a6");
    private static readonly Guid s_amId3 = Guid.Parse("d8b7eadd-30ce-49a7-9b8c-bae1d47f46a6");
    private static readonly Guid s_amId4 = Guid.Parse("e8b7eadd-30ce-49a7-9b8c-bae1d47f46a6");
    private static readonly Guid s_amId5 = Guid.Parse("f8b7eadd-30ce-49a7-9b8c-bae1d47f46a6");

    public LodgingDbContext()
    {
    }

    public LodgingDbContext(DbContextOptions<LodgingDbContext> options)
      : base(options)
    {
    }

    public virtual DbSet<Complex> Complex { get; set; }

    public virtual DbSet<ComplexAmenity> ComplexAmenity { get; set; }

    public virtual DbSet<RoomAmenity> RoomAmenity { get; set; }

    public virtual DbSet<Amenity> Amenity { get; set; }

    public virtual DbSet<Gender> Gender { get; set; }

    public virtual DbSet<Room> Room { get; set; }

    public virtual DbSet<RoomType> RoomType { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Amenity
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

        entity.HasData(
          new Amenity { Id = s_amId1, AmenityType = "fridge", Description = "to keep food fresh" },
          new Amenity { Id = s_amId2, AmenityType = "microwave", Description = string.Empty },
          new Amenity { Id = s_amId3, AmenityType = "pool", Description = "swimming" },
          new Amenity { Id = s_amId4, AmenityType = "kitchen", Description = "cook" },
          new Amenity { Id = s_amId5, AmenityType = "gym", Description = "work out" });
      });

      // Amenity Complex
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

        entity.HasData(
          new ComplexAmenity { Id = Guid.Parse("58b7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = s_amId1, ComplexId = s_cId1 },
          new ComplexAmenity { Id = Guid.Parse("59b7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = s_amId2, ComplexId = s_cId1 },
          new ComplexAmenity { Id = Guid.Parse("5ab7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = s_amId2, ComplexId = s_cId2 },
          new ComplexAmenity { Id = Guid.Parse("5bb7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = s_amId4, ComplexId = s_cId2 });
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

        entity.HasData(
          new RoomAmenity { Id = Guid.Parse("5cb7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = s_amId1, RoomId = Guid.Parse("249e5358-169a-4bc6-aa0f-c054952456fd") },
          new RoomAmenity { Id = Guid.Parse("5db7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = s_amId4, RoomId = Guid.Parse("249e5358-169a-4bc6-aa0f-c054952456fd") },
          new RoomAmenity { Id = Guid.Parse("5eb7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = s_amId5, RoomId = Guid.Parse("249e5358-169a-4bc6-aa0f-c054952456ee") },
          new RoomAmenity { Id = Guid.Parse("5fb7eadd-30ce-49a7-9b8c-bae1d47f46a6"), AmenityId = s_amId2, RoomId = Guid.Parse("249e5358-169a-4bc6-aa0f-c054952456ee") });
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
            Id = s_cId1,
            AddressId = Guid.Parse("0a4d616e-9650-44c9-8c6b-5aebd3f9a67e"),
            ProviderId = Guid.Parse("dfc872fc-b708-4caf-b3f1-3c842c8d3078"),
            ComplexName = "Liv+",
            ContactNumber = "8177517911"
          },
          new Complex
          {
            Id = s_cId2,
            AddressId = Guid.Parse("280905b8-63ce-4372-b204-8cb764d6f271"),
            ProviderId = Guid.Parse("dfc872fc-b708-4caf-b3f1-3c842c8d3078"),
            ComplexName = "SampleComplex",
            ContactNumber = "4445550506"
          },
          new Complex
          {
            Id = s_cId3,
            AddressId = Guid.Parse("837c3248-1685-4d08-934a-0f17a6d1836a"),
            ProviderId = Guid.Parse("f7631719-b74b-46c4-bb89-6b3a3681ac06"),
            ComplexName = "Complex",
            ContactNumber = "7771112222"
          },
          new Complex
          {
            Id = s_cId4,
            AddressId = Guid.Parse("56b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
            ProviderId = Guid.Parse("f7631719-b74b-46c4-bb89-6b3a3681ac06"),
            ComplexName = "ComplexNearMe",
            ContactNumber = "3332221111"
          });
      });

      // Gender
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
         new Gender() { Id = 2, Type = "Female" });
      });

      // Room
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
            ComplexId = s_cId1,
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
            ComplexId = s_cId1,
            NumberOfBeds = 4,
            RoomNumber = "2127E",
            NumberOfOccupants = 1
          },
          new Room()
          {
            RoomTypeId = 1,
            GenderId = 1,
            LeaseEnd = DateTime.Today.AddMonths(1),
            LeaseStart = DateTime.Now.AddDays(1),
            Id = Guid.Parse("fa1d6c6e-9650-44c9-8c6b-5aebd3f9a671"),
            ComplexId = s_cId2,
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
            ComplexId = s_cId3,
            NumberOfBeds = 3,
            RoomNumber = "2421",
            NumberOfOccupants = 1
          });
      });

      // Room Type
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
           new RoomType() { Id = 4, Type = "Hotel/Motel" });
      });
    }
  }
}
