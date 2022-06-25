using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeoForum.Areas.Identity.Data;
using NeoForum.Data;

namespace NeoForum.Controllers
{
    public class UsersController : Controller
    {
        private readonly NeoForumDbContext _context;
        private readonly ILogger<UsersController> _logger;
        public readonly UserManager<NeoForumUser> _userManager;

        public UsersController(NeoForumDbContext context, ILogger<UsersController> logger, UserManager<NeoForumUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }
    }
}
