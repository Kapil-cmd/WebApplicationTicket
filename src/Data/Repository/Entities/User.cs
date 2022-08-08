using Repository.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entites
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        //public bool IsEmailConfirmed { get; set; }
        public int Age { get; set; }
        public long PhoneNumber { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }



        public virtual IEnumerable<Category> Categories {get; set;}
        public virtual IEnumerable<Ticket> MyCreatedTicket { get; set; }
        public virtual IEnumerable<UserRole> Roles { get; set;}
        public virtual IEnumerable<UserTicket> AssignedeTickets {get; set; }
    }
}
