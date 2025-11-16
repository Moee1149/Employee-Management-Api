using MyWebApiApp.Dto;
using MyWebApiApp.Models;

namespace MyWebApiApp.Iservice;

public interface IAuthService
{
    public Task<User?> RegisterUser(UserDto request);
    public Task<string?> LoginUser(UserDto request);
}