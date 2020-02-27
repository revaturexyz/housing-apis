using Microsoft.EntityFrameworkCore;
using System;

namespace Revature.Lodging.DataAccess.Entities
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
                entity.HasKey(e => e.AmenityId);

                entity.Property(e => e.AmenityType)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .IsRequired();
            });

            modelBuilder.Entity<AmenityComplex>(entity =>
            {
                entity.HasKey(e => e.AmenityComplexId);

                entity.HasOne(e => e.Amenity)
                    .WithMany(d => d.AmenityComplex)
                    .HasForeignKey(p => p.AmenityId)
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
                entity.HasKey(e => e.AmenityRoomId);
                
                entity.HasOne(e => e.Amenity)
                    .WithMany(d => d.AmenityRoom)
                    .HasForeignKey(p => p.AmenityId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.Room)
                    .WithMany(d => d.AmenityRoom)
                    .HasForeignKey(p => p.RoomId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Complex>(entity =>
            {
                entity.HasKey(e => e.ComplexId);

                entity.Property(e => e.AddressId)
                    .IsRequired();
                
                entity.Property(e => e.ProviderId)
                    .IsRequired();
                
                entity.Property(e => e.ComplexName)
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
                entity.HasKey(e => e.RoomId);

                entity.Property(e => e.RoomNumber)
                    .IsRequired();

                entity.Property(e => e.NumberOfBeds)
                    .IsRequired();

                entity.Property(e => e.NumberOfOccupants)
                    .IsRequired();

                entity.Property(e => e.LeaseStart);

                entity.Property(e => e.LeaseEnd);

                entity.HasOne(e => e.Gender)
                  .WithMany(d => d.Room)
                  .HasForeignKey(p => p.GenderId)
                  .IsRequired();

                entity.HasOne(e => e.RoomType)
                .WithMany(d => d.Room)
                .HasForeignKey(p => p.RoomTypeId)
                .IsRequired();

              entity.HasOne(e => e.Complex)
                .WithMany(d => d.Room)
              .HasForeignKey(p => p.ComplexId)
              .IsRequired();

                entity.HasOne(e => e.FloorPlan)
              .WithMany(d => d.Room)
              .HasForeignKey(e => e.FloorPlanID)
              .IsRequired();
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.HasKey(e => e.RoomTypeId);

                entity.Property(e => e.Type)
                    .IsRequired();
            });


        }
    }


}
