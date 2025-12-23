using System;

namespace EVS.Models
{
    public class ClassSession
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Student or teacher
        public string Subject { get; set; }
        public string TeacherName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
    }
}
