using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_API_BASE.Doman.enumerates;

namespace BE_API_BASE.Doman.Entities
{
    public class User:BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName {  get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreateTime { get; set; }
        [MaybeNull]
        public DateTime UpdateTime { get; set; }
        public string Avatar {  get; set; }
        public string Address { get; set; }
        public virtual ICollection<Permission>? Permissions { get; set; }
        public constantEnums.UserStatusEnum UserStatus { get; set; } = constantEnums.UserStatusEnum.UnActivated;
    }
}
