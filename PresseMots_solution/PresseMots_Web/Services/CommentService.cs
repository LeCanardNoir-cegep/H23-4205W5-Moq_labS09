using Microsoft.EntityFrameworkCore;
using PresseMots_DataAccess.Context;
using PresseMots_DataModels.Entities;
using PresseMots_Utility;
using PresseMots_Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PresseMots_Web.Services
{
    public class CommentService : PresseMots_DataAccess.Services.ServiceBaseEF<Comment>, ICommentService
    {
        private WordCounter wordCounter;

        public CommentService(PresseMotsDbContext dbContext, WordCounter wordCounter) : base(dbContext)
        {
            this.wordCounter = wordCounter;

        }

        public async Task DeleteAsync(int id) {
            var fromDb = await this.GetByIdAsync(id);
            fromDb.Hidden = true;
            await this.EditAsync(fromDb);        
        }

        public async Task<CommentViewModel> GetVMByStoryIdAsync(int storyId)
        {
            var story = await _dbContext.Stories.Where(x => x.Id == storyId).FirstOrDefaultAsync();


            if (story == null) throw new ArgumentNullException("Story is not found");




            var wordCount = wordCounter.Count(story.Content);
            var title = story.Title;
            var maxSubStr = story.Content?.Length ?? 0;
            if (maxSubStr > 300) maxSubStr = 300;
            var shortStory = story.Content?.Substring(0, maxSubStr);

            var vm = new CommentViewModel();

            vm.WordCount = wordCount;
            vm.ShortTitle = title;
            vm.ShortStory = shortStory;
            vm.StoryId = storyId;
            vm.Comments = story.Comments.Where(x => !x.Hidden).OrderBy(x => x.Id).ToList();

            return vm;

        }
    }
}
