using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_API_BASE.Application.Payloads.ResponseModels.DataUsers
{
    public class DataResponseUser : DataResponseBase
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Avatar { get; set; }
        public string UserStatus { get; set; }
    }
}
