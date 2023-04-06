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
using PresseMots_Web.Services;
using Vereyon.Web;

namespace PresseMots_Web.Controllers
{
    public class StoriesController : Controller
    {
        private IServiceBase<User> _userServices;
        private readonly IStoryService _service;

        public StoriesController(IStoryService service, IServiceBase<User> userService)
        {
            _userServices = userService;
            _service = service;
        }

        // GET: Stories/Index/
        public  IActionResult Index(int? Id)
        {            
            return View( Id == null? _service.GetAll() : _service.GetById(Id.Value));
        }

        public IActionResult SearchByTag(string tagName) {

            //Implantez le
            return View(nameof(Index), _service.SearchByTagName(tagName));


        }


        // GET: Stories/Create
        public IActionResult Create()
        {
            ViewData["OwnerId"] = new SelectList(_userServices.GetAll(), "Id", "Username");
            return View();
        }

        // POST: Stories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create([Bind("Title,Content,OwnerId")] Story story)
        {
            if (ModelState.IsValid)
            {
                _service.Create(story);

                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerId"] = new SelectList(_userServices.GetAll(), "Id", "Username", story.OwnerId);
            return View(story);
        }

        // GET: Stories/Edit/5
        public  IActionResult Edit(int id)
        {


            var story =  _service.GetById(id);
            if (story == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_userServices.GetAll(), "Id", "Username", story.OwnerId);
            return View(story);
        }

        // POST: Stories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit([FromServices] HtmlSanitizer sanitizer, int id, [Bind("Id,Title,Content,Draft,OwnerId,CreationTime,LastEditTime,PublishTime")] Story story)
        {
            if (id != story.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                    _service.Edit(story);
             
                   return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerId"] = new SelectList(_userServices.GetAll(), "Id", "Username", story.OwnerId);
            return View(story);
        }

        // GET: Stories/Delete/5
        public  IActionResult Delete(int id)
        {
            return View(_service.GetById(id));
        }

        // POST: Stories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);

            return RedirectToAction(nameof(Index));
        }

    }
}
