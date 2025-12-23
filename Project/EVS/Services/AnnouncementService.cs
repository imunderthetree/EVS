using System.Data;

namespace EVS.Services
{
    public class AnnouncementService : DatabaseService
    {
        public AnnouncementService(IConfiguration configuration) : base(configuration) { }

        public async Task<DataTable> GetAllAnnouncementsAsync()
        {
            var query = @"SELECT AnnouncementID, Title, Content, DatePosted 
                         FROM Announcement 
                         ORDER BY DatePosted DESC";

            return await ExecuteQueryAsync(query);
        }

        public async Task<DataTable> GetAnnouncementByIdAsync(int announcementId)
        {
            var query = @"SELECT AnnouncementID, Title, Content, DatePosted 
                         FROM Announcement 
                         WHERE AnnouncementID = @id";

            var parameters = new Dictionary<string, object> { { "@id", announcementId } };
            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<bool> CreateAnnouncementAsync(string title, string content)
        {
            var query = @"INSERT INTO Announcement (Title, Content, DatePosted) 
                         VALUES (@title, @content, @date)";

            var parameters = new Dictionary<string, object>
            {
                { "@title", title },
                { "@content", content },
                { "@date", DateTime.Now }
            };

            await ExecuteNonQueryAsync(query, parameters);
            return true;
        }
    }
}