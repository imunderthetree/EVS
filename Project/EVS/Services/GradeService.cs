using System.Data;

namespace EVS.Services
{
    public class GradeService : DatabaseService
    {
        public GradeService(IConfiguration configuration) : base(configuration) { }

        public async Task<DataTable> GetAssignmentGradesAsync(int assignmentId)
        {
            var query = @"SELECT sa.StudentID, s.FullName, sa.Grade, a.TotalGrade
                         FROM Student_Assignment sa
                         INNER JOIN Student s ON sa.StudentID = s.StudentID
                         INNER JOIN Assignment a ON sa.AssignmentID = a.AssignmentID
                         WHERE sa.AssignmentID = @assignmentId
                         ORDER BY s.FullName";

            var parameters = new Dictionary<string, object> { { "@assignmentId", assignmentId } };
            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<DataTable> GetExamGradesAsync(int examId)
        {
            var query = @"SELECT se.StudentID, s.FullName, se.Score
                         FROM Student_Exam se
                         INNER JOIN Student s ON se.StudentID = s.StudentID
                         WHERE se.ExamID = @examId
                         ORDER BY s.FullName";

            var parameters = new Dictionary<string, object> { { "@examId", examId } };
            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<bool> GradeAssignmentAsync(int studentId, int assignmentId, decimal grade)
        {
            // Check if grade exists
            var checkQuery = "SELECT COUNT(*) FROM Student_Assignment WHERE StudentID = @studentId AND AssignmentID = @assignmentId";
            var checkParams = new Dictionary<string, object>
            {
                { "@studentId", studentId },
                { "@assignmentId", assignmentId }
            };

            var exists = Convert.ToInt32(await ExecuteScalarAsync(checkQuery, checkParams)) > 0;

            string query;
            if (exists)
            {
                query = @"UPDATE Student_Assignment SET Grade = @grade 
                         WHERE StudentID = @studentId AND AssignmentID = @assignmentId";
            }
            else
            {
                query = @"INSERT INTO Student_Assignment (StudentID, AssignmentID, Grade) 
                         VALUES (@studentId, @assignmentId, @grade)";
            }

            var parameters = new Dictionary<string, object>
            {
                { "@studentId", studentId },
                { "@assignmentId", assignmentId },
                { "@grade", grade }
            };

            await ExecuteNonQueryAsync(query, parameters);
            return true;
        }

        public async Task<bool> GradeExamAsync(int studentId, int examId, decimal score)
        {
            var checkQuery = "SELECT COUNT(*) FROM Student_Exam WHERE StudentID = @studentId AND ExamID = @examId";
            var checkParams = new Dictionary<string, object>
            {
                { "@studentId", studentId },
                { "@examId", examId }
            };

            var exists = Convert.ToInt32(await ExecuteScalarAsync(checkQuery, checkParams)) > 0;

            string query;
            if (exists)
            {
                query = @"UPDATE Student_Exam SET Score = @score 
                         WHERE StudentID = @studentId AND ExamID = @examId";
            }
            else
            {
                query = @"INSERT INTO Student_Exam (StudentID, ExamID, Score) 
                         VALUES (@studentId, @examId, @score)";
            }

            var parameters = new Dictionary<string, object>
            {
                { "@studentId", studentId },
                { "@examId", examId },
                { "@score", score }
            };

            await ExecuteNonQueryAsync(query, parameters);
            return true;
        }
    }
}