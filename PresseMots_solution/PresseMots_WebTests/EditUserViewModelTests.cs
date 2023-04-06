using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using PresseMots_DataModels.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Moq;
using PresseMots_Web.Models;
using PresseMots_DataAccess.Services;

namespace PresseMots_WebTests
{
    public class EditUserViewModelTests
    {

        private string oldPassword = "avSJaSVg25D2EJ";

        public List<ValidationResult> ValidateEntity(object entity)
        {
            return ValidateEntity(entity, null);
        }

        public List<ValidationResult> ValidateEntity(object entity, string property)
        {
            //arrange common elements. 

            var _localsMock = new Mock<IStringLocalizer<AddUserViewModel>>();
            var errorMessage = "Passwords are not strong enough.";
            _localsMock.Setup(_ => _[errorMessage]).Returns(new LocalizedString(errorMessage, errorMessage));

            var _localsEditMock = new Mock<IStringLocalizer<EditUserViewModel>>();
            var errorMessageEdit = "Old password is not validated";
            _localsEditMock.Setup(_ => _[errorMessageEdit]).Returns(new LocalizedString(errorMessageEdit, errorMessageEdit));


            var _userServiceMock = new Mock<IServiceBase<User>>();
            _userServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Password = oldPassword });



            var services = new Mock<IServiceProvider>();
            services.Setup(x => x.GetService(typeof(IStringLocalizer<AddUserViewModel>))).Returns(_localsMock.Object);
            services.Setup(x => x.GetService(typeof(IStringLocalizer<EditUserViewModel>))).Returns(_localsEditMock.Object);
            services.Setup(x => x.GetService(typeof(IServiceBase<User>))).Returns(_userServiceMock.Object);





            var validationContext = new ValidationContext(entity, services.Object, null);
            var results = new List<ValidationResult>();

            //act
            Validator.TryValidateObject(entity, validationContext, results, validateAllProperties: true); //valide les propriétés
            Validator.TryValidateObject(entity, validationContext, results); //valide le IValidatableObject


            return results.Where(x => x.MemberNames.Any(y => property == null || y == property)).ToList();
        }


        /// <summary>
        /// Une autre façon de tester est de tester l'ensemble des validations. 
        /// C'est moins "unitaire" mais beaucoup plus facilement maintenable.
        /// Dans vos devoirs on préférera l'approche de AddUserViewModelTests
        /// </summary>
        [Theory]
        [InlineData("xxxxxxx")]
        [InlineData("")]
        [InlineData("yyy@")]
        [InlineData("@yyy")]
        [InlineData("@email.com")]
        public void EntityInvalidCheckEmailValidity(string email)
        {

            //arrange
            var entity = new EditUserViewModel()
            {
                Id = 0,
                ConfirmPassword = "b",
                Password = "a",
                Email = email,
                Username = new string('y', 200),
                OldPassword = "c"


            };

            //act
            var errors = ValidateEntity(entity).OrderBy(x => x.MemberNames.FirstOrDefault() ?? String.Empty); //il faut faire un order by sinon l'ordre n`est pas garanti!


            //assert
            Assert.Collection(errors,
                  x => Assert.Equal("Passwords don't match", x.ErrorMessage),
                  x => Assert.Equal(string.Format(DefaultValidationMessages.EmailAddressAttribute_Invalid, "Email"), x.ErrorMessage),
                  x => Assert.Equal("Old password is not validated", x.ErrorMessage),
                  x => Assert.Equal("Passwords are not strong enough.", x.ErrorMessage),
                  x => Assert.Equal(string.Format(DefaultValidationMessages.StringLengthAttribute_ValidationError, "Username", 100), x.ErrorMessage)
                );



        }


        /// <summary>
        /// Une autre façon de tester est de tester l'ensemble des validations. 
        /// C'est moins "unitaire" mais beaucoup plus facilement maintenable.
        /// Dans vos devoirs on préférera l'approche de AddUserViewModelTests
        /// </summary>
        [Theory]
        [InlineData("a")]
        [InlineData("aa")]
        [InlineData("abcd")]
        [InlineData("@yyy")]
        [InlineData("zxcvbn")]
        public void EntityInvalidCheckValidityPassword(string password)
        {

            //arrange
            var entity = new EditUserViewModel()
            {
                Id = 0,
                ConfirmPassword = "b",
                Password = password,
                Email = "xyz",
                Username = new string('y', 200),
                OldPassword = "c"


            };

            //act
            var errors = ValidateEntity(entity).OrderBy(x => x.MemberNames.FirstOrDefault() ?? String.Empty);


            //assert
            Assert.Collection(errors,
                  x => Assert.Equal("Passwords don't match", x.ErrorMessage),
                  x => Assert.Equal(string.Format(DefaultValidationMessages.EmailAddressAttribute_Invalid, "Email"), x.ErrorMessage),
                  x => Assert.Equal("Old password is not validated", x.ErrorMessage),
                  x => Assert.Equal("Passwords are not strong enough.", x.ErrorMessage),
                  x => Assert.Equal(string.Format(DefaultValidationMessages.StringLengthAttribute_ValidationError, "Username", 100), x.ErrorMessage)
                );



        }


        [Fact]
        public void EntityValid()
        {

            //arrange
            var entity = new EditUserViewModel()
            {
                Id = 0,
                ConfirmPassword = "moNRtUKM3Av2zX",
                Password = "moNRtUKM3Av2zX",
                Email = "x@y.com",
                Username = new string('y', 99),
                OldPassword = oldPassword


            };

            //act
            var errors = ValidateEntity(entity).OrderBy(x => x.MemberNames.FirstOrDefault() ?? String.Empty);


            //assert
            Assert.Empty(errors);



        }






    }
}
