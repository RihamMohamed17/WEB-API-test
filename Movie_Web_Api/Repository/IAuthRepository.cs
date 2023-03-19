using System.Threading.Tasks;
using Movie_Web_Api.Models;

namespace Movie_Web_Api.Repository
{
    public interface IAuthRepository
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(LoginModel model);
        Task<string> AddRoleAsync(AddRoleModel model);
    }
}