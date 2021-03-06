﻿// <auto-generated />
using System;
using Essenbee.Bot.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Essenbee.Bot.Web.Migrations
{
    [DbContext(typeof(AppDataContext))]
    [Migration("20180711083812_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Essenbee.Bot.Core.Data.StartupMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Message");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("StartupMessages");
                });

            modelBuilder.Entity("Essenbee.Bot.Core.Data.TimedMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Delay");

                    b.Property<string>("Message");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("TimedMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
