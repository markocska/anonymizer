﻿// <auto-generated />
using System;
using LoggingDal.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LoggingDal.Migrations
{
    [DbContext(typeof(SerilogContext))]
    partial class SerilogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LoggingDal.Model.Logs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ex");

                    b.Property<string>("GroupKey")
                        .HasMaxLength(500);

                    b.Property<string>("JobDescription")
                        .HasMaxLength(500);

                    b.Property<string>("JobKey")
                        .HasMaxLength(500);

                    b.Property<string>("LogEvent");

                    b.Property<string>("Msg");

                    b.Property<string>("Severity");

                    b.Property<string>("Template");

                    b.Property<DateTime?>("Timestamp")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("Severity");

                    b.HasIndex("Timestamp");

                    b.HasIndex("GroupKey", "JobKey");

                    b.HasIndex("GroupKey", "JobKey", "Severity");

                    b.HasIndex("GroupKey", "JobKey", "Timestamp");

                    b.HasIndex("GroupKey", "JobKey", "Severity", "Timestamp");

                    b.ToTable("Logs");
                });
#pragma warning restore 612, 618
        }
    }
}
