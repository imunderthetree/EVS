using System.Security.Cryptography;
using System.Text;
using System.Data;

namespace EVS.Services
{
    public class AuthService : DatabaseService
    {
        public AuthService(IConfiguration configuration) : base(configuration) { }

        public async Task<(bool success, int? userId, string? fullName, string? accountType)> ValidateLoginAsync(string username, string password, string accountType)
        {
            var hashedPassword = HashPassword(password);
            
            string query;
            string idColumn;
            string nameColumn = "FullName"; // Default column name
            
            switch (accountType)
            {
                case "Teacher":
                    query = @"SELECT TeacherID, FullName FROM Teacher 
                             WHERE Email = @username AND PasswordHash = @password";
                    idColumn = "TeacherID";
                    break;
                    
                case "Student":
                    query = @"SELECT StudentID, FullName FROM Student 
                             WHERE Email = @username AND PasswordHash = @password";
                    idColumn = "StudentID";
                    break;
                    
                case "Parent":
                    query = @"SELECT ParentID, FullName FROM Parent 
                             WHERE Email = @username AND PasswordHash = @password";
                    idColumn = "ParentID";
                    break;
                    
                default: // Admin
                    query = @"SELECT AdminID, FullName FROM Administrator 
                             WHERE Username = @username AND PasswordHash = @password";
                    idColumn = "AdminID";
                    break;
            }

            var parameters = new Dictionary<string, object>
            {
                { "@username", username },
                { "@password", hashedPassword }
            };

            try
            {
                var result = await ExecuteQueryAsync(query, parameters);

                if (result.Rows.Count > 0)
                {
                    var row = result.Rows[0];
                    return (
                        true, 
                        Convert.ToInt32(row[idColumn]), 
                        row[nameColumn]?.ToString() ?? "User",
                        accountType
                    );
                }
            }
            catch (Exception ex)
            {
                // Log the error (consider using ILogger in production)
                Console.WriteLine($"Login error: {ex.Message}");
            }

            return (false, null, null, null);
        }

        public async Task<bool> RegisterUserAsync(string fullName, string email, string password, string accountType)
        {
            var hashedPassword = HashPassword(password);
            
            // Check if user already exists in the appropriate table
            string checkQuery;
            
            switch (accountType)
            {
                case "Teacher":
                    checkQuery = "SELECT COUNT(*) FROM Teacher WHERE Email = @email";
                    break;
                    
                case "Student":
                    checkQuery = "SELECT COUNT(*) FROM Student WHERE Email = @email";
                    break;
                    
                case "Parent":
                    checkQuery = "SELECT COUNT(*) FROM Parent WHERE Email = @email";
                    break;
                    
                default: // Admin
                    checkQuery = "SELECT COUNT(*) FROM Administrator WHERE Username = @email";
                    break;
            }
            
            var checkParams = new Dictionary<string, object> { { "@email", email } };
            var exists = Convert.ToInt32(await ExecuteScalarAsync(checkQuery, checkParams));
            
            if (exists > 0)
                return false;

            // Insert into the appropriate table
            string insertQuery;
            Dictionary<string, object> parameters;
            
            switch (accountType)
            {
                case "Teacher":
                    insertQuery = @"INSERT INTO Teacher (FullName, Email, PasswordHash) 
                                   VALUES (@fullName, @email, @password)";
                    parameters = new Dictionary<string, object>
                    {
                        { "@fullName", fullName },
                        { "@email", email },
                        { "@password", hashedPassword }
                    };
                    break;
                    
                case "Student":
                    insertQuery = @"INSERT INTO Student (FullName, Email, PasswordHash) 
                                   VALUES (@fullName, @email, @password)";
                    parameters = new Dictionary<string, object>
                    {
                        { "@fullName", fullName },
                        { "@email", email },
                        { "@password", hashedPassword }
                    };
                    break;
                    
                case "Parent":
                    insertQuery = @"INSERT INTO Parent (FullName, Email, PasswordHash) 
                                   VALUES (@fullName, @email, @password)";
                    parameters = new Dictionary<string, object>
                    {
                        { "@fullName", fullName },
                        { "@email", email },
                        { "@password", hashedPassword }
                    };
                    break;
                    
                default: // Admin
                    insertQuery = @"INSERT INTO Administrator (Username, PasswordHash, FullName) 
                                   VALUES (@email, @password, @fullName)";
                    parameters = new Dictionary<string, object>
                    {
                        { "@email", email },
                        { "@password", hashedPassword },
                        { "@fullName", fullName }
                    };
                    break;
            }

            try
            {
                await ExecuteNonQueryAsync(insertQuery, parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword, string accountType)
        {
            var oldHashedPassword = HashPassword(oldPassword);
            var newHashedPassword = HashPassword(newPassword);

            string verifyQuery;
            string updateQuery;
            string idColumn;

            switch (accountType)
            {
                case "Teacher":
                    idColumn = "TeacherID";
                    verifyQuery = $"SELECT COUNT(*) FROM Teacher WHERE {idColumn} = @userId AND PasswordHash = @oldPassword";
                    updateQuery = $"UPDATE Teacher SET PasswordHash = @newPassword WHERE {idColumn} = @userId";
                    break;

                case "Student":
                    idColumn = "StudentID";
                    verifyQuery = $"SELECT COUNT(*) FROM Student WHERE {idColumn} = @userId AND PasswordHash = @oldPassword";
                    updateQuery = $"UPDATE Student SET PasswordHash = @newPassword WHERE {idColumn} = @userId";
                    break;

                case "Parent":
                    idColumn = "ParentID";
                    verifyQuery = $"SELECT COUNT(*) FROM Parent WHERE {idColumn} = @userId AND PasswordHash = @oldPassword";
                    updateQuery = $"UPDATE Parent SET PasswordHash = @newPassword WHERE {idColumn} = @userId";
                    break;

                default: // Admin
                    idColumn = "AdminID";
                    verifyQuery = $"SELECT COUNT(*) FROM Administrator WHERE {idColumn} = @userId AND PasswordHash = @oldPassword";
                    updateQuery = $"UPDATE Administrator SET PasswordHash = @newPassword WHERE {idColumn} = @userId";
                    break;
            }

            var verifyParams = new Dictionary<string, object>
            {
                { "@userId", userId },
                { "@oldPassword", oldHashedPassword }
            };

            var count = Convert.ToInt32(await ExecuteScalarAsync(verifyQuery, verifyParams));

            if (count == 0)
                return false;

            var updateParams = new Dictionary<string, object>
            {
                { "@userId", userId },
                { "@newPassword", newHashedPassword }
            };

            await ExecuteNonQueryAsync(updateQuery, updateParams);
            return true;
        }

        public async Task<bool> SendPasswordRecoveryAsync(string email, string accountType)
        {
            string query;
            
            switch (accountType)
            {
                case "Teacher":
                    query = "SELECT COUNT(*) FROM Teacher WHERE Email = @email";
                    break;
                case "Student":
                    query = "SELECT COUNT(*) FROM Student WHERE Email = @email";
                    break;
                case "Parent":
                    query = "SELECT COUNT(*) FROM Parent WHERE Email = @email";
                    break;
                default:
                    query = "SELECT COUNT(*) FROM Administrator WHERE Username = @email";
                    break;
            }

            var parameters = new Dictionary<string, object> { { "@email", email } };
            var count = Convert.ToInt32(await ExecuteScalarAsync(query, parameters));

            // Return true even if email doesn't exist (security best practice)
            // In production, you would send an actual email here
            return true;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}