using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_API_BASE.Application.Constants;
using BE_API_BASE.Application.InterfaceService;
using BE_API_BASE.Application.Payloads.RequestModels.UserRequests;

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
        #endregion

        #region User Management Endpoints
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await _authService.GetAllUser());
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserById([FromQuery] int id)
        {
            return Ok(await _authService.GetUserById(id));
        }

        [HttpPut("user/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Request_User_Update request)
        {
            return Ok(await _authService.EditUser(id, request));
        }

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUserById(long id)
        {
            return Ok(await _authService.DeleteUserById(id));
        }
        #endregion
    }

}
