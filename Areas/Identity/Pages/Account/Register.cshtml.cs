using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using LinkShortener.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using LinkShortener.Data;
using LinkShortener.Common;
using LinkShortener.Models;

namespace LinkShortener.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    //   [ValidateAntiForgeryToken]
    [IgnoreAntiforgeryToken]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RecaptchaHttpClient _recaptchaClient;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RecaptchaHttpClient recaptchaClient,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _recaptchaClient = recaptchaClient;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public string RefererId { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Entered email is not valid")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string ReferrerId { get; set; }
        }

        public void OnGet([FromRoute]string referrerId = null, string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            RefererId = referrerId;
        }



        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            string token = Request.Form["g-recaptcha-response"];

            bool success = await _recaptchaClient.ValidateToken(token);
            returnUrl = returnUrl ?? Url.Content("~/");
            if (success)
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, ReferrerId = Input.ReferrerId };
                    var result = await _userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        /*var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { userId = user.Id, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");*/


                        await _signInManager.SignInAsync(user, isPersistent: false);

                        await _context.AddReferrerLinkAsync(user.Id, Request.Scheme + "://" + Request.Host.Value + "/Identity/Account/Register");
                        await _context.SaveChangesAsync();
                        return LocalRedirect(returnUrl);
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            if(string.IsNullOrEmpty(token))
                ModelState.AddModelError(string.Empty, "You need to be validated by recaptcha");

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
