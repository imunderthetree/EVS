using System;

namespace EVS.Models
{
    public class AttendanceRecord
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ClassName { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } // Present, Absent, Late, etc.
        public string Justification { get; set; }
    }
}
