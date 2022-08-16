namespace Repository.Entites
{
    public class Role
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        public virtual IEnumerable<UserRole> Users { get; set; }
        public virtual IEnumerable<RolePermission> Permissions { get; set; }
    }
}
