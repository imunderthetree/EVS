using System;

namespace EVS.Models
{
    public class CourseGrade
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CourseName { get; set; }
        public string Term { get; set; }
        public string Grade { get; set; }
        public double GradePoint { get; set; }
        public int Credits { get; set; }
    }
}
