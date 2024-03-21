using System.ComponentModel.DataAnnotations;

namespace CaravanApi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public string? FilePath { get; set; }
    }
}
