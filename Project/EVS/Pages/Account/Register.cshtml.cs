using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVS.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        [Required]
        [Display(Name = "Full name")]
        public string FullName { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? Message { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
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

            // TODO: hook into your user creation / identity flow.
            Message = "Registration received. Check your email for verification (placeholder).";
            ModelState.Clear();
            return Page();
        }
    }
}