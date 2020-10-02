using BookStore.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        IEnumerable<SelectListItem> GetComboCountries();
    }
}
