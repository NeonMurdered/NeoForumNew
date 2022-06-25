using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NeoForum.Areas.Identity.Data;

namespace NeoForum.TagHelpers
{
    [HtmlTargetElement("td", Attributes = "i-role")]
    public class RoleUsersTH : TagHelper
    {
        private UserManager<NeoForumUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public RoleUsersTH(UserManager<NeoForumUser> usermgr, RoleManager<IdentityRole> rolemgr)
        {
            userManager = usermgr;
            roleManager = rolemgr;
        }

        [HtmlAttributeName("i-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();
            IdentityRole role = await roleManager.FindByIdAsync(Role);
            if (role != null)
            {
                foreach (var user in userManager.Users)
                {
                    if (user != null && await userManager.IsInRoleAsync(user, role.Name))
                        names.Add(user.UserName);
                }
            }
            output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
        }
    }
}
