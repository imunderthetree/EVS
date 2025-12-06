using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class ForgotPasswordModel : PageModel
{
    [BindProperty]
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Message { get; set; } = "";

    public void OnGet()
    {
        // Purely UI for now
    }

    public void OnPost()
    {
        // Placeholder for sending recovery code
        Message = "Recovery code logic not implemented yet";
    }
}
