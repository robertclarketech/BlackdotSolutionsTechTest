﻿// <auto-generated />
using System;
using BlackdotTechTest.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlackdotTechTest.WebApi.Migrations
{
    [DbContext(typeof(BlackdotTechTestContext))]
    [Migration("20200315140436_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2");

            modelBuilder.Entity("BlackdotTechTest.Domain.Entities.SearchEngineQuery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateEdited")
                        .HasColumnType("TEXT");

                    b.Property<string>("Query")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SearchEngineQueries");
                });

            modelBuilder.Entity("BlackdotTechTest.Domain.Entities.SearchEngineQueryResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateEdited")
                        .HasColumnType("TEXT");

                    b.Property<string>("ResultLink")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ResultText")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SearchEngineQueryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SearchEngineType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SearchEngineQueryId");

                    b.ToTable("SearchEngineQueryResults");
                });

            modelBuilder.Entity("BlackdotTechTest.Domain.Entities.SearchEngineQueryResult", b =>
                {
                    b.HasOne("BlackdotTechTest.Domain.Entities.SearchEngineQuery", "SearchEngineQuery")
                        .WithMany("QueryResults")
                        .HasForeignKey("SearchEngineQueryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
