using System.Collections.Generic;
using System.Threading.Tasks;
using HomeTest.Models;

namespace HomeTest.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
    }
}