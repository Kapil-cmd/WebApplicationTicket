using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entities;

namespace Repository.Configurations
{
    public class PermissionConfig : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");
            builder.HasKey(x => x.PermissionId);
            builder.Property(x => x.PermissionId).HasMaxLength(200).HasColumnName(@"PId");
            builder.Property(x => x.Name).HasMaxLength(200).HasColumnName(@"PermissionName").IsRequired();
        }
    }
}
