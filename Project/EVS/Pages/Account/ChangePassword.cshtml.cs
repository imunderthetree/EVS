using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using EVS.Services;

namespace EVS.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        private readonly AuthService _authService;

        public ChangePasswordModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Old password is required")]
        public string OldPassword { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "New password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string NewPassword { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Please confirm your new password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string Message { get; set; } = "";

        public IActionResult OnGet()
        {
            // Check if user is logged in
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (!ModelState.IsValid)
            {
                Message = "Please fill in all fields correctly";
                return Page();
            }

            if (NewPassword != ConfirmPassword)
            {
                Message = "New passwords do not match";
                return Page();
            }

            var success = await _authService.ChangePasswordAsync(adminId.Value, OldPassword, NewPassword);

            if (success)
            {
                Message = "Password changed successfully!";
                ModelState.Clear();

                // Clear form
                OldPassword = string.Empty;
                NewPassword = string.Empty;
                ConfirmPassword = string.Empty;
            }
            else
            {
                Message = "Old password is incorrect";
            }

            return Page();
        }
    }
}