using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeoForum.Areas.Identity.Data;
using NeoForum.Data;
using NeoForum.Models;
using System.Diagnostics;

namespace NeoForum.Controllers
{
    public class MessagesController : Controller
    {
        public readonly NeoForumDbContext _context;
        private readonly ILogger<MessagesController> _logger;
        public readonly UserManager<NeoForumUser> _userManager;

        public MessagesController(NeoForumDbContext context, ILogger<MessagesController> logger, UserManager<NeoForumUser> userManager)
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
            var messages = await _context.Messages.ToListAsync();
            return View(messages);
        }

        public async Task<IActionResult> Create(Message message)
        {
            if (ModelState.IsValid)
            {
                message.UserName = User.Identity.Name;
                var sender = await _userManager.GetUserAsync(User);
                message.UserID = sender.Id;
                await _context.Messages.AddAsync(message);
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
