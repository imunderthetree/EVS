using System.Data;

namespace EVS.Services
{
    public class AssignmentService : DatabaseService
    {
        public AssignmentService(IConfiguration configuration) : base(configuration) { }

        public async Task<DataTable> GetAllAssignmentsAsync()
        {
            var query = @"SELECT a.AssignmentID, a.Title, s.SubjectName, a.TotalGrade
                         FROM Assignment a
                         INNER JOIN Subject s ON a.SubjectID = s.SubjectID
                         ORDER BY a.AssignmentID DESC";

            return await ExecuteQueryAsync(query);
        }

        public async Task<bool> AddAssignmentAsync(string title, int subjectId, decimal totalGrade)
        {
            var query = @"INSERT INTO Assignment (Title, SubjectID, TotalGrade) 
                         VALUES (@title, @subjectId, @grade)";

            var parameters = new Dictionary<string, object>
            {
                { "@title", title },
                { "@subjectId", subjectId },
                { "@grade", totalGrade }
            };

            await ExecuteNonQueryAsync(query, parameters);
            return true;
        }
    }
}