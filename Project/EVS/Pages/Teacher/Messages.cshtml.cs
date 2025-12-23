using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Teacher
{
    public class MessagesModel : PageModel
    {
        private readonly MessageService _messageService;

        public MessagesModel(MessageService messageService)
        {
            _messageService = messageService;
        }

        public DataTable Messages { get; set; } = new DataTable();

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Messages = await _messageService.GetUserMessagesAsync(teacherId.Value);

            return Page();
        }
    }
}
