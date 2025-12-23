using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EVS.Services;

namespace EVS.Pages
{
    public class ContactModel : PageModel
    {
        private readonly MessageService _messageService;

        public ContactModel(MessageService messageService)
        {
            _messageService = messageService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Message is required")]
        [Display(Name = "Message")]
        public string MessageBody { get; set; } = string.Empty;

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

            // Store contact message - you could create a ContactMessage table or use Message table
            // For now, we'll send it to admin (AdminID = 1)
            await _messageService.SendMessageAsync(0, 1, $"Contact from {Name} ({Email}): {MessageBody}");

            Message = "Thank you for contacting us! We'll get back to you soon.";
            ModelState.Clear();

            // Clear form
            Name = string.Empty;
            Email = string.Empty;
            MessageBody = string.Empty;

            return Page();
        }
    }
}