namespace DataLayer
{
    public class Role
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int AccountId { get; set; }
        public Account Account { get; set; } 
    }
}