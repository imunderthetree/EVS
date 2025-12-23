using System.Data;

namespace EVS.Services
{
    public class AttendanceService : DatabaseService
    {
        public AttendanceService(IConfiguration configuration) : base(configuration) { }

        public async Task<DataTable> GetStudentAttendanceAsync(int studentId)
        {
            var query = @"SELECT AttendanceID, AttendanceDate, Status 
                         FROM Attendance 
                         WHERE StudentID = @studentId 
                         ORDER BY AttendanceDate DESC";

            var parameters = new Dictionary<string, object> { { "@studentId", studentId } };
            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<Dictionary<string, int>> GetAttendanceSummaryAsync(int studentId)
        {
            var query = @"SELECT Status, COUNT(*) as Count 
                         FROM Attendance 
                         WHERE StudentID = @studentId 
                         GROUP BY Status";

            var parameters = new Dictionary<string, object> { { "@studentId", studentId } };
            var result = await ExecuteQueryAsync(query, parameters);

            var summary = new Dictionary<string, int>
            {
                { "Present", 0 },
                { "Absent", 0 },
                { "Late", 0 }
            };

            foreach (DataRow row in result.Rows)
            {
                var status = row["Status"].ToString();
                var count = Convert.ToInt32(row["Count"]);
                if (summary.ContainsKey(status!))
                {
                    summary[status!] = count;
                }
            }

            return summary;
        }

        public async Task<bool> RecordAttendanceAsync(int studentId, DateTime date, string status)
        {
            var query = @"INSERT INTO Attendance (StudentID, AttendanceDate, Status) 
                         VALUES (@studentId, @date, @status)";

            var parameters = new Dictionary<string, object>
            {
                { "@studentId", studentId },
                { "@date", date },
                { "@status", status }
            };

            await ExecuteNonQueryAsync(query, parameters);
            return true;
        }
    }
}