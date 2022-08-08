using Common.Enums;
using Microsoft.AspNetCore.Http;
using Repository.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entites
{
    public class Ticket
    {
        public string TicketId { get; set; } = Guid.NewGuid().ToString();
        public string TicketDetails { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public string AssignedTo { get; set; }
        public string CategoryName { get; set; }
        public StatusEnum Status { get; set; }
        public string ImageName { get; set; }


        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
        public virtual IEnumerable<UserTicket> AssignedUsers { get; set; }
        
    }
}

