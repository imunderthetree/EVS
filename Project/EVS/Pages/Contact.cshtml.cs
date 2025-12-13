using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVS.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        [Required]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [Display(Name = "Message")]
        public string MessageBody { get; set; } = string.Empty;

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

            // TODO: send message to inbox / create support ticket.
            Message = "Thanks — your message has been received. We'll reply by email shortly.";
            ModelState.Clear();
            return Page();
        }
    }
}