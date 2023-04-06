using PresseMots_DataAccess.Context;
using PresseMots_Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PresseMots_Web.Services
{
    public interface IUserService
        {

        public Task<SimpleUserViewModel> GetByIdAsync(int id);
        public Task<List<SimpleUserViewModel>> GetAllAsync();
            public Task<UserProfileViewModel> GetForDetailAsync(int id);
            public Task<UserProfileViewModel> GetForDetailAsync(int id, string SearchTerm);
            public Task CreateAsync(AddUserViewModel user);
            public Task EditAsync(EditUserViewModel user);
            public Task<EditUserViewModel> GetForEditAsync(int id);
            public Task DeleteAsync(int id);

        }

    }
