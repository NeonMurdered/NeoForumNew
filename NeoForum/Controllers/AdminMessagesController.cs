using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeoForum.Areas.Identity.Data;
using NeoForum.Data;
using NeoForum.Models;
using System.Diagnostics;

namespace NeoForum.Controllers
{
    [Authorize(Roles = "Admin, Moderator")]
    public class AdminMessagesController : Controller
    {
        public readonly NeoForumDbContext _context;
        private readonly ILogger<AdminMessagesController> _logger;
        public readonly UserManager<NeoForumUser> _userManager;

        public AdminMessagesController(NeoForumDbContext context, ILogger<AdminMessagesController> logger, UserManager<NeoForumUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.UserName;
            }
            var adminmessages = await _context.AdminMessages.ToListAsync();
            return View(adminmessages);
        }

        public async Task<IActionResult> Create(AdminMessage adminmessage)
        {
            if (ModelState.IsValid)
            {
                adminmessage.UserName = User.Identity.Name;
                var adminsender = await _userManager.GetUserAsync(User);
                adminmessage.UserID = adminsender.Id;
                await _context.AdminMessages.AddAsync(adminmessage);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Error();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
