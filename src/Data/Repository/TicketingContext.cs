﻿using Microsoft.EntityFrameworkCore;
using Repository.Configurations;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class TicketingContext : DbContext
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public TicketingContext(DbContextOptions<TicketingContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CategoryConfig());
            builder.ApplyConfiguration(new PermissionConfig());
            builder.ApplyConfiguration(new RoleConfig());
            builder.ApplyConfiguration(new RolePermissionConfig());
            builder.ApplyConfiguration(new TicketConfig());
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new UserRoleConfig());

        }
    }
}
