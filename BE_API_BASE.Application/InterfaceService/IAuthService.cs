using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_API_BASE.Application.Payloads.RequestModels.UserRequests;
using BE_API_BASE.Application.Payloads.Response;
using BE_API_BASE.Application.Payloads.ResponseModels.DataUsers;
using BE_API_BASE.Doman.Entities;

namespace BE_API_BASE.Application.InterfaceService
{
    public interface IAuthService
    {
        Task<ResponseObject<DataResponseUser>> Register(Request_Register request); //đăng ký tài khoản
        Task<string> ConfirmRegisterAccount(string confirmCode);// nhập mã confirm code để kích hoạt tài khoản

        Task<ResponseObject<DataResponseLogin>> GetJwtTokenAsync(User user);//lấy ra token của người dùng

        Task<ResponseObject<DataResponseLogin>> Login(Request_Login request); //Login


        Task<ResponseObject<List<DataResponseUser>>> GetAllUser();
        Task<ResponseObject<DataResponseUser>> GetUserById(int id);
        Task<ResponseObject<DataResponseUser>> EditUser(int id, Request_User_Update request);
        Task<ResponseObject<DataResponseUser>> DeleteUserById(long id);
    }
}
