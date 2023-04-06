using Microsoft.AspNetCore.Mvc;
using Moq;
using PresseMots_DataModels.Entities;
using PresseMots_Web.Controllers;
using PresseMots_Web.Models;
using PresseMots_Web.Services;

namespace PresseMots_WebTests
{
    public class CommentsControllerTests
    {
        [Fact]
        public async Task Index_GetRedirectToStoriesIndex()
        {
            // ARRANGE
            var mockService = new Mock<ICommentService>();
            var controller = new CommentsController(mockService.Object);

            // ACT
            var result = await controller.Index(null);

            // ASSERT
            Assert.IsType<RedirectToActionResult>(result);

            var viewResult = result as RedirectToActionResult;
            Assert.Equal("Index", viewResult.ActionName);
            Assert.Equal("Stories", viewResult.ControllerName);

        }

        [Fact]
        public async Task Index_GetValidStoryId()
        {
            // ARRANGE
            var mockService = new Mock<ICommentService>();
            mockService.Setup(x => x.GetVMByStoryIdAsync(It.IsAny<int>())).ReturnsAsync(new CommentViewModel()
            {
                StoryId = 3
            });

            var controller = new CommentsController(mockService.Object);

            // ACT
            var result = await controller.Index(3);

            // ASSERT
            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.ViewData);

            var model = viewResult.ViewData.Model as CommentViewModel;
            Assert.IsType<CommentViewModel>(viewResult.Model);
            Assert.Equal(3, model.StoryId);
        }

        [Fact]
        public async Task Create_GetRedirectToStoriesIndex()
        {
            // ARRANGE
            var mockService = new Mock<ICommentService>();
            var controller = new CommentsController(mockService.Object);

            // ACT
            var result = controller.Create(It.IsAny<int?>());

            // ASSERT
            Assert.IsType<RedirectToActionResult>(result);

            var viewResult = result as RedirectToActionResult;
            Assert.Equal("Index", viewResult.ActionName);
            Assert.Equal("Stories", viewResult.ControllerName);
        }

        [Fact]
        public async Task Create_GetStoryIdValid()
        {
            // ARRANGE
            var mockService = new Mock<ICommentService>();
            var controller = new CommentsController(mockService.Object);

            // ACT
            var result = controller.Create(1);

            // ASSERT
            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.ViewData);

            var model = viewResult.ViewData.Model as Comment;
            Assert.IsType<Comment>(viewResult.Model);
            Assert.Equal(1, model.StoryId);
            Assert.Equal(0, model.Id);
        }

        [Fact]
        public async Task Create_PostValidCommentAsync()
        {
            // ARRANGE
            var mockServ = new Mock<ICommentService>();
            mockServ.Setup(x => x.CreateAsync(It.IsAny<Comment>())).ReturnsAsync(new Comment());
            var controller = new CommentsController(mockServ.Object);
            var comment = new Comment()
            {
                Id = 1
            };
            // ACT
            var result = await controller.Create(comment);
            var viewResult = result as RedirectToActionResult;

            // ASSERT
            mockServ.Verify( x => x.CreateAsync(It.IsAny<Comment>()), Times.Once());
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
            Assert.Equal(null, viewResult.ControllerName);
        }

        [Fact]
        public async Task Create_PostInvalidCommentAsync()
        {
            // ARRANGE
            var mockServ = new Mock<ICommentService>();
            mockServ.Setup(x => x.CreateAsync(It.IsAny<Comment>())).ReturnsAsync(new Comment());
            var controller = new CommentsController(mockServ.Object);
            var comment = new Comment()
            {
                Id = 1
            };
            controller.ModelState.AddModelError("", "Error");
            // ACT
            var result = await controller.Create(comment);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as Comment;

            // ASSERT
            mockServ.Verify(x => x.CreateAsync(It.IsAny<Comment>()), Times.Never());
            Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.ViewData);
            Assert.IsType<Comment>(viewResult.Model);
            Assert.Same(comment, model);
        }

    }
}