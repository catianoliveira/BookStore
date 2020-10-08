using BookStore.Data.Entities;
using BookStore.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public UserManager<User> UserManager { get; }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();


            await _userHelper.CheckRoleAsync("SuperAdmin");
            await _userHelper.CheckRoleAsync("Client");
            await _userHelper.CheckRoleAsync("Reseller");

            if (!_context.Categories.Any())
            {
                AddCategories("Romance");
                AddCategories("Poetry");
                AddCategories("Children's");
                AddCategories("Crime & Thriller");
                AddCategories("Fiction");
                AddCategories("Others");
                await _context.SaveChangesAsync();
            }


                if (!_context.Countries.Any())
            {
                CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
                List<RegionInfo> countriesList = new List<RegionInfo>();
                var countries = new List<Country>();
                foreach (CultureInfo ci in cultures)
                {
                    RegionInfo regionInfo = new RegionInfo(ci.Name);
                    if (countriesList.Count(x => x.EnglishName == regionInfo.EnglishName) <= 0)
                    {
                        countriesList.Add(regionInfo);
                    }
                }

                foreach (RegionInfo regionInfo in countriesList.OrderBy(x => x.EnglishName))
                {
                    var country = regionInfo.EnglishName;
                    AddCountries(country);

                    await _context.SaveChangesAsync();
                }

                AddCountries("null");
            }


            var user = await _userHelper.GetUserByEmailAsync("catia-96@hotmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Cátia",
                    LastName = "Oliveira",
                    Email = "catia-96@hotmail.com",
                    UserName = "catia-96@hotmail.com",
                    PhoneNumber = "123456",
                    Address = "Rua da Luz",
                    City = "Lisboa",
                    CountryId = 179
                };

                var result = await _userHelper.AddUserAsync(user, "123456");



                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder.");
                }


                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);


                var isInRole = await _userHelper.IsUserInRoleAsync(user, "SuperAdmin");
                if (!isInRole)
                {
                    await _userHelper.AddUsertoRoleAsync(user, "SuperAdmin");
                }
            }
        }

        private void AddCategories(string name)
        {
            _context.Categories.Add(new Category
            {
                Name = name
            });
        }


        private void AddCountries(string name)
        {
            _context.Countries.Add(new Country
            {
                Name = name
            });
        }
    }
}
