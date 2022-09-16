using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entities;

namespace Repository.Configurations
{
    public class CategoryTicketConfig : IEntityTypeConfiguration<CategoryTicket>
    {
        public void Configure(EntityTypeBuilder<CategoryTicket> builder)
        {
            builder.HasKey(x => new { x.TicketId, x.CategoryName });

            builder.HasOne(x => x.aTicket)
                .WithMany(x => x.CategoryTickets)
                .HasForeignKey(x => x.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.aCategory)
                .WithMany(x => x.CategoryTickets)
                .HasForeignKey(x => x.CategoryName)
                .HasPrincipalKey(x => x.CategoryName)
                .OnDelete(DeleteBehavior.ClientCascade);

        }
    }
}
