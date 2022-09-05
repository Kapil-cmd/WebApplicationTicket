using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entities;

namespace Repository.Configurations
{
    public class CategoryTicketConfig : IEntityTypeConfiguration<CategoryTicket>
    {
        public void Configure(EntityTypeBuilder<CategoryTicket> builder)
        {
            builder.ToTable("CategoryTicket");

            builder.HasKey(x => new { x.CategoryId, x.TicketId });

            builder.HasOne(x => x.aTicket)
                .WithMany(x => x.tCategory)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(x => x.aCategory)
                .WithMany(x => x.cTicket)
                .HasForeignKey(x => x.TicketId)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
