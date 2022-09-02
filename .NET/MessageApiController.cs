using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Messages;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessageApiController : BaseApiController
    {
        private IAuthenticationService<int> _authService = null;
        private IMessageService _service = null;
        public MessageApiController(IMessageService service, IAuthenticationService<int> authService, ILogger<IMessageService> logger) : base(logger)
        {
            _service = service;
            _authService = authService;

        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.DeleteMessageById(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("sender/{id:int}")]
        public ActionResult<ItemsResponse<Paged<Message>>> GetSenderById(int id, int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                Paged<Message> pagedMessages = _service.GetSenderById(id, pageIndex, pageSize);

                if (pagedMessages == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Message>> { Item = pagedMessages };
                    
                }   

            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse("$Generic Error: ${ex.Message}");
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("conversation/{id:int}")]
        public ActionResult<ItemsResponse<Paged<Message>>> GetByConversation(/*int senderId,*/ int id, int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                Paged<Message> pagedMessages = _service.GetByConversation(userId, id, pageIndex, pageSize);

                if (pagedMessages == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Message>> { Item = pagedMessages };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse("$Generic Error: ${ex.Message}");
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("receiver/{id:int}")]
        public ActionResult<ItemsResponse<Paged<Message>>> GetReceiverById(int id, int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                Paged<Message> pagedMessages = _service.GetReceiverById(id, pageIndex,pageSize);

                if (pagedMessages == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Message>> { Item = pagedMessages };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse("$Generic Error: ${ex.Message}");
            }
            return StatusCode(iCode, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(MessageUpdate model)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                
                _service.UpdateMessage(model);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(iCode, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> MessageAdd(MessageAddRequest model)
        {
            ObjectResult result = null;

            try
            {
            
                int id = _service.CreateMessage(model);

            
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

              
                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }
    }
}
