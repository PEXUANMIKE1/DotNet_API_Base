using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_API_BASE.Doman.Entities;
using BE_API_BASE.Doman.InterfaceRepositories;
using BE_API_BASE.Infrastructure.DataContexts;

namespace BE_API_BASE.Infrastructure.ImplementRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        #region Xử lý chuỗi
        private Task<bool> CompareStringAsync(string str1, string str2) 
        {
            return Task.FromResult(string.Equals(str1.ToLowerInvariant(),str2.ToLowerInvariant()));
        }
        private async Task<bool> IsStringInListAsync(string inputString, List<string> listString)
        {
            if(inputString == null)
            {
                throw new ArgumentNullException(nameof(inputString));
            }
            if(listString == null)
            {
                throw new ArgumentNullException(nameof(listString));
            }
            foreach(var item in listString)
            {
                if(await CompareStringAsync(inputString, item))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        public async Task AddRoleToUserAsync(User user, List<string> listRoles)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if(listRoles == null)
            {
                throw new ArgumentNullException(nameof(listRoles));
            }
            foreach(var role in listRoles.Distinct())
            {
                var roleOfUser = await GetRolesOfUserAsync(user);
                if(await IsStringInListAsync(role, roleOfUser.ToList()))
                {
                    throw new ArgumentException("Người dùng đã có quyền này rồi");
                }
                else
                {
                    var roleItem = await _context.Roles.SingleOrDefaultAsync(x => x.RoleCode.Equals(role));
                    if(roleItem == null)
                    {
                        throw new ArgumentNullException("Không tồn tại quyền này!");
                    }
                    _context.Permissions.Add(new Permission
                    {
                        RoleId = roleItem.Id,
                        UserId = user.Id,
                    }); 
                }
            }
            _context.SaveChanges();
        }

        public async Task<IEnumerable<string>> GetRolesOfUserAsync(User user)
        {
            var roles = new List<string>();
            var listRoles = _context.Permissions.Where(x=>x.UserId == user.Id).AsQueryable();
            foreach(var item in listRoles.Distinct())
            {
                var role = _context.Roles.SingleOrDefault(x=>x.Id == item.RoleId);
                roles.Add(role.RoleCode);
            }
            return roles.AsEnumerable();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x=> x.Email.ToLower().Equals(email.ToLower()));
            return user;
        }

        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.PhoneNumber.ToLower().Equals(phoneNumber.ToLower()));
            return user;
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName.ToLower().Equals(userName.ToLower()));
            return user;
        }
    }
}
