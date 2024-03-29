﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LoggingDal.Model
{
    public partial class SerilogContext : DbContext
    {
        public SerilogContext()
        {
        }

        public SerilogContext(DbContextOptions<SerilogContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Logs> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-UBKC2MM;Database=Serilog;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.GroupKey).HasMaxLength(500);

                entity.Property(e => e.JobDescription).HasMaxLength(500);

                entity.Property(e => e.JobKey).HasMaxLength(500);

                entity.Property(e => e.Timestamp).HasColumnType("datetime")
                    .HasConversion(v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

                //entity.HasIndex(e => new { e.GroupKey, e.JobKey });

                //entity.HasIndex(e => new { e.GroupKey, e.JobKey, e.Severity });

                entity.HasIndex(e => new { e.GroupKey, e.JobKey, e.Severity, e.Timestamp });

                entity.HasIndex(e => new { e.GroupKey, e.JobKey, e.Timestamp});

                entity.HasIndex(e => new { e.JobDescription, e.Severity, e.Timestamp });

                entity.HasIndex(e => new { e.JobDescription, e.Timestamp });

                entity.HasIndex(e => new { e.JobDescription, e.GroupKey, e.Severity, e.Timestamp });

                entity.HasIndex(e => new { e.JobDescription, e.GroupKey, e.Timestamp });

                entity.HasIndex(e => new { e.Timestamp, e.Severity });

                entity.HasIndex(e => new { e.Severity, e.Timestamp });



            });
        }
    }
}
