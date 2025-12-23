using System.Data;
namespace EVS.Services
{
    public class StudentService : DatabaseService
    {
        public StudentService(IConfiguration configuration) : base(configuration) { }

        public async Task<DataTable> GetAllStudentsAsync()
        {
            var query = @"SELECT s.StudentID, s.FullName, s.GradeLevel, 
                         p.FullName as ParentName
                         FROM Student s
                         LEFT JOIN Parent p ON s.ParentID = p.ParentID
                         ORDER BY s.FullName";

            return await ExecuteQueryAsync(query);
        }

        public async Task<DataTable> SearchStudentsAsync(string searchTerm, int? gradeLevel = null)
        {
            var query = @"SELECT s.StudentID, s.FullName, s.GradeLevel, 
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

        public async Task<int> GetTotalStudentsCountAsync()
        {
            var result = await ExecuteScalarAsync("SELECT COUNT(*) FROM Student");
            return Convert.ToInt32(result);
        }
    }
}