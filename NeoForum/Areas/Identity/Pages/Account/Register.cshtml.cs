// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using NeoForum.Areas.Identity.Data;
using NeoForum.Models.Enums;

namespace NeoForum.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<NeoForumUser> _signInManager;
        private readonly UserManager<NeoForumUser> _userManager;
        private readonly IUserStore<NeoForumUser> _userStore;
        private readonly IUserEmailStore<NeoForumUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<NeoForumUser> userManager,
            IUserStore<NeoForumUser> userStore,
            SignInManager<NeoForumUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

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
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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
            /// 
            [Required(ErrorMessage = "Имя не указано")]
            [DataType(DataType.Text)]
            [Display(Name = "Имя")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Фамилия не указана")]
            [DataType(DataType.Text)]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Адрес почты не указан")]
            [EmailAddress (ErrorMessage = "Неверный формат почты")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Имя пользователя не указано")]
            [DataType(DataType.Text)]
            [StringLength(15, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 15 символов")]
            [Display(Name = "Имя пользователя")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Страна не указана")]
            [Display(Name = "Страна")]
            public Country Country { get; set; }

            [Required(ErrorMessage = "Возраст не указан")]
            [Range(14, 80, ErrorMessage = "Недопустимый возраст")]
            [Display(Name = "Возраст")]
            public int Age { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Пароль не указан")]
            [StringLength(100, ErrorMessage = "{0} должно быть, по крайней мере {2} и максимально {1} символов...", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Подтвердите Пароль")]
            [Compare("Password", ErrorMessage = "Пароли не совпадают...")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                this.Response.Redirect("/");
            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                //var user = CreateUser();
                var user = new NeoForumUser
                {
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    UserName = Input.UserName,
                    Country = Input.Country,
                    Age = Input.Age,
                    Email = Input.Email
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        /*private NeoForumUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<NeoForumUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(NeoForumUser)}'. " +
                    $"Ensure that '{nameof(NeoForumUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }*/

        private IUserEmailStore<NeoForumUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<NeoForumUser>)_userStore;
        }
    }
}
