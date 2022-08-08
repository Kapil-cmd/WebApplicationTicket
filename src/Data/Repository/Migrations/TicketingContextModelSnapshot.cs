﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository;

#nullable disable

namespace Repository.Migrations
{
    [DbContext(typeof(TicketingContext))]
    partial class TicketingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Common.ViewModels.Tickets.ListCategory", b =>
                {
                    b.Property<string>("CId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryCId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CId");

                    b.HasIndex("CategoryCId");

                    b.ToTable("ListCategory");
                });

            modelBuilder.Entity("Repository.Entites.Category", b =>
                {
                    b.Property<string>("CId")
                        .ValueGeneratedOnAdd()
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

                    b.Property<string>("TicketId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("TicketId");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("Repository.Entites.Permission", b =>
                {
                    b.Property<string>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("PId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PermissionName");

                    b.Property<string>("RolePermissionPermissionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RolePermissionRoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PermissionId");

                    b.HasIndex("RolePermissionRoleId", "RolePermissionPermissionId");

                    b.ToTable("Permissions", (string)null);
                });

            modelBuilder.Entity("Repository.Entites.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsSelected")
                        .HasColumnType("bit");

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
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AssignedTo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)");

                    b.Property<DateTime?>("CreatedDateTime")
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

                    b.Property<string>("UserTicketTicketId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserTicketUserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TicketId");

                    b.HasIndex("CategoryName");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("UserTicketUserId", "UserTicketTicketId");

                    b.ToTable("Tickets", (string)null);
                });

            modelBuilder.Entity("Repository.Entites.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("CategoryCId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<long>("PhoneNumber")
                        .HasMaxLength(13)
                        .HasColumnType("bigint");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("UserTicketTicketId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserTicketUserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryCId");

                    b.HasIndex("UserTicketUserId", "UserTicketTicketId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Repository.Entites.UserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsSelected")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
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

            modelBuilder.Entity("Common.ViewModels.Tickets.ListCategory", b =>
                {
                    b.HasOne("Repository.Entites.Category", null)
                        .WithMany("listCategories")
                        .HasForeignKey("CategoryCId");
                });

            modelBuilder.Entity("Repository.Entites.Category", b =>
                {
                    b.HasOne("Repository.Entites.User", "User")
                        .WithMany("Categories")
                        .HasForeignKey("CreatedBy")
                        .HasPrincipalKey("UserName")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Repository.Entites.Ticket", null)
                        .WithMany("Categories")
                        .HasForeignKey("TicketId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Repository.Entites.Permission", b =>
                {
                    b.HasOne("Repository.Entites.RolePermission", null)
                        .WithMany("Permissions")
                        .HasForeignKey("RolePermissionRoleId", "RolePermissionPermissionId");
                });

            modelBuilder.Entity("Repository.Entites.RolePermission", b =>
                {
                    b.HasOne("Repository.Entites.Permission", "aPermission")
                        .WithMany("Roles")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Repository.Entites.Role", "aRole")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
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
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Repository.Entites.User", "User")
                        .WithMany("Ticket")
                        .HasForeignKey("CreatedBy")
                        .HasPrincipalKey("UserName")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Repository.Entities.UserTicket", null)
                        .WithMany("Tickets")
                        .HasForeignKey("UserTicketUserId", "UserTicketTicketId");

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Repository.Entites.User", b =>
                {
                    b.HasOne("Repository.Entites.Category", null)
                        .WithMany("Users")
                        .HasForeignKey("CategoryCId");

                    b.HasOne("Repository.Entities.UserTicket", null)
                        .WithMany("Users")
                        .HasForeignKey("UserTicketUserId", "UserTicketTicketId");
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
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("aRole");

                    b.Navigation("aUser");
                });

            modelBuilder.Entity("Repository.Entities.UserTicket", b =>
                {
                    b.HasOne("Repository.Entites.Ticket", "aTicket")
                        .WithMany("Users")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Repository.Entites.User", "aUser")
                        .WithMany("Tickets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("aTicket");

                    b.Navigation("aUser");
                });

            modelBuilder.Entity("Repository.Entites.Category", b =>
                {
                    b.Navigation("Tickets");

                    b.Navigation("Users");

                    b.Navigation("listCategories");
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

            modelBuilder.Entity("Repository.Entites.RolePermission", b =>
                {
                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("Repository.Entites.Ticket", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Repository.Entites.User", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Roles");

                    b.Navigation("Ticket");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Repository.Entities.UserTicket", b =>
                {
                    b.Navigation("Tickets");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
