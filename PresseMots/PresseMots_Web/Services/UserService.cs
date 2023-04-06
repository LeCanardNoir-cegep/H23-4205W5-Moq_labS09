using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using PresseMots_DataAccess.Context;
using PresseMots_DataModels.Entities;
using PresseMots_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresseMots_Web.Services
{
    


    public class UserService : PresseMots_DataAccess.Services.ServiceBaseEF<User>, IUserService
    {
        private IStringLocalizer<UserService> _localizer;

        public UserService(PresseMotsDbContext dbContext, IStringLocalizer<UserService> localizer) : base(dbContext)
        {
            this._localizer = localizer;


        }

        async Task<List<SimpleUserViewModel>> IUserService.GetAllAsync()
        {
            var list = await this.GetAllAsync();
            var result = list.Select(x => new SimpleUserViewModel(x.Id, x.Username, x.Email, x.Stories.Count())).ToList();
            return result;
        }

      public  async Task<UserProfileViewModel> GetForDetailAsync(int id, string SearchTerm)
        {
            var user = await this.GetByIdAsync(id);
            if (user == null) throw new System.ArgumentException("Id not found");



            var userStories = findStories(user.Stories, SearchTerm);
            var stories = userStories.Select(x => new UserProfileStoryViewModel(x)).ToList();
            var likes = findStories(user.Stories, SearchTerm).SelectMany(x => x.Likes.Select(y => y.Story)).Select(x => new UserProfileStoryViewModel(x)).ToList();
            var shares = findStories(user.Stories, SearchTerm).SelectMany(x => x.Shares.Select(y => y.Story)).Select(x => new UserProfileStoryViewModel(x)).ToList();

            var tags = user.Stories.SelectMany(x => x.StoryTags.Select(y => y.TagId)).ToList();
            var likeables = findStories(_dbContext.Stories, SearchTerm).Where(x => x.StoryTags.Any(x => tags.Contains(x.TagId))).Select(x => new UserProfileStoryViewModel(x)).ToList();

            var userVM = new UserProfileViewModel(user, string.Empty, false, false, false, false, stories, likes, shares, likeables);

            return userVM;
        }

        private IQueryable<Story> findStories(IEnumerable<Story> stories, string searchText)
        {

            var elem = stories.AsQueryable();
            searchText = searchText?.ToUpper() ?? String.Empty;
            var searchTextSplit = searchText.Split(" ");
            foreach (var searchItem in searchTextSplit)
            {
                elem = elem.Where(x => x.Title.ToUpper().Contains(searchItem) || x.Content.ToUpper().Contains(searchItem));

            }
            return elem;



        }

       public async Task<UserProfileViewModel> GetForDetailAsync(int id)
        {
            return await GetForDetailAsync(id, string.Empty);
        }


        public async Task EditAsync(EditUserViewModel user)
        {
            if (user.Id == null) throw new System.ArgumentException("Id not found");
            var userFromDb = await GetByIdAsync(user.Id.Value);


            userFromDb.Username = user.Username;
            userFromDb.Email = user.Email;
            userFromDb.Password = user.Password;

            await this.EditAsync(userFromDb);
        }

        public async Task<EditUserViewModel> GetForEditAsync(int id)
        {
            var user = new EditUserViewModel(await GetByIdAsync(id));
            return user;
        }

        public async Task CreateAsync(AddUserViewModel user)
        {

            var newUser = new User()
            {
                Id = 0,
                Username = user.Username,
                Email = user.Email,
                Password = user.Password

            };
            
            await this.CreateAsync(newUser);

        }

        async Task<SimpleUserViewModel> IUserService.GetByIdAsync(int id)
        {
            var user = await this.GetByIdAsync(id);
            var result = new SimpleUserViewModel(user, user.Stories.Count());
            return result;
        }
    }
}
