using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Data.Entities;
using BookStore.Data.Repositories;
using BookStore.Helpers;
using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly ICountryRepository _countryRepository;

        public AdminController(
            IUserHelper userHelper,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            DataContext context,
            SignInManager<User> signInManager,
            ICountryRepository countryRepository)
        {
            _userHelper = userHelper;
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _countryRepository = countryRepository;
        }



        public IActionResult CreateRole()
        {
            return View();
        }


        /// <summary>
        /// creates role if it doesn't exist already
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userHelper.CheckRoleAsync(model.Role);
                    ModelState.AddModelError(string.Empty, "Role created with success");
                    return RedirectToAction(nameof(ListUsers));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There's a role with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

                return View(model);
            }

            this.ModelState.AddModelError(string.Empty, "Role already exists");

            return View(model);
        }



        /// <summary>
        /// lists users and their roles
        /// </summary>
        /// <returns></returns>
        //TODO [Authorize(Roles = "SuperAdmin, Admin, Employee")]
        public async Task<ActionResult> ListUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRoleViewModel>();

            foreach (User user in users)
            {
                var thisViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    Name = user.FullName,
                    UserName = user.Email,
                    Roles = await GetUserRoles(user)
                };
                userRolesViewModel.Add(thisViewModel);
            }

            return View(userRolesViewModel);
        }




        private async Task<List<string>> GetUserRoles(User user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }


        /// <summary>
        /// gets users details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IActionResult> UserDetails(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }


            var model = new UserRoleViewModel
            {
                Name = user.FullName,
                UserName = user.UserName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                UserId = user.Id,
            };

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userHelper.GetUserByIdAsync(id);

            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "User not found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                City = user.City,
                PhoneNumber = user.PhoneNumber,
                Countries = _countryRepository.GetComboCountries(),
                CountryId = user.CountryId,
                Roles = _roleManager.Roles.ToList().Select(
                    x => new SelectListItem()
                    {
                        Selected = userRoles.Contains(x.Name),
                        Text = x.Name,
                        Value = x.Id
                    })
            };

            return View(model);
        }



        /// <summary>
        /// edits users information and role
        /// </summary>
        /// <param name="editUser"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel editUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (User.IsInRole("Employee"))
                    {
                        ModelState.AddModelError(string.Empty, "Employees cannot edit users");
                    }
                    var user = await _userHelper.GetUserByIdAsync(editUser.Id);

                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "User not found");
                    }

                    user.FirstName = editUser.FirstName;
                    user.LastName = editUser.LastName;
                    user.Address = editUser.Address;
                    user.PhoneNumber = editUser.PhoneNumber;
                    user.CountryId = editUser.CountryId;
                    user.City = editUser.City;

                    var selectedRole = await _roleManager.FindByIdAsync(editUser.SelectedRole);

                    foreach (var currentRole in _roleManager.Roles.ToList())
                    {
                        var isSelectedRole = selectedRole.Name.Equals(currentRole.Name);
                        if (!isSelectedRole && await _userHelper.IsUserInRoleAsync(user, currentRole.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(user, currentRole.Name);
                        }
                    }

                    await _userHelper.AddUserToRoleAsync(user, selectedRole.Name);

                    var result = await _userHelper.UpdateUserAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListUsers");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return RedirectToAction(nameof(ListUsers));
        }



        public async Task<IActionResult> Delete(string id)
        {
            if (User.IsInRole("Employee"))
            {
                ModelState.AddModelError(string.Empty, "Employees cannot delete users");
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                await _userManager.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction(nameof(ListUsers));
        }
    }
}
