using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_API_BASE.Doman.Entities;

namespace BE_API_BASE.Doman.InterfaceRepositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUserName(string userName);
        Task<User> GetUserByPhoneNumber(string phoneNumber);
        Task AddRoleToUserAsync(User user, List<string> listRoles);
        Task<IEnumerable<string>> GetRolesOfUserAsync(User user);
        Task DeleteRolesAsync(User user, List<string> roles);
    }
}
