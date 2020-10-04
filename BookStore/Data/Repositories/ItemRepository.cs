using BookStore.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        private readonly DataContext _context;

        public ItemRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Items.Include(p => p.User);
        }

        public IEnumerable<SelectListItem> GetComboItems()
        {
            var list = _context.Items.Select(p => new SelectListItem
            {
                Text = p.Title,
                Value = p.Id.ToString()
            }).ToList();


            list.Insert(0, new SelectListItem
            {
                Text = "(Select a product...)",
                Value = "0"
            });

            return list;
        }

        public IQueryable GetAllInCategroy(int categoryId)
        {
            return _context.Categories
                .Where(c => c.Id == categoryId);
        }
    }
}