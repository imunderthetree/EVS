using System.Data;

namespace EVS.Services
{
    public class ExamService : DatabaseService
    {
        public ExamService(IConfiguration configuration) : base(configuration) { }

        public async Task<DataTable> GetAllExamsAsync()
        {
            var query = @"SELECT e.ExamID, e.ExamType, s.SubjectName, e.ExamDate
                         FROM Exam e
                         INNER JOIN Subject s ON e.SubjectID = s.SubjectID
                         ORDER BY e.ExamDate DESC";

            return await ExecuteQueryAsync(query);
        }

        public async Task<bool> AddExamAsync(int subjectId, DateTime examDate, string examType)
        {
            var query = @"INSERT INTO Exam (SubjectID, ExamDate, ExamType) 
                         VALUES (@subjectId, @date, @type)";

            var parameters = new Dictionary<string, object>
            {
                { "@subjectId", subjectId },
                { "@date", examDate },
                { "@type", examType }
            };

            await ExecuteNonQueryAsync(query, parameters);
            return true;
        }
    }
}
