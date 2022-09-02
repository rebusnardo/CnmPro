using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Messages;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class MessageService : IMessageService
    {
        public IDataProvider _data = null;
        private IAuthenticationService<int> _authService = null;

        public MessageService(IAuthenticationService<int> authService, IDataProvider data)
        {
            _data = data;
            _authService = authService;
        }

        public void DeleteMessageById(int id)
        {
            string procName = "[dbo].[Messages_DeleteById]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                }, returnParameters: null);
        }

        public Paged<Message> GetSenderById(int id, int pageIndex, int pageSize)
        {

            Paged<Message> pagedResult = null;

            List<Message> list = null;

            int totalCount = 0;

            string procName = "[dbo].[Messages_Select_BySenderId]";

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@SenderId", id);
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Message aMessage = MessageMapper(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (list == null)
                    {
                        list = new List<Message>();
                    }
                    list.Add(aMessage);

                });
            if (list != null)
            {
                pagedResult = new Paged<Message>(list, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public Paged<Message> GetReceiverById(int id, int pageIndex, int pageSize)
        {
            Paged<Message> pagedResult = null;

            List<Message> list = null;

            int totalCount = 0;

            string procName = "[dbo].[Messages_Select_ByRece]";

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@RecipientId", id);
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Message aMessage = MessageMapper(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (list == null)
                    {
                        list = new List<Message>();
                    }
                    list.Add(aMessage);

                });
            if (list != null)
            {
                pagedResult = new Paged<Message>(list, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public Paged<Message> GetByConversation(int senderId, int recipientId, int pageIndex, int pageSize)
        {
            Paged<Message> pagedResult = null;

            List<Message> list = null;

            int totalCount = 0;

            string procName = "[dbo].[Messages_Select_Conversation]";

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@SenderId", senderId);
                    parameterCollection.AddWithValue("@RecipientId", recipientId);
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);

                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Message aMessage = MessageMapper(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (list == null)
                    {
                        list = new List<Message>();
                    }
                    list.Add(aMessage);

                });
            if (list != null)
            {
                pagedResult = new Paged<Message>(list, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public void UpdateMessage(MessageUpdate UpdateModel)
        {

            string procName = "[dbo].[Messages_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {

                AddCommonParams(UpdateModel, col);
                col.AddWithValue("@Id", UpdateModel.Id);

            }, returnParameters: null);
        }

        public int CreateMessage(MessageAddRequest AddModel)
        {
            int id = 0;

            string procName = "[dbo].[Messages_Insert]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(AddModel, col);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCol)
            {
                object oId = returnCol["@Id"].Value;

                int.TryParse(oId.ToString(), out id);

            });

            return id;
        }

        private static void AddCommonParams(MessageAddRequest AddModel, SqlParameterCollection col)
        {
            col.AddWithValue("@Message", AddModel.MessageText);
            col.AddWithValue("@Subject", AddModel.Subject);
            col.AddWithValue("@RecipientId", AddModel.RecipientId);
            col.AddWithValue("@SenderId", AddModel.SenderId);
            col.AddWithValue("@DateSent", AddModel.DateSent);
            col.AddWithValue("@DateRead", AddModel.DateRead);
        }

        private static Message MessageMapper(IDataReader reader, ref int startingIndex)
        {

            Message aMessage = new Message();
            UserProfileBase profile = new UserProfileBase();

            aMessage.Id = reader.GetSafeInt32(startingIndex++);
            aMessage.MessageText = reader.GetSafeString(startingIndex++);
            aMessage.Subject = reader.GetSafeString(startingIndex++);
            aMessage.RecipientId = reader.GetSafeInt32(startingIndex++);
            aMessage.SenderId = reader.GetSafeInt32(startingIndex++);
            aMessage.DateSent = reader.GetSafeDateTime(startingIndex++);
            aMessage.DateRead = reader.GetSafeDateTime(startingIndex++);
            aMessage.DateModified = reader.GetSafeDateTime(startingIndex++);
            aMessage.DateCreated = reader.GetSafeDateTime(startingIndex++);    
            aMessage.RecipientData = reader.DeserializeObject<List<MessageUserBase>>(startingIndex++);
            aMessage.SenderData = reader.DeserializeObject<List<MessageUserBase>>(startingIndex++); 

            return aMessage;
        }
    }
}
