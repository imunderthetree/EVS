using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVS.Pages.Account
{
    public class LogoutPageModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Clear session
            HttpContext.Session.Clear();
            
            // Redirect to login page
            return RedirectToPage("/Account/Login");
        }
    }
}