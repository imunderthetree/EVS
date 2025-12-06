using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class LoginModel : PageModel
{
    [BindProperty]
    [Required]
    public string Username { get; set; }

    [BindProperty]
    [Required]
    public string Password { get; set; }

    public string ErrorMessage { get; set; } = ""; // Placeholder for UI

    public void OnGet()
    {
        // No logic yet; purely UI
    }

    public void OnPost()
    {
        // Placeholder for post logic later
        ErrorMessage = "Login logic not implemented yet";
    }
}
