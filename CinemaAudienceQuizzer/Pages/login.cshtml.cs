using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace CinemaAudienceQuizzer.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string ErrorMessage { get; set; }

        // Replace with your actual hashed password
        private const string AdminUsername = "admin";
        private const string AdminPasswordHash = "AQAAAAIAAYagAAAAEMIOFrEyXtzUTv7tQzk8ZKgcxg3R3Hp4YmWjbJffebwSgCFpDDmlh8ecXYiT+9jOkQ==";

        public async Task<IActionResult> OnPostAsync()
        {
            var hasher = new PasswordHasher<object>();
            var result = hasher.VerifyHashedPassword(null, AdminPasswordHash, Input.Password);

            if (Input.Username == AdminUsername && result == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, AdminUsername),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToPage("/Admin");
            }
            else
            {
                ErrorMessage = "Invalid credentials.";
                return Page();
            }
        }

        public class InputModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
