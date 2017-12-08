﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using FrontendServices;

namespace FrontendServices.Migrations
{
    [DbContext(typeof(LocalDB))]
    partial class LocalDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("FrontendServices.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EventInfo");

                    b.Property<string>("EventType");

                    b.Property<DateTime>("OccuredOn");

                    b.Property<bool>("WasRead");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });
        }
    }
}
