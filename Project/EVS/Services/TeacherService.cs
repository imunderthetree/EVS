using System.Data;

namespace EVS.Services
{
    public class TeacherService : DatabaseService
    {
        public TeacherService(IConfiguration configuration) : base(configuration) { }

        public async Task<DataTable> GetAllTeachersAsync()
        {
            var query = @"SELECT t.TeacherID, t.FullName, t.Email,
                         STRING_AGG(sp.SpecializationName, ', ') as Specializations
                         FROM Teacher t
                         LEFT JOIN Teacher_Specialization ts ON t.TeacherID = ts.TeacherID
                         LEFT JOIN Specialization sp ON ts.SpecializationID = sp.SpecializationID
                         GROUP BY t.TeacherID, t.FullName, t.Email
                         ORDER BY t.FullName";

            return await ExecuteQueryAsync(query);
        }

        public async Task<bool> AddTeacherAsync(string fullName, string email)
        {
            var query = "INSERT INTO Teacher (FullName, Email) VALUES (@name, @email)";
            var parameters = new Dictionary<string, object>
            {
                { "@name", fullName },
                { "@email", email }
            };

            await ExecuteNonQueryAsync(query, parameters);
            return true;
        }

        public async Task<int> GetTotalTeachersCountAsync()
        {
            var result = await ExecuteScalarAsync("SELECT COUNT(*) FROM Teacher");
            return Convert.ToInt32(result);
        }
    }
}
