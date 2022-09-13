using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entities;

namespace Repository.Configurations
{
    public class FieldValidationConfig : IEntityTypeConfiguration<FieldValidation>
    {
        public void Configure(EntityTypeBuilder<FieldValidation> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(x => x.Length).HasMaxLength(1000);
            builder.Property(x => x.Name).IsRequired();
        }
    }
}
