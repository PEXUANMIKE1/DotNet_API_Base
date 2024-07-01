using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_API_BASE.Application.Constants;
using BE_API_BASE.Application.InterfaceService;
using BE_API_BASE.Application.Payloads.RequestModels.UserRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace BE_API_BASE.Controllers
{
    [Route(Constant.DefaultValue.DEFAULT_CONTROLLER_ROUTER)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Private Members
        private readonly IAuthService _authService;
        #endregion

        #region Constructor
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region Authentication Endpoints
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Request_Register request)
        {
            return Ok(await _authService.Register(request));
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmRegisterAccount(string confirmCode)
        {
            return Ok(await _authService.ConfirmRegisterAccount(confirmCode));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Request_Login request)
        {
            return Ok(await _authService.Login(request));
        }

        [HttpPut("change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //xác thực bằng token và lấy ra dữ liệu trong token
        public async Task<IActionResult> ChangePassword([FromBody] Request_ChangePassword request)
        {
            long id = long.Parse(HttpContext.User.FindFirst("Id").Value); // lấy ra id trong token
            return Ok(await _authService.ChangePassword(id,request));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> FogotPassword([FromBody] string email)
        {
            return Ok(await _authService.ForgotPassword(email));
        }

        [HttpPut("confirm-create-new-password")]
        public async Task<IActionResult> ConfirmCreateNewPassword([FromBody] Request_CreateNewPassword request)
        {
            return Ok(await _authService.ConfirmCreateNewPassword(request));
        }

        [HttpPost("{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddRoleForUser([FromRoute] long userId, [FromBody] List<string> roles)
        {
            return Ok(await _authService.AddRoleForUser(userId, roles));
        }

        [HttpDelete("{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteRoleForUser([FromRoute] long userId, [FromBody] List<string> roles)
        {
            return Ok(await _authService.DeleteRoleForUser(userId, roles));
        }


        #endregion

        #region User Management Endpoints
        [HttpGet("users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await _authService.GetAllUser());
        }

        [HttpGet("user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserById([FromQuery] int id)
        {
            return Ok(await _authService.GetUserById(id));
        }

        [HttpPut("user/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Request_User_Update request)
        {
            return Ok(await _authService.EditUser(id, request));
        }

        [HttpDelete("user/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUserById(long id)
        {
            return Ok(await _authService.DeleteUserById(id));
        }
        #endregion
    }

}
