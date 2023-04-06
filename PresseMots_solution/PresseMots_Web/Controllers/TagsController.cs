using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PresseMots_DataAccess.Context;
using PresseMots_DataAccess.Services;
using PresseMots_DataModels.Entities;

namespace PresseMots_Web.Controllers
{
    public class TagsController : Controller
    {
        private readonly IServiceBaseAsync<Tag> _service;

        public TagsController(IServiceBaseAsync<Tag> service)
        {
            _service = service;
        }

        // GET: Tags
        public async Task<IActionResult> Index()
        {
              return View(await _service.GetAllAsync());
        }

     

        // GET: Tags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(tag);
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

       

        // GET: Tags/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            return View(await _service.GetByIdAsync(id));
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

    }
}
