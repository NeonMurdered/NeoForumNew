using Microsoft.AspNetCore.Identity;
using NeoForum.Areas.Identity.Data;

namespace NeoForum.Models
{
    public class RoleEdit
    {
        public IdentityRole? Role { get; set; }
        public IEnumerable<NeoForumUser>? Members { get; set; }
        public IEnumerable<NeoForumUser>? NonMembers { get; set; }
    }
}
