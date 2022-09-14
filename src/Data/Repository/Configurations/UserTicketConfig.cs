using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entities;

namespace Repository.Configurations
{
    public class UserTicketConfig : IEntityTypeConfiguration<UserTicket>
    {
        public void Configure(EntityTypeBuilder<UserTicket> builder)
        {
            builder.HasKey(x => new { x.UserName, x.TicketId });

            builder.HasOne(x => x.aUser)
                .WithMany(x => x.Tickets)
                .HasForeignKey(x => x.UserName)
                .HasPrincipalKey(x => x.UserName)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(x => x.aTicket)
                .WithMany(x => x.AssignedUsers)
                .HasForeignKey(x => x.TicketId)
                .HasPrincipalKey(x => x.TicketId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
