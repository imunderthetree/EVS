using System.Data;

namespace EVS.Services
{
    public class ClassroomService : DatabaseService
    {
        public ClassroomService(IConfiguration configuration) : base(configuration) { }

        public async Task<DataTable> GetAllClassroomsAsync()
        {
            var query = "SELECT ClassroomID, ClassroomName FROM Classroom ORDER BY ClassroomName";
            return await ExecuteQueryAsync(query);
        }

        public async Task<bool> AddClassroomAsync(string classroomName)
        {
            var query = "INSERT INTO Classroom (ClassroomName) VALUES (@name)";
            var parameters = new Dictionary<string, object> { { "@name", classroomName } };

            await ExecuteNonQueryAsync(query, parameters);
            return true;
        }
    }
}