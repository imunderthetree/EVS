using System.Data;
namespace EVS.Services
{
    public class StudentService : DatabaseService
    {
        public StudentService(IConfiguration configuration) : base(configuration) { }

        public async Task<DataTable> GetAllStudentsAsync()
        {
            var query = @"SELECT s.StudentID, s.FullName, s.GradeLevel, s.ParentID,
                         p.FullName as ParentName
                         FROM Student s
                         LEFT JOIN Parent p ON s.ParentID = p.ParentID
                         ORDER BY s.FullName";

            return await ExecuteQueryAsync(query);
        }

        public async Task<DataTable> SearchStudentsAsync(string searchTerm, int? gradeLevel = null)
        {
            var query = @"SELECT s.StudentID, s.FullName, s.GradeLevel, s.ParentID,
                         p.FullName as ParentName
                         FROM Student s
                         LEFT JOIN Parent p ON s.ParentID = p.ParentID
                         WHERE s.FullName LIKE @search";

            if (gradeLevel.HasValue)
                query += " AND s.GradeLevel = @grade";

            query += " ORDER BY s.FullName";

            var parameters = new Dictionary<string, object>
            {
                { "@search", $"%{searchTerm}%" }
            };

            if (gradeLevel.HasValue)
                parameters.Add("@grade", gradeLevel.Value);

            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<DataTable> GetStudentsByParentAsync(int parentId)
        {
            var query = @"SELECT s.StudentID, s.FullName, s.GradeLevel, s.ParentID,
                         p.FullName as ParentName
                         FROM Student s
                         LEFT JOIN Parent p ON s.ParentID = p.ParentID
                         WHERE s.ParentID = @parentId
                         ORDER BY s.FullName";

            var parameters = new Dictionary<string, object> { { "@parentId", parentId } };
            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<int> GetTotalStudentsCountAsync()
        {
            var result = await ExecuteScalarAsync("SELECT COUNT(*) FROM Student");
            return Convert.ToInt32(result);
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            var parameters = new Dictionary<string, object> { { "@studentId", studentId } };
            var deleteQuery = "DELETE FROM Student WHERE StudentID = @studentId";
            await ExecuteNonQueryAsync(deleteQuery, parameters);
            return true;
        }

        public async Task<bool> AddStudentAsync(string fullName, int gradeLevel, int? parentId, string? email = null)
        {
            var query = @"INSERT INTO Student (FullName, GradeLevel, ParentID, Email) 
                         VALUES (@name, @grade, @parentId, @email)";

            var parameters = new Dictionary<string, object>
            {
                { "@name", fullName },
                { "@grade", gradeLevel },
                { "@parentId", (object?)parentId ?? DBNull.Value },
                { "@email", (object?)email ?? DBNull.Value }
            };

            await ExecuteNonQueryAsync(query, parameters);
            return true;
        }
    }
}