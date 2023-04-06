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
            //arrange
            var ICommentServiceMock = new Mock<ICommentService>();
            var controller = new CommentsController(ICommentServiceMock.Object);


            //act
            var result = await controller.Index(null);


            //assert
            Assert.IsType<RedirectToActionResult>(result);
            var viewResult = result as RedirectToActionResult;

            Assert.Equal("Index", viewResult.ActionName);
            Assert.Equal("Stories", viewResult.ControllerName);
            


        }

        [Fact]
        public async Task Index_GetValidStoryId()
        {
            //arrange
            var ICommentServiceMock = new Mock<ICommentService>();
            ICommentServiceMock.Setup(x => x.GetVMByStoryIdAsync(It.IsAny<int>())).ReturnsAsync(new CommentViewModel() { StoryId=3});
            var controller = new CommentsController(ICommentServiceMock.Object);


            //act
            var result = await controller.Index(3);


            //assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;

            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.ViewData);


            Assert.IsType<CommentViewModel>(viewResult.Model);
            var model = viewResult.ViewData.Model as CommentViewModel;

            Assert.Equal(3, model.StoryId);





        }



        [Fact]
        public void Create_GetRedirectToStoriesIndex() {

            //arrange
            var ICommentServiceMock = new Mock<ICommentService>();
            var controller = new CommentsController(ICommentServiceMock.Object);

            //act 
            var result =  controller.Create((int?)null);


            //assert
            Assert.IsType<RedirectToActionResult>(result);

            var viewResult = result as RedirectToActionResult;

            Assert.Equal("Index", viewResult.ActionName);
            Assert.Equal("Stories", viewResult.ControllerName);

        }




        [Fact]
        public void Create_GetStoryIdValid()
        {

            //arrange
            var ICommentServiceMock = new Mock<ICommentService>();
            var controller = new CommentsController(ICommentServiceMock.Object);

            //act 
            var result = controller.Create(12);


            //assert
            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;

            Assert.IsType<Comment>(viewResult.Model);

            var model = viewResult.Model as Comment;

            Assert.Equal(12, model.StoryId);
            Assert.Equal(0, model.Id);


        }

        [Fact]
        public async Task Create_PostValidCommentAsync() {

            //arrange
            var ICommentServiceMock = new Mock<ICommentService>();
            ICommentServiceMock.Setup(x => x.CreateAsync(It.IsAny<Comment>())).ReturnsAsync(new Comment());
            var controller = new CommentsController(ICommentServiceMock.Object);
            var comment = new Comment()
            {
                Id = 1
            };

            //act 

            var result = await controller.Create(comment);


            //assert 
            ICommentServiceMock.Verify(x => x.CreateAsync(It.IsAny<Comment>()), Times.Once());

            Assert.IsType<RedirectToActionResult>(result);

            var viewResult = result as RedirectToActionResult;

            Assert.Equal("Index", viewResult.ActionName);
            Assert.Equal(null, viewResult.ControllerName);




        }

        [Fact]
        public async Task Create_PostInvalidCommentAsync()
        {

            //arrange
            var ICommentServiceMock = new Mock<ICommentService>();
            ICommentServiceMock.Setup(x => x.CreateAsync(It.IsAny<Comment>())).ReturnsAsync(new Comment());
            var controller = new CommentsController(ICommentServiceMock.Object);
            var comment = new Comment()
            {
                Id = 1
            };
            controller.ModelState.AddModelError("", "Error");

            //act 

            var result = await controller.Create(comment);


            //assert 
            ICommentServiceMock.Verify(x => x.CreateAsync(It.IsAny<Comment>()), Times.Never());

            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;

            Assert.IsType<Comment>(viewResult.Model);

            var model = viewResult.Model as Comment;

            Assert.Same(comment,model);





        }

        [Fact]
        public async Task Edit_GetCommentIdValid()
        {
            //arrange
            var ICommentServiceMock = new Mock<ICommentService>();
            var commentReturn = new Comment() { Id = 2 };
            ICommentServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(commentReturn);
            var controller = new CommentsController(ICommentServiceMock.Object);

            //act
           var result = await controller.Edit(2);


            //Assert 
            ICommentServiceMock.Verify(x => x.GetByIdAsync(It.Is<int>(y => y == 2)), Times.Once());
            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;


            Assert.Same(commentReturn, viewResult.Model); 
            
            //on peut valider l`instance au lieu du id (ci-dessus). Si on fait ça, le reste n`est pas nécessaire. 


            //le reste. 

            Assert.IsType<Comment>(viewResult.Model);
            var model = viewResult.Model as Comment;
            Assert.Equal(2, model.Id);


        }

        [Fact]
        public async Task Edit_PostCommentValid() {

            //arrange
            var ICommentServiceMock = new Mock<ICommentService>();
            ICommentServiceMock.Setup(x => x.EditAsync(It.IsAny<Comment>()));
            var controller = new CommentsController(ICommentServiceMock.Object);
            var comment = new Comment()
            {
                Id = 1
            };

            //act 

            var result = await controller.Edit(comment.Id,comment);


            //assert 
            ICommentServiceMock.Verify(x => x.EditAsync(It.IsAny<Comment>()), Times.Once());

            Assert.IsType<RedirectToActionResult>(result);

            var viewResult = result as RedirectToActionResult;

            Assert.Equal("Index", viewResult.ActionName);
            Assert.Equal(null, viewResult.ControllerName);

        }


        [Fact]
        public async Task Edit_PostInvalidCommentAsync()
        {

            //arrange
            var ICommentServiceMock = new Mock<ICommentService>();
            ICommentServiceMock.Setup(x => x.EditAsync(It.IsAny<Comment>()));
            var controller = new CommentsController(ICommentServiceMock.Object);
            var comment = new Comment()
            {
                Id = 1
            };
            controller.ModelState.AddModelError("", "Error");

            //act 

            var result = await controller.Edit(comment.Id,comment);


            //assert 
            ICommentServiceMock.Verify(x => x.EditAsync(It.IsAny<Comment>()), Times.Never());

            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;

            Assert.IsType<Comment>(viewResult.Model);

            var model = viewResult.Model as Comment;

            Assert.Same(comment, model);





        }





    }
}