using BookStore.Data.Entities;
using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Helpers
{
    public interface IUserHelper
    {
        Task AddUserToRoleAsync(User user, string roleName);



        Task<User> GetUserByEmailAsync(string email);


        Task<IdentityResult> AddUserAsync(User user, string password);


        Task<SignInResult> LoginAsync(LoginViewModel model);


        Task LogoutAsync();


        Task<IdentityResult> UpdateUserAsync(User user);


        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);


        Task<SignInResult> ValidatePasswordAsync(User user, string password);


        Task CheckRoleAsync(string roleName);


        Task<bool> IsUserInRoleAsync(User user, string roleName);


        Task AddUsertoRoleAsync(User user, string roleName);


        Task<string> GenerateEmailConfirmationTokenAsync(User user);


        Task<IdentityResult> ConfirmEmailAsync(User user, string token);


        Task<User> GetUserByIdAsync(string userId);



        Task<string> GeneratePasswordResetTokenAsync(User user);


        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
    }
}