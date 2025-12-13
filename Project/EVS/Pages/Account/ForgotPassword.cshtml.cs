using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVS.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        [BindProperty]
        [Required, EmailAddress]
        public string? Email { get; set; }

        [TempData]
        public string? Message { get; set; }

        // LayoutName controls which layout the page uses. Defaults to the app's main layout.
        public string LayoutName { get; private set; } = "_Layout";

        public void OnGet()
        {
            // Allow explicit query string choice: ?layout=auth will use the auth layout
            var layoutQuery = Request.Query["layout"].ToString();
            if (!string.IsNullOrEmpty(layoutQuery) && layoutQuery.Equals("auth", System.StringComparison.OrdinalIgnoreCase))
            {
                LayoutName = "_AuthLayout";
                return;
            }

            // Or allow cookie-based preference set by the client toggle button
            if (Request.Cookies.TryGetValue("evs_layout", out var cookie) && cookie == "auth")
            {
                LayoutName = "_AuthLayout";
            }
        }

        public IActionResult OnPost()
        {
            // preserve layout selection on postback
            OnGet();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // TODO: Replace with real recovery code send logic (Identity/email service)
            // For now simulate success and show friendly message.
            Message = "If that email exists in our system, a recovery code has been sent.";

            // Stay on the page and show the message
            return Page();
        }
    }
}
