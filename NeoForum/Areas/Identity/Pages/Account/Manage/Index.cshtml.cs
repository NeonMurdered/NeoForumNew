// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NeoForum.Areas.Identity.Data;
using NeoForum.Models.Enums;

namespace NeoForum.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<NeoForumUser> _userManager;
        private readonly SignInManager<NeoForumUser> _signInManager;

        public IndexModel(
            UserManager<NeoForumUser> userManager,
            SignInManager<NeoForumUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Имя")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Имя пользователя")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress(ErrorMessage = "Неверный формат почты")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Страна")]
            public Country Country { get; set; }

            [Required]
            [Range(14, 80, ErrorMessage = "Недопустимый возраст")]
            [Display(Name = "Возраст")]
            public int Age { get; set; }

            [Required]
            [Phone]
            [Display(Name = "Номер телефона")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Изображение профиля")]
            public byte[] ProfilePicture { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            Input = new InputModel
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Country = user.Country,
                Age = user.Age,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.UserName != user.UserName)
            {
                user.UserName = Input.UserName;
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }

            if (Input.Email != user.Email)
            {
                user.Email = Input.Email;
            }

            if (Input.Age != user.Age)
            {
                user.Age = Input.Age;
            }

            if (Input.Country != user.Country)
            {
                user.Country = Input.Country;
            }

            if (Input.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = Input.PhoneNumber;
            }

            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    user.ProfilePicture = dataStream.ToArray();
                }
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Ваш профиль обновлён";
            return RedirectToPage();
        }
    }
}
