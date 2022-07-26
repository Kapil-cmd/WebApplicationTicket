using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entites;

namespace Repository.Configurations
{
    public class TicketConfig : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");

            builder.HasKey(x => x.TicketId);
            builder.Property(x => x.TicketId).IsRequired();
            builder.Property(x => x.TicketDetails).HasMaxLength(200).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.CreatedDateTime);
            builder.Property(x => x.ModifiedDateTime);
            builder.Property(x => x.ModifiedBy);
            builder.Property(x => x.CategoryId);
            builder.Property(x => x.CategoryName).IsRequired();
            builder.Property(x => x.AssignedTo).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.ImageName);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Ticket)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Tickets)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
