using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configurations
{
    public class CategoryConfig: IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(x => x.CId);
            builder.Property(x => x.CId).HasMaxLength(50);
            builder.Property(x => x.CategoryName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.CreatedBy).HasMaxLength(25).IsRequired();
            builder.Property(x => x.CreatedDateTime);
            builder.Property(x => x.ModifiedBy).HasColumnName("Modified By");
            builder.Property(x => x.ModifiedDateTime);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Categories)
                   .HasForeignKey(x => x.CreatedBy)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
