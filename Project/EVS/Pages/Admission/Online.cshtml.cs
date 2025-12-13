using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVS.Pages.Admission
{
    public class OnlineModel : PageModel
    {
        [BindProperty]
        [Required]
        [Display(Name = "Student name")]
        public string StudentName { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [BindProperty]
        [Required]
        public string Grade { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [Display(Name = "Parent / guardian name")]
        public string ParentName { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [EmailAddress]
        [Display(Name = "Parent email")]
        public string ParentEmail { get; set; } = string.Empty;

        [BindProperty]
        public string? Notes { get; set; }

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

            // TODO: persist application to database / send notifications.
            Message = "Application submitted. We'll contact you with next steps.";
            ModelState.Clear();
            return Page();
        }
    }
}