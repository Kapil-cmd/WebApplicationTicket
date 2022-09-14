using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entites;

namespace Repository.Configurations
{
    public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder.HasKey(x => new { x.UserName, x.RoleName });

            builder.HasOne(x => x.aUser)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.UserName)
                .HasPrincipalKey(x => x.UserName)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(x => x.aRole)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleName)
                .HasPrincipalKey(x => x.Name)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
