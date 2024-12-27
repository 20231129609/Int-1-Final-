using Microsoft.AspNetCore.SignalR;

namespace deneme135.Hubs
{
    public class ExamHub : Hub
    {
        public async Task ExamUpdated(string message)
        {
            await Clients.All.SendAsync("ReceiveExamUpdate", message);
        }

        public async Task ExamDeleted(string message)
        {
            await Clients.All.SendAsync("ReceiveExamDelete", message);
        }

        public async Task ExamCreated(string message)
        {
            await Clients.All.SendAsync("ReceiveExamCreate", message);
        }
    }
} 