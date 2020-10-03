using BookStore.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboCategories()
        {
            var list = _context.Categories.OrderBy(p => p.Name).Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()

            }).ToList();


            list.Insert(0, new SelectListItem
            {
                Text = "Select a category",
                Value = "0"
            });

            return list;
        }
    }
}
