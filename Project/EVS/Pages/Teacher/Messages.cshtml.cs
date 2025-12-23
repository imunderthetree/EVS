using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Teacher
{
    public class MessagesModel : PageModel
    {
        private readonly MessageService _messageService;
        private readonly StudentService _studentService;

        public MessagesModel(MessageService messageService, StudentService studentService)
        {
            _messageService = messageService;
            _studentService = studentService;
        }

        public DataTable Messages { get; set; } = new DataTable();
        public DataTable Students { get; set; } = new DataTable();
        public int TeacherId { get; set; }

        [BindProperty]
        public int ReceiverId { get; set; }

        [BindProperty]
        public string MessageContent { get; set; } = string.Empty;

        public string? SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            TeacherId = teacherId.Value;
            Messages = await _messageService.GetUserMessagesAsync(teacherId.Value);
            Students = await _studentService.GetAllStudentsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostSendAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (string.IsNullOrWhiteSpace(MessageContent) || ReceiverId <= 0)
            {
                await LoadDataAsync(teacherId.Value);
                return Page();
            }

            await _messageService.SendMessageAsync(teacherId.Value, ReceiverId, MessageContent);
            SuccessMessage = "Message sent successfully!";

            MessageContent = string.Empty;
            await LoadDataAsync(teacherId.Value);
            return Page();
        }

        private async Task LoadDataAsync(int teacherId)
        {
            TeacherId = teacherId;
            Messages = await _messageService.GetUserMessagesAsync(teacherId);
            Students = await _studentService.GetAllStudentsAsync();
        }
    }
}
