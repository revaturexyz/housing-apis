using System;
using Microsoft.EntityFrameworkCore;

namespace Revature.Address.DataAccess.Entities
{
  /// <summary>
  /// This class specifies the structure of the
  /// database during the EF build steps.
  /// </summary>
  public partial class AddressDbContext : DbContext
  {
    public AddressDbContext()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddressDbContext"/> class.
    /// Sets the options for the database context.
    /// </summary>
    public AddressDbContext(DbContextOptions<AddressDbContext> options)
      : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    /// <summary>
    /// Creates the model for the Address table in the database.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Address>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Street).IsRequired();
        entity.Property(e => e.City).IsRequired();
        entity.Property(e => e.State).IsRequired();
        entity.Property(e => e.Country).IsRequired();
        entity.Property(e => e.ZipCode).IsRequired().HasMaxLength(5);
        entity.HasData(
          new Address
          {
            Id = Guid.Parse("50b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
            Street = "2905 Brattleboro Ave",
            City = "Des Moines",
            State = "IA",
            Country = "USA",
            ZipCode = "50311"
          }, // Used by 2nd tenant
          new Address
          {
            Id = Guid.Parse("52b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
            Street = "11730 Plaza America Dr.",
            City = "Reston",
            State = "VA",
            Country = "USA",
            ZipCode = "20190"
          }, // Used by 3rd tenant
          new Address
          {
            Id = Guid.Parse("1a4d6c6e-9640-44c9-8c6b-5aebd3f9a67e"),
            Street = "920 S Mesquite St",
            City = "Arlington",
            State = "TX",
            Country = "USA",
            ZipCode = "76010"
          }, // Used by first tenant
          new Address
          {
            Id = Guid.Parse("0a4d616e-9650-44c9-8c6b-5aebd3f9a67e"),
            Street = "2020 E Randol Mill Rd",
            City = "Arlington",
            State = "TX",
            Country = "USA",
            ZipCode = "76011"
          }, // Used by first complex
          new Address
          {
            Id = Guid.Parse("280905b8-63ce-4372-b204-8cb764d6f271"),
            Street = "1311 Murdock Rd",
            City = "Dallas",
            State = "TX",
            Country = "USA",
            ZipCode = "75217"
          }, // Used by second complex
          new Address
          {
            Id = Guid.Parse("837c3248-1685-4d08-934a-0f17a6d1836a"),
            Street = "430 18th Ave E",
            City = "Seattle",
            State = "WA",
            Country = "USA",
            ZipCode = "98112"
          }, // Used by third complex
          new Address
          {
            Id = Guid.Parse("56b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
            Street = "424 Riverside Ave, South",
            City = "Sartell",
            State = "MN",
            Country = "USA",
            ZipCode = "56377"
          }); // Used by 4th complex
      });
    }
  }
}
