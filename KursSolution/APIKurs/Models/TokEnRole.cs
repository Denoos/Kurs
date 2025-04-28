namespace APIKurs.Models
{
    public class TokEnRole
    {
        public int Id { get; set; }

        public Role Title { get; set; } = null!;

        public string Token { get; set; }
    }
}
