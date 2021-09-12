﻿namespace MiniFootball.Areas.Identity.Pages.Account
{
    using AspNetCoreHero.ToastNotification.Abstractions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using MiniFootball.Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using static GlobalConstant.Notifications;

    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly INotyfService notifications;

        public LoginModel(
            SignInManager<User> signInManager,
            INotyfService notifications)
        {
            this.signInManager = signInManager;
            this.notifications = notifications;

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = GlobalConstant.Login.RememberMe)]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var result = await signInManager
                .PasswordSignInAsync(Input.Email,
                                     Input.Password,
                                     Input.RememberMe,
                                     false);

            if (result.Succeeded && Url.IsLocalUrl(returnUrl))
            {
                notifications.Success(Welcome, DurationInSeconds);
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                notifications.Error(InvalidEmailOrPassword);
                return Page();
            }
        }

        public IActionResult OnPostExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });

            var result = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, result);
        }

        public async Task <IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            ReturnUrl = returnUrl;
            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (remoteError != null)
            {
                
            }

            var info = await signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                
            }

            return Page();
        }
    }
}