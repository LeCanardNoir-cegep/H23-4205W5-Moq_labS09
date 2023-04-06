using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PresseMots_DataAccess.Context;
using PresseMots_DataModels.Entities;
using PresseMots_Utility;
using PresseMots_Web.Models;
using PresseMots_Web.Services;

namespace PresseMots_Web.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService _service;

        public CommentsController(ICommentService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int? storyId)
        {
            if (storyId == null) return RedirectToAction("Index", "Stories");
            var vm = await this._service.GetVMByStoryIdAsync(storyId.Value);
            return View(vm);
        }

 
        public IActionResult Create(int? storyId)
        {
            if (storyId == null) return RedirectToAction("Index", "Stories");
            return View(new Comment() { StoryId = storyId ?? 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,DisplayName,Rating,Content,StoryId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(comment);
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }


        public  async Task<IActionResult> Edit(int id) => View(await _service.GetByIdAsync(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Edit(int id, [Bind("Id,Email,DisplayName,Rating,Content,Hidden,StoryId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                await _service.EditAsync(comment);
                return RedirectToAction(nameof(Index),new { storyId=comment.StoryId });
            }
            return View(comment);
        }

        public async  Task<IActionResult> Delete(int id) => View(await _service.GetByIdAsync(id));
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storyId = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(id);

            return RedirectToAction(nameof(Index), new { storyId=storyId});
        }

    }
}
