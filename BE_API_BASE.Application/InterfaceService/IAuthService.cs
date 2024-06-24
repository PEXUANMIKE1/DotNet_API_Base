using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_API_BASE.Application.Payloads.RequestModels.UserRequests;
using BE_API_BASE.Application.Payloads.Response;
using BE_API_BASE.Application.Payloads.ResponseModels.DataUsers;

namespace BE_API_BASE.Application.InterfaceService
{
    public interface IAuthService
    {
        Task<ResponseObject<DataResponseUser>> Register(Request_Register request);
        Task<string> ConfirmRegisterAccount(string confirmCode);
        Task<ResponseObject<List<DataResponseUser>>> GetAllUser();
        Task<ResponseObject<DataResponseUser>> GetUserById(int id);
        Task<ResponseObject<DataResponseUser>> EditUser(int id, Request_User_Update request);
        Task<ResponseObject<DataResponseUser>> DeleteUserById(long id);
    }
}
