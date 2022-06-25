using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NeoForum.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        public ViewResult Index() => View(User?.Claims);
    }
}
