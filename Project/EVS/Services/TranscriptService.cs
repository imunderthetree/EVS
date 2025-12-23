using System.Data;

namespace EVS.Services
{
    public class TranscriptService : DatabaseService
    {
        public TranscriptService(IConfiguration configuration) : base(configuration) { }

        public async Task<DataTable> GetStudentTranscriptAsync(int studentId)
        {
            var query = @"SELECT t.TranscriptID, t.Term, se.Score, e.ExamDate, s.SubjectName
                         FROM Transcript t
                         INNER JOIN Transcript_Exam te ON t.TranscriptID = te.TranscriptID
                         INNER JOIN Student_Exam se ON te.ExamID = se.ExamID
                         INNER JOIN Exam e ON se.ExamID = e.ExamID
                         INNER JOIN Subject s ON e.SubjectID = s.SubjectID
                         WHERE t.StudentID = @studentId AND se.StudentID = @studentId
                         ORDER BY t.Term, s.SubjectName";

            var parameters = new Dictionary<string, object> { { "@studentId", studentId } };
            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<DataTable> GetStudentGradesAsync(int studentId)
        {
            var query = @"SELECT s.SubjectName, sa.Grade, a.Title as AssignmentTitle
                         FROM Student_Assignment sa
                         INNER JOIN Assignment a ON sa.AssignmentID = a.AssignmentID
                         INNER JOIN Subject s ON a.SubjectID = s.SubjectID
                         WHERE sa.StudentID = @studentId
                         ORDER BY s.SubjectName";

            var parameters = new Dictionary<string, object> { { "@studentId", studentId } };
            return await ExecuteQueryAsync(query, parameters);
        }
    }
}