using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Admin
{
    public class ManageParentsModel : PageModel
    {
        private readonly ParentService _parentService;

        public ManageParentsModel(ParentService parentService)
        {
            _parentService = parentService;
        }

        public DataTable Parents { get; set; } = new DataTable();

        public async Task<IActionResult> OnGetAsync()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Parents = await _parentService.GetAllParentsAsync();
            return Page();
        }
    }
}