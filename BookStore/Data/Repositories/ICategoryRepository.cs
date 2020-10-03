using BookStore.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookStore.Data.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        IEnumerable<SelectListItem> GetComboCategories();
    }
}