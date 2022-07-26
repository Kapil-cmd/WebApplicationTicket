using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entites;

namespace Repository.Configurations
{
    public class RolePermissionConfig : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("RolePermissions");

            builder.HasKey(x => new { x.RoleId, x.PermissionId});

            builder.HasOne(x => x.aRole)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.HasOne(x => x.aPermission)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
