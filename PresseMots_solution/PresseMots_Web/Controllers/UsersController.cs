using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using PresseMots_DataAccess.Context;
using PresseMots_DataModels.Entities;
using PresseMots_Web.Models;
using PresseMots_Web.Services;

namespace PresseMots_Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _service;
        private readonly IStringLocalizer<UsersController> _localizer;

        public UsersController(IUserService service, IStringLocalizer<UsersController> localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int id)
        {      
            return View(_service.GetForDetailAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> DetailsSearch(int UserId, string SearchTerm, bool AuthoredOpened, bool LikedStoriesOpened, bool SharedStoriesOpened, bool LikeableStoriesOpened)
        {

            var model = await _service.GetForDetailAsync(UserId, SearchTerm);
            model.AuthoredOpened = AuthoredOpened;
            model.LikedStoriesOpened = LikeableStoriesOpened;
            model.SharedStoriesOpened = SharedStoriesOpened;
            model.LikeableStoriesOpened = LikeableStoriesOpened;

            return View("Details", model);

        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Email,Password,ConfirmPassword")] AddUserViewModel user)
        {


            if (ModelState.IsValid)
            {
                await _service.CreateAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
           
            return View(await _service.GetForEditAsync(id));
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Email,OldPassword,Password,ConfirmPassword")] EditUserViewModel user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }          


            if (ModelState.IsValid)
            {
                await _service.EditAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            return View(await _service.GetByIdAsync(id));
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
