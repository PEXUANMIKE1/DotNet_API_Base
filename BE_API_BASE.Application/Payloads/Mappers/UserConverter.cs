using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_API_BASE.Application.Payloads.ResponseModels.DataUsers;
using BE_API_BASE.Doman.Entities;

namespace BE_API_BASE.Application.Payloads.Mappers
{
    public class UserConverter
    {
        public DataResponseUser EntityDTO(User user)
        {
            return new DataResponseUser
            {
                Id = user.Id,
                Avatar = user.Avatar,
                CreateTime = user.CreateTime,
                DateOfBirth = user.DateOfBirth,
                UpdateTime = user.UpdateTime,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                UserStatus = user.UserStatus.ToString(),
            };
        }
    }
}
