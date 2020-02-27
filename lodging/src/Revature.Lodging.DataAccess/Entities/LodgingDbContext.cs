using Microsoft.EntityFrameworkCore;
using System;

namespace Revature.Complex.DataAccess.Entities
{
    public class LodgingDbContext : DbContext
    {
        public LodgingDbContext() {}

        public LodgingDbContext(DbContextOptions<LodgingDbContext> options) : base(options) {}

        public virtual DbSet<Complex> Complex { get; set; }

        public virtual DbSet<Amenity> Amenity { get; set; }

        public virtual DbSet<AmenityComplex> AmenityComplex { get; set; }

        public virtual DbSet<AmenityFloorPlan> AmenityFloorPlan { get; set; }

        public virtual DbSet<AmenityRoom> AmenityRoom { get; set; }

        public virtual DbSet<Room> Room { get; set; }

        public virtual DbSet<FloorPlan> FloorPlan { get; set; }

        public virtual DbSet<Gender> Gender { get; set; }

        public virtual DbSet<RoomType> RoomType { get; set; }
    }
}