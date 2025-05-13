using Business.DTOs.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(UserRegisterDTO registerDTO);
        Task<IdentityResult> CreateAdmin(UserRegisterDTO registerDTO);
        Task<SignInResult> LoginAsync(UserLoginDTO userLoginDTO);
        Task LogOutAsync();
    }
}
