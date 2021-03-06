namespace Repository.Entites
{
    public class Category
    {
        public int CId { get; set; }
        public string CategoryName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public virtual User User{get; set;}
        public List<Ticket> Tickets { get; set; }
        public List<User> Users { get; set; }
    }
}
