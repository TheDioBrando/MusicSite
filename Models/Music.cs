using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RolesSiteMVC.Models
{
    public class Music
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100), Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name ="Текст песни")]
        public string? SongsText { get; set; }

        [Required, Display(Name = "Ссылка")]
        public string Source { get; set; }


        public int AuthorID { get; set; }
        public Author Author { get; set; }
    }
}
