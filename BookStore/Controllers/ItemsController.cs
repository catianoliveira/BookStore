using BookStore.Data.Repositories;
using BookStore.Helpers;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public ItemsController(
            IItemRepository itemRepository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {

            _itemRepository = itemRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }


        // GET: Products
        public IActionResult Index()
        {
            return View(_itemRepository.GetAll().OrderBy(p => p.Title));
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View(); //TODO Not Found
            }

            var product = await _itemRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return View(); //TODO Not Found
            }

            return View(product);
        }


        // GET: Products/Create
        //TODO [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
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
                    path = await _imageHelper.UploadImageAsyc(model.ImageFile, "Items");
                }

                var product = _converterHelper.ToItem(model, path, true);

                product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _itemRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Products/Edit/5
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View(); //TODO Not Found
            }

            var product = await _itemRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return View(); //TODO Not Found
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
                        path = await _imageHelper.UploadImageAsyc(model.ImageFile, "Items");
                    }

                    var product = _converterHelper.ToItem(model, path, false);

                    //TODO: Change to the logged user
                    product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _itemRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _itemRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Products/Delete/5
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View(); //TODO Not Found
            }

            var product = await _itemRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return View(); //TODO Not Found
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _itemRepository.GetByIdAsync(id);
            await _itemRepository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult ProductNotFound()
        {
            return View();
        }
    }
}
