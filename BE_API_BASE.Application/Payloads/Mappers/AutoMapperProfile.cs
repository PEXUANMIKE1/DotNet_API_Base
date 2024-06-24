using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_API_BASE.Application.Payloads.RequestModels.UserRequests;
using BE_API_BASE.Application.Payloads.ResponseModels.DataUsers;
using BE_API_BASE.Doman.Entities;

namespace BE_API_BASE.Application.Payloads.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Request_Register, User>();
            CreateMap<User, DataResponseUser>();
        }
    }
}
