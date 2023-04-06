using PresseMots_DataAccess.Services;
using PresseMots_DataModels.Entities;
using PresseMots_Web.Models;
using System.Threading.Tasks;

namespace PresseMots_Web.Services
{
    public interface ICommentService : IServiceBaseAsync<Comment> {
            public Task<CommentViewModel> GetVMByStoryIdAsync(int storyId);
        }

}
