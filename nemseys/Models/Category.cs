namespace Nemesis.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Report> Reports { get; set; }
    }
}
