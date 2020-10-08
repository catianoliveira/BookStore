using BookStore.Data.Entities;
using BookStore.Data.Repositories;
using BookStore.Helpers;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOrderRepository _orderRepository;

        public ItemsController(
            IItemRepository itemRepository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper,
            ICategoryRepository categoryRepository,
            IOrderRepository orderRepository)
        {

            _itemRepository = itemRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
        }

        [Authorize(Roles = "SuperAdmin")]
        // GET: Products
        public IActionResult Index()
        {
            return View(_itemRepository.GetAll().OrderBy(p => p.Title));
        }


        public IActionResult ChooseItem(int id)
        {
            var model = new AddItemViewModel
            {
                Quantity = 1,
                ItemId = id,
                Items = _itemRepository.GetComboItems()
            };

            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChooseItem(AddItemViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

                else
                {
                    try
                    {
                        await _orderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);
                        return this.RedirectToAction("InStock");
                    }

                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }
            return this.View(model);
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View(); 
            }

            var product = await _itemRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return View(); 
            }

            return View(product);
        }


        // GET: Products/Create
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            else
            {
                try
                {
                    var model = new ItemViewModel
                    {
                        Categories = _categoryRepository.GetComboCategories()
                    };

                    return View(model);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View();
        }


        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Items");
                }

                var product = _converterHelper.ToItem(model, path, true);

                product.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                await _itemRepository.CreateAsync(product);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "Id can't be null");
            }

            var product = await _itemRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                ModelState.AddModelError(string.Empty, "Id can't be null");
            }

            var view = _converterHelper.ToProductViewModel(product);

            return View(view);
        }



        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Items");
                    }

                    var product = _converterHelper.ToItem(model, path, false);

                    product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _itemRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!await _itemRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airport = await _itemRepository.GetByIdAsync(id.Value);

            if (airport == null)
            {
                return NotFound();
            }

            try
            {
                await _itemRepository.DeleteAsync(airport);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

       


        public IActionResult ProductNotFound()
        {
            return View();
        }


        public IActionResult InStock()
        {
            return View(_itemRepository.GetInStock());
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult NotInStock()
        {
            return View(_itemRepository.GetNotInStock());
        }


    }
}
