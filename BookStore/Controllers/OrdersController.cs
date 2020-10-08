using BookStore.Data.Repositories;
using BookStore.Helpers;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMailHelper _mailHelper;

        public OrdersController(
            IOrderRepository orderRepository,
            IMailHelper mailHelper)
        {
            _orderRepository = orderRepository;
            _mailHelper = mailHelper;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (User.IsInRole("SuperAdmin"))
            {
                var model = _orderRepository.GetAll();
                return View(model);
            }

            else
            {
                var model = await _orderRepository.GetOrdersAsync(this.User.Identity.Name);
                return View(model);
            }
        }


        public async Task<IActionResult> Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var model = await _orderRepository.GetDetailTempsAsync(User.Identity.Name);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }



        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _orderRepository.DeleteDetailTempAsync(id.Value);
            return this.RedirectToAction("Create");
        }



        public async Task<IActionResult> Increase(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, 1);
            return this.RedirectToAction("Create");
        }


        public async Task<IActionResult> Decrease(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, -1);
            return this.RedirectToAction("Create");
        }


        public async Task<IActionResult> ConfirmOrder()
        {
            var response = await _orderRepository.ConfirmOrderAsync(this.User.Identity.Name);
            if (response != null)
            {
                try
                {
                    PdfGenerator generator = new PdfGenerator();

                    var pdf = generator.CreatePdf(response, User.Identity.Name);

                    _mailHelper.SendMailWithPDF(User.Identity.Name, "Order Confirmation", 
                        "You can find your order's details in the attachment.", pdf);

                    return View();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return this.RedirectToAction("Index");
        }


        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Deliver(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.GetOrderAsync(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            var model = new DeliverViewModel
            {
                Id = order.Id,
                DeliveryDate = DateTime.Today
            };

            return View(model);

        }


        [HttpPost]
        public async Task<IActionResult> Deliver(DeliverViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _orderRepository.DeliverOrderAsync(model);
                    return this.RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View();
        }
    }
}