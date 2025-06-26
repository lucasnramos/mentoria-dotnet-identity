namespace Authentication.Adapter.Models
{
    public class UserLogged
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
    }

}
