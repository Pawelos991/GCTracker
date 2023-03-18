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

    public virtual DbSet<Crawler> Crawlers { get; set; }

    public virtual DbSet<SchemaVersion> SchemaVersions { get; set; }

    public virtual DbSet<Scraper> Scrapers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Crawler>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("crawler");

            entity.Property(e => e.CrawlerLink).HasMaxLength(1);
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.PageName).HasMaxLength(255);
        });

        modelBuilder.Entity<SchemaVersion>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Applied).HasColumnType("datetime");
            entity.Property(e => e.ScriptName).HasMaxLength(255);
        });

        modelBuilder.Entity<Scraper>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("scraper");

            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PageName).HasMaxLength(255);
            entity.Property(e => e.PhotoLink).HasMaxLength(1);
            entity.Property(e => e.Price).HasColumnType("smallmoney");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
