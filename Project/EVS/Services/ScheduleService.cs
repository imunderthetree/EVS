using System.Data;

namespace EVS.Services
{
    public class ScheduleService : DatabaseService
    {
        public ScheduleService(IConfiguration configuration) : base(configuration) { }

        public async Task<DataTable> GetScheduleAsync()
        {
            var query = @"SELECT sch.ScheduleID, sch.DayOfWeek, sch.StartTime, sch.EndTime,
                         s.SubjectName, c.ClassroomName
                         FROM Schedule sch
                         INNER JOIN Subject s ON sch.SubjectID = s.SubjectID
                         INNER JOIN Classroom c ON sch.ClassroomID = c.ClassroomID
                         ORDER BY CASE sch.DayOfWeek 
                             WHEN 'Monday' THEN 1 
                             WHEN 'Tuesday' THEN 2 
                             WHEN 'Wednesday' THEN 3 
                             WHEN 'Thursday' THEN 4 
                             WHEN 'Friday' THEN 5 
                             WHEN 'Saturday' THEN 6 
                             WHEN 'Sunday' THEN 7 
                             ELSE 8 END,
                         sch.StartTime";

            return await ExecuteQueryAsync(query);
        }

        public async Task<DataTable> GetStudentScheduleAsync(int studentId)
        {
            var query = @"SELECT sch.ScheduleID, sch.DayOfWeek, sch.StartTime, sch.EndTime,
                         s.SubjectName, c.ClassroomName
                         FROM Schedule sch
                         INNER JOIN Subject s ON sch.SubjectID = s.SubjectID
                         INNER JOIN Classroom c ON sch.ClassroomID = c.ClassroomID
                         INNER JOIN Student_Subject ss ON s.SubjectID = ss.SubjectID
                         WHERE ss.StudentID = @studentId
                         ORDER BY CASE sch.DayOfWeek 
                             WHEN 'Monday' THEN 1 
                             WHEN 'Tuesday' THEN 2 
                             WHEN 'Wednesday' THEN 3 
                             WHEN 'Thursday' THEN 4 
                             WHEN 'Friday' THEN 5 
                             WHEN 'Saturday' THEN 6 
                             WHEN 'Sunday' THEN 7 
                             ELSE 8 END,
                         sch.StartTime";

            var parameters = new Dictionary<string, object> { { "@studentId", studentId } };
            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<bool> CreateScheduleAsync(int subjectId, int classroomId, string dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            var query = @"INSERT INTO Schedule (SubjectID, ClassroomID, DayOfWeek, StartTime, EndTime) 
                         VALUES (@subjectId, @classroomId, @day, @start, @end)";

            var parameters = new Dictionary<string, object>
            {
                { "@subjectId", subjectId },
                { "@classroomId", classroomId },
                { "@day", dayOfWeek },
                { "@start", startTime },
                { "@end", endTime }
            };

            await ExecuteNonQueryAsync(query, parameters);
            return true;
        }
    }
}