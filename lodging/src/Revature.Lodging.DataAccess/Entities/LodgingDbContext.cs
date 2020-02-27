using Microsoft.EntityFrameworkCore;
using System;

namespace Revature.Complex.DataAccess.Entities
{
    public class LodgingDbContext : DbContext
    {
        public LodgingDbContext() {}

        public LodgingDbContext(DbContextOptions<LodgingDbContext> options) : base(options) {}

        public virtual DbSet<Amenity> Amenity { get; set; }

        public virtual DbSet<AmenityComplex> AmenityComplex { get; set; }

        public virtual DbSet<AmenityFloorPlan> AmenityFloorPlan { get; set; }

        public virtual DbSet<AmenityRoom> AmenityRoom { get; set; }

        public virtual DbSet<Complex> Complex { get; set; }

        public virtual DbSet<FloorPlan> FloorPlan { get; set; }

        public virtual DbSet<Gender> Gender { get; set; }

        public virtual DbSet<Room> Room { get; set; }

        public virtual DbSet<RoomType> RoomType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Amenity>(entity =>
            {
                entity.HasKey(e => e.AmenityID);

                entity.Property(e => e.AmenityType)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .IsRequired();
            });

            modelBuilder.Entity<AmenityComplex>(entity =>
            {
                entity.HasKey(e => e.AmenityComplexID);

                entity.HasOne(e => e.Amenity)
                    .WithMany(d => d.AmenityComplex)
                    .HasForeignKey(p => p.AmenityID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.Complex)
                    .WithMany(d => d.AmenityComplex)
                    .HasForeignKey(p => p.ComplexId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AmenityFloorPlan>(entity =>
            {
                entity.HasKey(e => e.AmenityFloorPlanID);

                entity.HasOne(e => e.Amenity)
                    .WithMany(d => d.AmenityFloorPlan)
                    .HasForeignKey(p => p.AmenityID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.FloorPlan)
                    .WithMany(d => d.AmenityFloorPlan)
                    .HasForeignKey(p => p.FloorPlanID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AmenityRoom>(entity =>
            {
                entity.HasKey(e => AmenityRoomID);
                
                entity.HasOne(e => e.Amenity)
                    .WithMany(d => d.AmenityRoom)
                    .HasForeignKey(p => p.AmenityID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.Room)
                    .WithMany(d => d.AmenityRoom)
                    .HasForeignKey(p => p.RoomID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Complex>(entity =>
            {
                entity.HasKey(e => e.ComplexId);

                entity.Property(e => e.AddressID)
                    .IsRequired();
                
                entity.Property(e => e.ProviderID)
                    .IsRequired();
                
                entity.Property(e => e.ComplexName)
                    .IsRequired();

                entity.Property(e => ContactNumber)
                    .IsRequired();
            });

            modelBuilder.Entity<FloorPlan>(entity => 
            {
                entity.HasKey(e => e.FloorPlanID);

                entity.Property(e => e.FloorPlanName)
                    .IsRequired();

                entity.Property(e => e.NumberBeds)
                    .IsRequired();

                entity.HasOne(e => e.RoomType)
                    .WithMany(d => d.FloorPlan)
                    .HasForeignKey(p => p.RoomTypeID)
                    .IsRequired();

                entity.HasOne(e => e.Complex)
                    .WithMany(d => d.FloorPlan)
                    .HasForeignKey(p => p.ComplexID)
                    .IsRequired();
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasKey(e => e.GenderID);

                entity.Property(e => e.Type)
                    .IsRequired();
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.RoomID);

                entity.Property(e => e.RoomNumber)
                    .IsRequired();

                entity.Property(e => e.NumberOfBeds)
                    .IsRequired();

                entity.Property(e => e.NumberOfOccupants)
                    .IsRequired();

                entity.Property(e => e.LeaseStart);

                entity.Property(e => e.LeaseEnd);

                entity.HasForeignKey(e => e.GenderID);

                entity.HasForeignKey(e => e.RoomTypeID);

                entity.HasForeignKey(e => e.ComplexID);

                entity.HasForeignKey(e => e.FloorPlanID);
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.HasKey(e => e.RoomTypeID);

                entity.Property(e => e.Type)
                    .IsRequired();
            });


        }
    }


}