using System.Data;
using System.Threading.Tasks;

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

        public async Task<DataTable> GetClassroomAssignmentsAsync(int classroomId)
        {
            var query = "SELECT * FROM Assignments WHERE ClassroomId = @ClassroomId";
            var parameters = new Dictionary<string, object>
            {
                { "@ClassroomId", classroomId }
            };
            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<DataTable> GetClassroomScheduleAsync(int classroomId)
        {
            var query = "SELECT * FROM ClassroomSchedules WHERE ClassroomId = @ClassroomId";
            var parameters = new Dictionary<string, object>
            {
                { "@ClassroomId", classroomId }
            };
            return await ExecuteQueryAsync(query, parameters);
        }
    }
}