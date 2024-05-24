using EBook.Models;

namespace EBook.Services.Iface
{
    public interface ILoginService
    {
        Task<string> SignupAsync(UserDTO loginService);
        Task<string> RoleAsync(UserDTO loginService);
        Task<string>  GenerateToken(UserDTO login, string role);

    }
}
