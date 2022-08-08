﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entites
{
    public class Role
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public bool IsSelected { get; set;}
        public virtual IEnumerable<UserRole> Users { get; set; }
        public virtual IEnumerable<RolePermission> Permissions { get; set; }
    }
}
