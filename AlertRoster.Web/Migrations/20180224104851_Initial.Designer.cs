﻿// <auto-generated />
using AlertRoster.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace AlertRoster.Web.Migrations
{
    [DbContext(typeof(Models.Database))]
    [Migration("20180224104851_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("AlertRoster.Web.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(25);

                    b.HasKey("Id");

                    b.HasIndex("DisplayName")
                        .IsUnique();

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("AlertRoster.Web.Models.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("Members");
                });

            modelBuilder.Entity("AlertRoster.Web.Models.MemberGroup", b =>
                {
                    b.Property<int>("MemberId");

                    b.Property<int>("GroupId");

                    b.HasKey("MemberId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("MemberGroup");
                });

            modelBuilder.Entity("AlertRoster.Web.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<int>("GroupId");

                    b.Property<int>("SenderId");

                    b.Property<byte>("Status");

                    b.Property<DateTimeOffset>("Timestamp");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("AlertRoster.Web.Models.MemberGroup", b =>
                {
                    b.HasOne("AlertRoster.Web.Models.Group", "Group")
                        .WithMany("Members")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AlertRoster.Web.Models.Member", "Member")
                        .WithMany("Groups")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AlertRoster.Web.Models.Message", b =>
                {
                    b.HasOne("AlertRoster.Web.Models.Group", "Group")
                        .WithMany("Messages")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AlertRoster.Web.Models.Member", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
