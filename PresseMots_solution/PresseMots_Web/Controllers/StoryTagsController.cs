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
    public class StoryTagsController : Controller
    {
        private readonly IServiceBaseAsync<StoryTag> _service;
        private readonly IServiceBaseAsync<Story> _storyService;
        private readonly IServiceBaseAsync<Tag> _tagService;

        public StoryTagsController(IServiceBaseAsync<StoryTag> service, IServiceBaseAsync<Story> storyService, IServiceBaseAsync<Tag> tagService)
        {
            _service =service;
            _storyService = storyService;
            _tagService = tagService;
        }


  
        public async Task<IActionResult> Create(int storyId)
        {
            var story = await _storyService.GetByIdAsync(storyId);
            if (story == null)
            {
                return NotFound();
            }
            ViewBag.StoryTitle = story.Title;


            ViewData["TagId"] = new SelectList(await _tagService.GetAllAsync(), "Id", "Name");
            return View(new StoryTag() { Id=0, StoryId=storyId, TagId=0});
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StoryId,TagId")] StoryTag storyTag)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(storyTag);
                return RedirectToAction("Index", "Stories", new { Id = storyTag.StoryId});
            }

            var story = await _storyService.GetByIdAsync(storyTag.StoryId);
            if (story == null)
            {
                return NotFound();
            }
            ViewBag.StoryTitle = story.Title;

            ViewData["TagId"] = new SelectList(await _tagService.GetAllAsync(), "Id", "Name", storyTag.TagId);
            return View(storyTag);
        }

      

      


        public async Task<IActionResult> Delete(int id)
        {

            var storyTag = await _service.GetByIdAsync(id);
            
            return View(storyTag);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           var story = await _service.GetByIdAsync(id);
            await this._service.DeleteAsync(id);

            return RedirectToAction(nameof(Index), "Stories", new { Id=story.StoryId});
        }

  
    }
}
