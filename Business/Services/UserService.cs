using AutoMapper;
using Business.DTOs.Account;
using Business.IServices;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;

        }

        public async Task<IdentityResult> RegisterAsync(UserRegisterDTO registerDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User already exists with this email." });
            }

            if (registerDTO.Password != registerDTO.ConfirmPassword)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Passwords do not match." });
            }

            var user = _mapper.Map<AppUser>(registerDTO);
            user.EmailConfirmed = false;
            user.Role = "User";

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                return result;
            }

            var roleExists = await _roleManager.RoleExistsAsync("User");
            if (!roleExists)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role 'User' does not exist." });
            }

            await _userManager.AddToRoleAsync(user, "User");
            return result;
        }

        public async Task<SignInResult> LoginAsync(UserLoginDTO userLoginDTO)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDTO.Email);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            var result = await _signInManager.PasswordSignInAsync(userLoginDTO.Email, userLoginDTO.Password, userLoginDTO.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return SignInResult.Failed;
            }
            return result;
        }

        public async Task<IdentityResult> CreateAdmin(UserRegisterDTO registerDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Admin already exists with this email." });
            }

            if (registerDTO.Password != registerDTO.ConfirmPassword)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Passwords do not match." });
            }

            var admin = _mapper.Map<AppUser>(registerDTO);
            admin.EmailConfirmed = false;
            admin.Role = "Admin";

            var result = await _userManager.CreateAsync(admin, registerDTO.Password);
            if (!result.Succeeded)
            {
                return result;
            }

            var roleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role 'Admin' does not exist." });
            }

            await _userManager.AddToRoleAsync(admin, "Admin");
            return result;
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
