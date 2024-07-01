using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_API_BASE.Application.Handle.HandleEmail;
using BE_API_BASE.Application.InterfaceService;
using BE_API_BASE.Application.Payloads.Mappers;
using BE_API_BASE.Application.Payloads.RequestModels.UserRequests;
using BE_API_BASE.Application.Payloads.Response;
using BE_API_BASE.Application.Payloads.ResponseModels.DataUsers;
using BE_API_BASE.Doman.Entities;
using BE_API_BASE.Doman.InterfaceRepositories;
using BE_API_BASE.Doman.Validations;
using BCryptNet = BCrypt.Net.BCrypt;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using static BE_API_BASE.Doman.enumerates.constantEnums;
using Org.BouncyCastle.Asn1.Ocsp;

namespace BE_API_BASE.Application.ImplementService
{   

    public class AuthService : IAuthService
    {
        #region Private Members
        private readonly IBaseRepository<User> _baseUserRepository;
        private readonly UserConverter _userConverter;
        private readonly IConfiguration _configuration; 
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IBaseRepository<ConfirmEmail> _baseConfirmEmailRepository;
        private readonly IBaseRepository<Permission> _basePermissionRepository;
        private readonly IBaseRepository<Role> _baseRoleRepository;
        private readonly IBaseRepository<RefeshToken> _baseRefeshTokenRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        #endregion

        #region Constructor
        public AuthService(IBaseRepository<User> baseRepository, UserConverter userConverter,
            IConfiguration configuration, IUserRepository userRepository, IBaseRepository<Permission> basePermissionRepository,
            IEmailService emailService, IBaseRepository<ConfirmEmail> baseConfirmEmailRepository, IBaseRepository<Role> baseRoleRepository ,
            IBaseRepository<RefeshToken> baseRefeshTokenRepository, IHttpContextAccessor contextAccessor)
        {
            _baseUserRepository = baseRepository;
            _userConverter = userConverter;
            _configuration = configuration;
            _userRepository = userRepository;
            _emailService = emailService;
            _baseConfirmEmailRepository = baseConfirmEmailRepository;
            _basePermissionRepository = basePermissionRepository;
            _baseRoleRepository = baseRoleRepository;
            _baseRefeshTokenRepository = baseRefeshTokenRepository;
            _contextAccessor = contextAccessor;
        }
        #endregion

        #region Public Methods
        public async Task<ResponseObject<DataResponseUser>> DeleteUserById(long id)
        {
            try
            {
                var user = await _baseUserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Người dùng này không tồn tại!",
                        Data = null
                    };
                }
                _baseUserRepository.DeleteAsync(id);
                return new ResponseObject<DataResponseUser>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Xóa người dùng thành công!",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseObject<DataResponseUser>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Error" + ex.StackTrace,
                    Data = null
                };
            }

        }
        public async Task<ResponseObject<DataResponseUser>> EditUser(int id, Request_User_Update request)
        {
            try
            {
                var user = await _baseUserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Người dùng này không tồn tại!",
                        Data = null
                    };
                }
                if (!ValidateInput.IsValidEmail(request.Email))
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Định dạng Email không hợp lệ",
                        Data = null
                    };
                }
                if (!ValidateInput.IsValidPhoneNumber(request.PhoneNumber))
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Định dạng SĐT không hợp lệ",
                        Data = null
                    };
                }
                if (!Equals(request.Email,user.Email))
                {
                    if (await _userRepository.GetUserByEmail(request.Email) != null)
                    {
                        return new ResponseObject<DataResponseUser>
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Message = "Email đã tồn tại trong hệ thống! Vui lòng sử dụng Email khác",
                            Data = null
                        };
                    }
                }
                if (!Equals(request.PhoneNumber, user.PhoneNumber))
                {
                    if (await _userRepository.GetUserByPhoneNumber(request.PhoneNumber) != null)
                    {
                        return new ResponseObject<DataResponseUser>
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Message = "Số diện thoại đã tồn tại trong hệ thống! Vui lòng sử dụng SDT khác",
                            Data = null
                        };
                    }
                }
                user.Email = request.Email;
                user.DateOfBirth = request.DateOfBirth;
                user.PhoneNumber = request.PhoneNumber;
                user.FullName = request.FullName;
                user.Address = request.Address;
                user.UpdateTime = DateTime.UtcNow;

                _baseUserRepository.UpdateAsync(user);
                return new ResponseObject<DataResponseUser>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Cập nhật user thành công",
                    Data = _userConverter.EntityDTO(user)
                };
            }
            catch (Exception ex)
            {
                return new ResponseObject<DataResponseUser>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Error" + ex.StackTrace,
                    Data = null
                };
            }
        }
        public async Task<ResponseObject<List<DataResponseUser>>> GetAllUser()
        {
            try
            {
                // Lấy danh sách tất cả các User
                var listUser = await _baseUserRepository.GetAllAsync();

                // Chuyển đổi danh sách User sang DataResponseUser
                var responseList = listUser.Select(user => _userConverter.EntityDTO(user)).ToList();

                // Trả về kết quả
                //return responseList;
                return new ResponseObject<List<DataResponseUser>>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Get All User Success!",
                    Data = responseList 
                };
            }
            catch (Exception ex)
            {
                return new ResponseObject<List<DataResponseUser>>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Error" + ex.StackTrace,
                    Data = null
                };
            }
        }
        public async Task<ResponseObject<DataResponseUser>> GetUserById(int id)
        {
            try
            {
                var user = await _baseUserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Người dùng này không tồn tại!",
                        Data = null
                    };
                }
                var userRes = _userConverter.EntityDTO(user);
                return new ResponseObject<DataResponseUser>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Thực hiện thao tác thành công!",
                    Data = userRes
                };
            }
            catch(Exception ex)
            {
                return new ResponseObject<DataResponseUser>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Error" + ex.StackTrace,
                    Data = null
                };
            } 

        }
        public async Task<ResponseObject<DataResponseUser>> Register(Request_Register request)
        {
            try
            {
                if (!ValidateInput.IsValidEmail(request.Email))
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Định dạng Email không hợp lệ",
                        Data = null
                    };
                }
                if (!ValidateInput.IsValidPhoneNumber(request.PhoneNumber))
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Định dạng số điện thoại không hợp lệ",
                        Data = null
                    };
                }
                if(await _userRepository.GetUserByEmail(request.Email) != null)
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Email đã tồn tại trong hệ thống! Vui lòng sử dụng Email khác",
                        Data = null
                    };
                }
                if (await _userRepository.GetUserByPhoneNumber(request.PhoneNumber) != null)
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Số diện thoại đã tồn tại trong hệ thống! Vui lòng sử dụng SDT khác",
                        Data = null
                    };
                }
                if (await _userRepository.GetUserByUserName(request.UserName) != null)
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Tên tài khoản đã tồn tại trong hệ thống! Vui lòng sử dụng tên tài khoản khác khác",
                        Data = null
                    };
                }
                var User = new User
                {
                    Avatar = "https://st.quantrimang.com/photos/image/2021/05/21/AVT-Doi17-2.jpg",
                    IsActive = true,
                    CreateTime = DateTime.Now,
                    DateOfBirth = request.DateOfBirth,
                    Email = request.Email,
                    FullName = request.FullName,
                    Password = BCryptNet.HashPassword(request.Password),
                    PhoneNumber = request.PhoneNumber,
                    UserName = request.UserName,
                    Address = request.Address,
                    UserStatus = Doman.enumerates.constantEnums.UserStatusEnum.UnActivated,
                };
                User = await _baseUserRepository.CreateAsync(User);
                await _userRepository.AddRoleToUserAsync(User, new List<string> { "User" });
                //tạo đối tượng bảng confirmEmail để lưu vào database
                ConfirmEmail confirmEmail = new ConfirmEmail
                {
                    IsActive = true,
                    ConfirmCode = GenergateCodeActive(),
                    ExpiryTime = DateTime.Now.AddMinutes(5),
                    IsConfirmed = false,
                    UserId = User.Id,
                };
                //lưu vào databae
                confirmEmail = await _baseConfirmEmailRepository.CreateAsync(confirmEmail);
                var message = new EmailMessage(new string[] { request.Email }, "Nhận mã xác nhận tài khoản ", $"Mã xác nhận (tồn tại 5 phút): {confirmEmail.ConfirmCode}");
                //gửi mã xác nhận về email
                var responseMessage = _emailService.SendEmail(message);

                return new ResponseObject<DataResponseUser>
                {
                    Status = StatusCodes.Status201Created,
                    Message = "Mã xác nhận đã được gửi!. Vui lòng kiểm tra email",
                    Data = _userConverter.EntityDTO(User)
                };
            }
            catch (Exception ex)
            {
                return new ResponseObject<DataResponseUser>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Error" + ex.StackTrace,
                    Data = null
                };
            }
        }
        public async Task<string> ConfirmRegisterAccount(string confirmCode)
        {
            try
            {
                var code = await _baseConfirmEmailRepository.GetAsync(x=>x.ConfirmCode.Equals(confirmCode));
                if(code == null)
                {
                    return "Mã xác nhận không hợp lệ";
                }
                var user = await _baseUserRepository.GetAsync(x => x.Id == code.UserId);
                if(code.ExpiryTime <  DateTime.Now)
                {
                    return "Mã xác nhận đã hết hạn";
                }
                user.UserStatus = Doman.enumerates.constantEnums.UserStatusEnum.Activated;
                code.IsConfirmed = true;
                await _baseUserRepository.UpdateAsync(user);
                await _baseConfirmEmailRepository.UpdateAsync(code);
                return "Xác nhận đăng ký tài khoản thành công. Bạn có thể đăng nhập vào hệ thống";
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }
        public async Task<ResponseObject<DataResponseLogin>> GetJwtTokenAsync(User user)
        {
            try
            {
                var permissions = await _basePermissionRepository.GetAllAsync(x=>x.UserId == user.Id);
                var roles = await _baseRoleRepository.GetAllAsync();

                var authClaims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("Email", user.Email),
                    new Claim("PhoneNumber", user.PhoneNumber),
                    new Claim("Address", user.Address)
                };
                foreach (var permission in permissions)
                {
                    foreach (var role in roles)
                    {
                        if(role.Id == permission.RoleId)
                        {
                            authClaims.Add(new Claim("Permission", role.RoleName));
                        }
                    }
                }
                var userRole = await _userRepository.GetRolesOfUserAsync(user);
                foreach(var item in userRole)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, item));
                }
                var jwtToken = GetToken(authClaims);
                var refreshToken = GenerateRefreshToken();
                _ = int.TryParse(_configuration["JWT:RefreshTokenValidity"], out int refreshTokenValidity);

                RefeshToken rf = new RefeshToken
                {
                    IsActive = true,
                    ExpiryTime = DateTime.UtcNow.AddHours(refreshTokenValidity),
                    UserId = user.Id,
                    Token = refreshToken
                };
                rf = await _baseRefeshTokenRepository.CreateAsync(rf);
                return new ResponseObject<DataResponseLogin>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Tạo token thành công",
                    Data = new DataResponseLogin
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        RefeshToken = refreshToken
                    }
                };

            }
            catch (Exception ex)
            {
                return new ResponseObject<DataResponseLogin>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Error: "+ ex.StackTrace,
                    Data = null
                };
            }
        }
        public async Task<ResponseObject<DataResponseLogin>> Login(Request_Login request)
        {
            try
            {
                var userLog = await _baseUserRepository.GetAsync(x => x.UserName.Equals(request.UserName));
                if (userLog == null)
                {
                    return new ResponseObject<DataResponseLogin>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Tài khoản không chính xác!",
                        Data = null
                    };
                }
                if (userLog.UserStatus.ToString().Equals(UserStatusEnum.UnActivated.ToString()))
                {
                    return new ResponseObject<DataResponseLogin>
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Message = "Tài khoản chưa được xác thực!",
                        Data = null
                    };
                }
                bool checkPass = BCryptNet.Verify(request.Password, userLog.Password);
                if (!checkPass)
                {
                    return new ResponseObject<DataResponseLogin>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Mật khẩu không chính xác!",
                        Data = null
                    };
                }

                return new ResponseObject<DataResponseLogin>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Đăng nhập thành công!",
                    Data = new DataResponseLogin
                    {
                        AccessToken = GetJwtTokenAsync(userLog).Result.Data.AccessToken,
                        RefeshToken = GetJwtTokenAsync(userLog).Result.Data.RefeshToken
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResponseObject<DataResponseLogin>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Đăng nhập thất bại!\nError: " + ex.StackTrace,
                    Data = null
                };
            }
        }
        public async Task<ResponseObject<DataResponseUser>> ChangePassword(long userId, Request_ChangePassword request)
        {
            try
            {
                var user = await _baseUserRepository.GetByIdAsync(userId);
                var checkPass = BCryptNet.Verify(request.OldPassword, user.Password);
                if (!checkPass)
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Mật khẩu không chính xác !",
                        Data = null
                    };
                }
                if(request.NewPassword.Equals(request.OldPassword)) 
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Mật khẩu mới trùng với mật khẩu cũ! Vui lòng thay đổi",
                        Data = null
                    };
                }
                if (!request.NewPassword.Equals(request.ConfirmPassword))
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Mật khẩu xác nhận không trùng khớp!",
                        Data = null
                    };
                }
                user.Password = BCryptNet.HashPassword(request.NewPassword);
                user.UpdateTime = DateTime.UtcNow;
                await _baseUserRepository.UpdateAsync(user);
                return new ResponseObject<DataResponseUser>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Đổi mật khẩu thành công!",
                    Data = _userConverter.EntityDTO(user)
                };
            }
            catch (Exception ex)
            {
                return new ResponseObject<DataResponseUser>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Có lỗi trong quá trình xử lý\n Erorr: "+ex.StackTrace,
                    Data = null
                };
            }
        }
        public async Task<string> ForgotPassword(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(email);
                if(user == null)
                {
                    return "Email không chính xác!";
                }
                /*var listConfirmCode = await _baseConfirmEmailRepository.GetAllAsync(x=>x.UserId == user.Id);
                if(listConfirmCode.ToList().Count() > 0)
                {
                    foreach (var item in listConfirmCode) 
                    {
                        await _baseConfirmEmailRepository.DeleteAsync(item.Id);
                    }
                }*/
                ConfirmEmail confirmEmail = new ConfirmEmail
                {
                    IsActive = true,
                    ConfirmCode = GenergateCodeActive(),
                    ExpiryTime = DateTime.UtcNow.AddHours(2),
                    UserId = user.Id,
                    IsConfirmed = false,
                };
                confirmEmail = await _baseConfirmEmailRepository.CreateAsync(confirmEmail);
                var message = new EmailMessage(new string[] { user.Email }, "Mã xác nhận quên mật khẩu!", $"Mã xác nhận quên mật khẩu của bạn(2p): {confirmEmail.ConfirmCode}");
                var send = _emailService.SendEmail(message);
                return "Đã gửi mã xác nhận quên mật khẩu! Vui lòng kiểm tra hòm thư của bạn";
            }
            catch (Exception ex)
            {
                return "Error: "+ex.StackTrace;
            }
        }
        public async Task<string> ConfirmCreateNewPassword(Request_CreateNewPassword request)
        {
            try
            {
                var confirm = await _baseConfirmEmailRepository.GetAsync(x=>x.ConfirmCode.Equals(request.ConfirmCode));
                if (confirm == null)
                {
                    return "Mã xác nhận không chính xác!";
                }
                if(confirm.ExpiryTime < DateTime.UtcNow)
                {
                    return "Mã xác nhận đã hết hạn!";
                }
                if(!request.NewPassword.Equals(request.ConfirmPassword)) 
                {
                    return "Mật khẩu xác nhận không trùng khớp";
                }
                var user = await _baseUserRepository.GetByIdAsync(u=>u.Id == confirm.UserId);
                user.Password = BCryptNet.HashPassword(request.NewPassword);
                user.UpdateTime = DateTime.UtcNow;
                await _baseUserRepository.UpdateAsync(user);
                return "Thay đổi mật khẩu thành công!";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.StackTrace;
            }
        }
        public async Task<string> AddRoleForUser(long userId, List<string> roles)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if(!currentUser.Identity.IsAuthenticated) 
                {
                    return "Người dùng chưa xác thực";
                }
                if (!currentUser.IsInRole("Admin"))
                {
                    return "Bạn không có quyền hạn để sử dụng chức năng này";
                }
                var user = await _baseUserRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return "Người dùng không tồn tại";
                }

                await _userRepository.AddRoleToUserAsync(user, roles);
                return "Thêm quyền cho người dùng thành công!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> DeleteRoleForUser(long userId, List<string> roles)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return "Người dùng chưa xác thực";
                }
                if (!currentUser.IsInRole("Admin"))
                {
                    return "Bạn không có quyền hạn để sử dụng chức năng này";
                }
                var user = await _baseUserRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return "Người dùng không tồn tại";
                }

                await _userRepository.DeleteRolesAsync(user, roles);
                return "Xóa quyền của người dùng thành công!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Private Methods
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInHours"], out int tokenValidityInHours);
            var expirationUTC = DateTime.UtcNow.AddHours(tokenValidityInHours);
            /*var LocalTimeZone = TimeZoneInfo.Local;
            var expirationTimeInLocalTimeZone = TimeZoneInfo.ConvertTimeToUtc(expirationUTC,LocalTimeZone);*/

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: expirationUTC,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
        private string GenergateCodeActive()
        {
            string str = "code_" + DateTime.Now.Ticks.ToString();
            return str;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        #endregion
    }
}
