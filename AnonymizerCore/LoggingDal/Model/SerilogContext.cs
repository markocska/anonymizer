using System;
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

                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });
        }
    }
}
