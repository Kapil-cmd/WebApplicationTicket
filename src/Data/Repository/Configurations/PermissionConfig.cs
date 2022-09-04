using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entites;

namespace Repository.Configurations
{
    public class PermissionConfig : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");
            builder.HasKey(x => x.PermissionId);
            builder.Property(x => x.PermissionId).HasColumnName(@"PId");
            builder.Property(x => x.Name).HasColumnName(@"PermissionName");
            builder.Property(x => x.ParentPermissionId);
            builder.Property(x => x.Group);
        }
    }
}
