using System.Data;

namespace EVS.Services
{
    public class MessageService : DatabaseService
    {
        public MessageService(IConfiguration configuration) : base(configuration) { }

        public async Task<DataTable> GetUserMessagesAsync(int userId)
        {
            var query = @"SELECT MessageID, SenderID, ReceiverID, MessageContent, SentDate 
                         FROM Message 
                         WHERE SenderID = @userId OR ReceiverID = @userId 
                         ORDER BY SentDate DESC";

            var parameters = new Dictionary<string, object> { { "@userId", userId } };
            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<bool> SendMessageAsync(int senderId, int receiverId, string content)
        {
            var query = @"INSERT INTO Message (SenderID, ReceiverID, MessageContent, SentDate) 
                         VALUES (@senderId, @receiverId, @content, @date)";

            var parameters = new Dictionary<string, object>
            {
                { "@senderId", senderId },
                { "@receiverId", receiverId },
                { "@content", content },
                { "@date", DateTime.Now }
            };

            await ExecuteNonQueryAsync(query, parameters);
            return true;
        }
    }
}
