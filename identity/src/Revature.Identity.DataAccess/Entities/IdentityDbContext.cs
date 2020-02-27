using Microsoft.EntityFrameworkCore;
using Revature.Identity.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Revature.Identity.DataAccess
{
  public class IdentityDbContext : DbContext
  {
    /// <summary>

    /// Context class that acts to define the structure of a code-first database.

    /// </summary>

    ///



    // Defualt constructor

    public IdentityDbContext()

    { }



    // Constructor with options and iheritance from its parent class.

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)

    {

    }



    // All tables found in the database defined here

    public virtual DbSet<Notification> Notification { get; set; }

    public virtual DbSet<UpdateAction> UpdateAction { get; set; }

    public virtual DbSet<ProviderAccount> ProviderAccount { get; set; }

    public virtual DbSet<CoordinatorAccount> CoordinatorAccount { get; set; }

    public virtual DbSet<TenantAccount> TenantAccount { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

    {

      if (!optionsBuilder.IsConfigured)

      {
        {
          if (!optionsBuilder.IsConfigured)
          {
            optionsBuilder.UseNpgsql("User ID =postgres;Password=rev;Server=localhost;Port=5432;Database=IdentityDb;Integrated Security=true;Pooling=true;");
          }

        }
      }
    }


  }
}
