using System.Data;

namespace EVS.Services
{
    public class AdmissionService : DatabaseService
    {
        public AdmissionService(IConfiguration configuration) : base(configuration) { }

        public async Task<bool> SubmitApplicationAsync(
            string studentName,
            DateTime dateOfBirth,
            string grade,
            string parentName,
            string parentEmail,
            string? notes)
        {
            // First, create or get parent
            var parentQuery = @"INSERT INTO Parent (FullName, Email) VALUES (@name, @email);
                               SELECT LAST_INSERT_ID();";

            var parentParams = new Dictionary<string, object>
            {
                { "@name", parentName },
                { "@email", parentEmail }
            };

            var parentId = Convert.ToInt32(await ExecuteScalarAsync(parentQuery, parentParams));

            // Calculate grade level from grade string
            int gradeLevel = 0;
            if (int.TryParse(grade, out int parsedGrade))
            {
                gradeLevel = parsedGrade;
            }

            // Create student with "Pending" status (you can add a status field if needed)
            var studentQuery = @"INSERT INTO Student (FullName, GradeLevel, ParentID) 
                                VALUES (@name, @grade, @parentId)";

            var studentParams = new Dictionary<string, object>
            {
                { "@name", studentName },
                { "@grade", gradeLevel },
                { "@parentId", parentId }
            };

            await ExecuteNonQueryAsync(studentQuery, studentParams);

            return true;
        }
    }
}