using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entities;

namespace Repository.Configurations
{
    public class CategoryTempConfig : IEntityTypeConfiguration<CategoryTemp>
    {
        public void Configure(EntityTypeBuilder<CategoryTemp> builder)
        {
            builder.ToTable("CategoryTemp");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(x => x.CategoryName).IsRequired();
        }
    }
}
