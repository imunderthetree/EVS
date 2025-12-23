using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EVS.Services;

namespace EVS.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly AuthService _authService;

        public ForgotPasswordModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [TempData]
        public string? Message { get; set; }

        public string LayoutName { get; private set; } = "_Layout";

        public void OnGet()
        {
            var layoutQuery = Request.Query["layout"].ToString();
            if (!string.IsNullOrEmpty(layoutQuery) && layoutQuery.Equals("auth", System.StringComparison.OrdinalIgnoreCase))
            {
                LayoutName = "_AuthLayout";
                return;
            }

            if (Request.Cookies.TryGetValue("evs_layout", out var cookie) && cookie == "auth")
            {
                LayoutName = "_AuthLayout";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            OnGet();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Send recovery code (in production, this would send an actual email)
            await _authService.SendPasswordRecoveryAsync(Email!, "user");

            Message = "If that email exists in our system, a recovery code has been sent.";

            return Page();
        }
    }
}