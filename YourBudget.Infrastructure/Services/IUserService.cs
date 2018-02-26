using System.Threading.Tasks;
using YourBudget.Infrastructure.DTO;

namespace YourBudget.Infrastructure.Services
{
    public interface IUserService
    {
         Task RegisterAsync(string email, string username, string password);

         Task<UserDto> GetAsync(string email);
    }
}