using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EVS.Services;

namespace EVS.Pages.Admission
{
    public class OnlineModel : PageModel
    {
        private readonly AdmissionService _admissionService;

        public OnlineModel(AdmissionService admissionService)
        {
            _admissionService = admissionService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Student name is required")]
        [Display(Name = "Student name")]
        public string StudentName { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Grade is required")]
        public string Grade { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Parent name is required")]
        [Display(Name = "Parent / guardian name")]
        public string ParentName { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Parent email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Parent email")]
        public string ParentEmail { get; set; } = string.Empty;

        [BindProperty]
        public string? Notes { get; set; }

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

            // Save admission application
            var success = await _admissionService.SubmitApplicationAsync(
                StudentName,
                DateOfBirth!.Value,
                Grade,
                ParentName,
                ParentEmail,
                Notes);

            if (success)
            {
                Message = "Application submitted successfully! We'll contact you with next steps.";
                ModelState.Clear();

                // Clear form
                StudentName = string.Empty;
                DateOfBirth = null;
                Grade = string.Empty;
                ParentName = string.Empty;
                ParentEmail = string.Empty;
                Notes = string.Empty;
            }
            else
            {
                Message = "Failed to submit application. Please try again.";
            }

            return Page();
        }
    }
}