using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EVS.Services;

namespace EVS.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly AuthService _authService;

        public RegisterModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public string AccountType { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full name")]
        public string FullName { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? Message { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(nameof(ConfirmPassword), "Passwords do not match.");
                return Page();
            }

            var success = await _authService.RegisterUserAsync(FullName, Email, Password, AccountType);

            if (success)
            {
                Message = $"Registration successful as {AccountType}! You can now log in with your credentials.";
                ModelState.Clear();

                // Clear form
                AccountType = string.Empty;
                FullName = string.Empty;
                Email = string.Empty;
                Password = string.Empty;
                ConfirmPassword = string.Empty;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This email is already registered.");
            }

            return Page();
        }
    }
}