using System.Data;

namespace EVS.Services
{
    public class SubjectService : DatabaseService
    {
        public SubjectService(IConfiguration configuration) : base(configuration) { }

        public async Task<DataTable> GetAllSubjectsAsync()
        {
            var query = "SELECT SubjectID, SubjectName FROM Subject ORDER BY SubjectName";
            return await ExecuteQueryAsync(query);
        }

        public async Task<bool> AddSubjectAsync(string subjectName)
        {
            var query = "INSERT INTO Subject (SubjectName) VALUES (@name)";
            var parameters = new Dictionary<string, object> { { "@name", subjectName } };

            await ExecuteNonQueryAsync(query, parameters);
            return true;
        }

        public async Task<bool> DeleteSubjectAsync(int subjectId)
        {
            var query = "DELETE FROM Subject WHERE SubjectID = @id";
            var parameters = new Dictionary<string, object> { { "@id", subjectId } };

            await ExecuteNonQueryAsync(query, parameters);
            return true;
        }

        public async Task<int> GetTotalSubjectsCountAsync()
        {
            var result = await ExecuteScalarAsync("SELECT COUNT(*) FROM Subject");
            return Convert.ToInt32(result);
        }
    }
}