using BookStore.Data.Entities;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Helpers
{
    public interface IConverterHelper
    {
        Item ToItem(ItemViewModel model, string path, bool isNew);
        ItemViewModel ToProductViewModel(Item model);
    }
}
