using System.Data;

namespace EVS.Services
{
    public class ParentService : DatabaseService
    {
        public ParentService(IConfiguration configuration) : base(configuration) { }

        public async Task<DataTable> GetAllParentsAsync()
        {
            var query = @"SELECT p.ParentID, p.FullName, p.Email, p.PhoneNumber,
                         GROUP_CONCAT(s.FullName SEPARATOR ', ') as Students
                         FROM Parent p
                         LEFT JOIN Student s ON p.ParentID = s.ParentID
                         GROUP BY p.ParentID, p.FullName, p.Email, p.PhoneNumber
                         ORDER BY p.FullName";

            return await ExecuteQueryAsync(query);
        }

        public async Task<int> GetTotalParentsCountAsync()
        {
            var result = await ExecuteScalarAsync("SELECT COUNT(*) FROM Parent");
            return Convert.ToInt32(result);
        }
    }
}