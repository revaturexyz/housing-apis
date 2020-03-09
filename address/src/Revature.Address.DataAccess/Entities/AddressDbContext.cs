using Microsoft.EntityFrameworkCore;
using System;

namespace Revature.Address.DataAccess.Entities
{
  /// <summary>
  /// This class specifies the structure of the
  /// database during the EF build steps
  /// </summary>
  public partial class AddressDbContext : DbContext
  {
    public AddressDbContext() { }

    /// <summary>
    /// Sets the options for the database context
    /// </summary>
    /// <param name="options"></param>
    public AddressDbContext(DbContextOptions<AddressDbContext> options) : base(options) { }
    public virtual DbSet<Address> Addresses { get; set; }

    /// <summary>
    /// Creates the model for the Address table in the database
    /// </summary>
    /// <param name="modelBuilder"></param>
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
        entity.HasData(new Address()
        {
          Id = Guid.Parse("50b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
          Street = "404 Texas Street",
          City = "Texas City",
          State = "Texas",
          Country = "USA",
          ZipCode = "12345"
        },
        new Address()
        {
          Id = Guid.Parse("52b7eadd-30ce-49a7-9b8c-bae1d47f46a6"),
          Street = "405 Raccoon Street",
          City = "Raccoon City",
          State = "Raccoon",
          Country = "USA",
          ZipCode = "54321"
        });
      });
    }
  }
}
