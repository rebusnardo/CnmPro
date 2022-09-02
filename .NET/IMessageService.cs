using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Messages;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IMessageService
    {
        void DeleteMessageById(int id);

        Paged<Message> GetSenderById(int id, int pageIndex, int pageSize);

        void UpdateMessage(MessageUpdate UpdateModel);

        int CreateMessage(MessageAddRequest AddModel);

        Paged<Message> GetByConversation(int senderId, int recipientId, int pageIndex, int pageSize);

        Paged<Message> GetReceiverById(int id, int pageIndex, int pageSize);
    }
}