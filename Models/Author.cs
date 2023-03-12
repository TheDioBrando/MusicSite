using System.ComponentModel.DataAnnotations;

namespace RolesSiteMVC.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Display(Name = "Информация о группе")]
        public string? Information { get; set; }

        public ICollection<Music> Music { get; set; }
    }
}
