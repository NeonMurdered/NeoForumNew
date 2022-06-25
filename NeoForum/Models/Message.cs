using NeoForum.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace NeoForum.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Text { get; set; }
        public DateTime When { get; set; }

        public string? UserID { get; set; }

        public virtual NeoForumUser? Sender { get; set; }

        public Message()
        {
            When = DateTime.Now;
        }
    }
}
