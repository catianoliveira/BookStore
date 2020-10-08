using BookStore.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Data.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        IEnumerable<SelectListItem> GetComboCategories();

        IQueryable GetAllInCategory(int categoryId);

    }
}