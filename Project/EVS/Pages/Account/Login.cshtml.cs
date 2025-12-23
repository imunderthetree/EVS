using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using EVS.Services;

namespace EVS.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly AuthService _authService;

        public LoginModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Account type is required")]
        public string AccountType { get; set; } = "Admin";

        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
            // Clear any existing session
            HttpContext.Session.Clear();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill in all required fields";
                return Page();
            }

            var (success, userId, fullName, accountType) = await _authService.ValidateLoginAsync(Username, Password, AccountType);

            if (success && userId.HasValue)
            {
                // Store user info in session
                HttpContext.Session.SetInt32($"{accountType}Id", userId.Value);
                HttpContext.Session.SetString("AdminName", fullName ?? "User");
                HttpContext.Session.SetString("Username", Username);
                HttpContext.Session.SetString("AccountType", accountType ?? "Admin");

                // Redirect based on account type
                return accountType switch
                {
                    "Teacher" => RedirectToPage("/Teacher/Dashboard"),
                    "Student" => RedirectToPage("/Student/Dashboard"),
                    "Parent" => RedirectToPage("/Parent/Dashboard"),
                    _ => RedirectToPage("/Admin/Dashboard")
                };
            }

            ErrorMessage = "Invalid username or password";
            return Page();
        }
    }
}