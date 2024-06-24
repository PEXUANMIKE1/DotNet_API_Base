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
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Request_Register request)
        {
            return Ok(await _authService.Register(request));
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmRegisterAccount(string confirmCode)
        {
            return Ok(await _authService.ConfirmRegisterAccount(confirmCode));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await _authService.GetAllUser());
        }
        [HttpGet]
        public async Task<IActionResult> GetUserById([FromQuery] int id)
        {
            return Ok(await _authService.GetUserById(id));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id,[FromBody] Request_User_Update request)
        {
            return Ok(await _authService.EditUser(id,request));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUserById(long id)
        {
            return Ok(await _authService.DeleteUserById(id));
        }
    }
}
