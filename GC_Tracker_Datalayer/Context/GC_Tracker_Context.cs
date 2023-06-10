using System;
using System.Collections.Generic;
using GC_Tracker_Datalayer.Model;
using Microsoft.EntityFrameworkCore;

namespace GC_Tracker_Datalayer.Context;

public partial class GC_Tracker_Context : DbContext
{
    public GC_Tracker_Context()
    {
    }

    public GC_Tracker_Context(DbContextOptions<GC_Tracker_Context> options)
        : base(options)
    {
    }

    //public virtual DbSet<Crawler> Crawlers { get; set; }

    //public virtual DbSet<SchemaVersion> SchemaVersions { get; set; }

    public virtual DbSet<Images> Images { get; set; }
    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("product");
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<Images>(entity =>
        {
            entity.ToTable("images");
            entity.HasKey(x => x.Id);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
