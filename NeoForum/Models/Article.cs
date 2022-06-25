using NeoForum.Areas.Identity.Data;
using NeoForum.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace NeoForum.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Заголовок")]
        public string? Title { get; set; }

        [Display(Name = "Категория")]
        public Categories Categories { get; set; }

        [Required]
        [Display(Name = "Текст")]
        public string? Text { get; set; }

        [Required]
        [Display(Name = "Автор")]
        public string? Author { get; set; }

        public DateTime When { get; set; }
    }
}
