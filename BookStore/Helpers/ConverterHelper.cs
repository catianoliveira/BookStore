using BookStore.Data.Entities;
using BookStore.Models;
using System;

namespace BookStore.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Item ToItem(ItemViewModel model, string path, bool isNew)
        {
            return new Item
            {
                Id = isNew ? 0 : model.Id,
                ImageUrl = path,
                IsAvaiable = model.IsAvaiable,
                Title = model.Title,
                Author = model.Author,
                ISBN = model.ISBN,
                Price = model.Price,
                Stock = model.Stock,
                User = model.User
            };
        }

        public ItemViewModel ToProductViewModel(Item model)
        {
            return new ItemViewModel
            {
                Id = model.Id,
                ImageUrl = model.ImageUrl,
                IsAvaiable = model.IsAvaiable,
                Title = model.Title,
                Author = model.Author,
                ISBN = model.ISBN,
                Price = model.Price,
                Stock = model.Stock,
                User = model.User,
                AddedDate = DateTime.Today
            };
        }
    }
}
