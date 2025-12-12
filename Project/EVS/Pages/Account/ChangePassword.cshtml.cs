using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class ChangePasswordModel : PageModel
{
    [BindProperty]
    [Required]
    public string OldPassword { get; set; }

    [BindProperty]
    [Required]
    public string NewPassword { get; set; }

    [BindProperty]
    [Required]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }

    public string Message { get; set; } = "";

    public void OnGet()
    {
        // Purely UI for now
    }

    public void OnPost()
    {
        // Placeholder for changing password logic
        Message = "Change password logic not implemented yet";
    }
}
