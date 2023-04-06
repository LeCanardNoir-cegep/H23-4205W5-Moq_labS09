using PresseMots_DataAccess.Context;
using PresseMots_DataAccess.Services;
using PresseMots_DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;

namespace PresseMots_Web.Services
{
    public interface IStoryService : IServiceBase<Story>
    {
        public List<Story> SearchByTagName(string tagName);
    }
    public class StoryService : ServiceBaseEF<Story>, IStoryService
    {
        private HtmlSanitizer sanitizer;

        public StoryService(PresseMotsDbContext dbContext, HtmlSanitizer sanitizer) : base(dbContext)
        {
            this.sanitizer = sanitizer;
        }

        public List<Story> SearchByTagName(string tagName)
        {
            return _dbContext.Stories.Where(x => x.StoryTags.Any(y => y.Tag.Name.ToUpper() == tagName.ToUpper())).ToList();
        }

        public override Story Create(Story story) {

            story.CreationTime = DateTime.Now;
            story.PublishTime = null;
            story.LastEditTime = null;
            story.Draft = true;
            story.Content = sanitizer.Sanitize(story.Content);
            return base.Create(story);


        }

        public override void Edit(Story story)
        {
            
            story.LastEditTime = DateTime.Now;
            story.Content = sanitizer.Sanitize(story.Content);

            base.Edit(story);


        }



    }


}
