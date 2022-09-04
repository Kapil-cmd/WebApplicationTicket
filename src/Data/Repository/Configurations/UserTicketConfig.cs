using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entities;

namespace Repository.Configurations
{
    public class UserTicketConfig:IEntityTypeConfiguration<UserTicket>
    {
        public void Configure(EntityTypeBuilder<UserTicket> builder)
        {
            builder.ToTable("UserTickets");
            builder.HasKey(x => new { x.UserId, x.TicketId });

            builder.HasOne(x => x.aUser)
                .WithMany(x => x.AssignedeTickets)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(x => x.aTicket)
                .WithMany(x => x.AssignedUsers)
                .HasForeignKey(x => x.TicketId)
                .OnDelete(DeleteBehavior.ClientCascade);
                }
        }
}
