using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entites;

namespace Repository.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.UserName).HasMaxLength(25).IsRequired();
            builder.Property(x => x.FirstName).HasMaxLength(25).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(25).IsRequired();
            builder.Property(x => x.Address).HasMaxLength(25).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Age).IsRequired();
            builder.Property(x => x.DateOfBirth).IsRequired();
            builder.Property(x => x.PhoneNumber).HasMaxLength(13).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(25).IsRequired();
        }
    }
}
