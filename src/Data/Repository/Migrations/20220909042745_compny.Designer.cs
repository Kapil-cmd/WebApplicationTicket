﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository;

#nullable disable

namespace Repository.Migrations
{
    [DbContext(typeof(TicketingContext))]
    [Migration("20220909042745_compny")]
    partial class compny
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Repository.Entites.Category", b =>
                {
                    b.Property<string>("CId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("CId");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("Repository.Entites.Permission", b =>
                {
                    b.Property<string>("PermissionId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("PId");

                    b.Property<string>("MenuName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PermissionId");

                    b.ToTable("Permissions", (string)null);
                });

            modelBuilder.Entity("Repository.Entites.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Repository.Entites.RolePermission", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PermissionId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermissions", (string)null);
                });

            modelBuilder.Entity("Repository.Entites.Ticket", b =>
                {
                    b.Property<string>("TicketId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AssignedTo")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TicketDetails")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("TicketId");

                    b.HasIndex("CategoryName");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Tickets", (string)null);
                });

            modelBuilder.Entity("Repository.Entites.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("ActivationCode")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit")
                        .HasColumnName("IsEmailVerified");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("OTP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PhoneNumber")
                        .HasMaxLength(13)
                        .HasColumnType("bigint");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Repository.Entites.UserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("Repository.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumberOfEmployee")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("Repository.Entities.UserTicket", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TicketId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "TicketId");

                    b.HasIndex("TicketId");

                    b.ToTable("UserTickets", (string)null);
                });

            modelBuilder.Entity("Repository.Entites.Category", b =>
                {
                    b.HasOne("Repository.Entites.User", "User")
                        .WithMany("Categories")
                        .HasForeignKey("CreatedBy")
                        .HasPrincipalKey("UserName")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Repository.Entites.RolePermission", b =>
                {
                    b.HasOne("Repository.Entites.Permission", "aPermission")
                        .WithMany("Roles")
                        .HasForeignKey("PermissionId")
                        .IsRequired();

                    b.HasOne("Repository.Entites.Role", "aRole")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId")
                        .IsRequired();

                    b.Navigation("aPermission");

                    b.Navigation("aRole");
                });

            modelBuilder.Entity("Repository.Entites.Ticket", b =>
                {
                    b.HasOne("Repository.Entites.Category", "Category")
                        .WithMany("Tickets")
                        .HasForeignKey("CategoryName")
                        .HasPrincipalKey("CategoryName")
                        .IsRequired();

                    b.HasOne("Repository.Entites.User", "User")
                        .WithMany("MyCreatedTicket")
                        .HasForeignKey("CreatedBy")
                        .HasPrincipalKey("UserName")
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Repository.Entites.UserRole", b =>
                {
                    b.HasOne("Repository.Entites.Role", "aRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Repository.Entites.User", "aUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("aRole");

                    b.Navigation("aUser");
                });

            modelBuilder.Entity("Repository.Entities.UserTicket", b =>
                {
                    b.HasOne("Repository.Entites.Ticket", "aTicket")
                        .WithMany("AssignedUsers")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("Repository.Entites.User", "aUser")
                        .WithMany("AssignedeTickets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("aTicket");

                    b.Navigation("aUser");
                });

            modelBuilder.Entity("Repository.Entites.Category", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Repository.Entites.Permission", b =>
                {
                    b.Navigation("Roles");
                });

            modelBuilder.Entity("Repository.Entites.Role", b =>
                {
                    b.Navigation("Permissions");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Repository.Entites.Ticket", b =>
                {
                    b.Navigation("AssignedUsers");
                });

            modelBuilder.Entity("Repository.Entites.User", b =>
                {
                    b.Navigation("AssignedeTickets");

                    b.Navigation("Categories");

                    b.Navigation("MyCreatedTicket");

                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
