using Microsoft.Extensions.Localization;
using Moq;
using PresseMots_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresseMots_WebTests
{
    public class AddUserViewModelTests
    {

        public List<ValidationResult> ValidateEntity(object entity)
        {
            return ValidateEntity(entity, null);
        }

        public List<ValidationResult> ValidateEntity(object entity, string property) {

            //arrange common elements. 

            var _localsMobk = new Mock<IStringLocalizer<AddUserViewModel>>();
            var errorMessage = "Passwords are not strong enough.";
            _localsMobk.Setup(_ => _[errorMessage]).Returns(new LocalizedString(errorMessage, errorMessage));
            var services = new Mock<IServiceProvider>();
            services.Setup(x => x.GetService(typeof(IStringLocalizer<AddUserViewModel>))).Returns(_localsMobk.Object);

            var validationContext = new ValidationContext(entity, services.Object, null);
            var results = new List<ValidationResult>();

            //act
            Validator.TryValidateObject(entity, validationContext, results, validateAllProperties: true); //valide les propriétés
            Validator.TryValidateObject(entity, validationContext, results); //valide le IValidatableObject


            return results.Where(x => x.MemberNames.Any(y => property == null || y == property)).ToList();
        }


        [Fact]
        public void UsernameTooLong() {

            //arrange
            var entity = new AddUserViewModel() {
                Username = new string('x',200),
              
            };

            var expectedMessage = string.Format(DefaultValidationMessages.StringLengthAttribute_ValidationError, "Username", 100);


            //act
            var errors = ValidateEntity(entity,nameof(entity.Username));

         
            //assert
            Assert.Collection(errors, x => Assert.Equal(expectedMessage,x.ErrorMessage));


        }

        [Fact]
        public void UsernameValid() {

            //arrange
            var entity = new AddUserViewModel()
            {
                Username = new string('x', 99),
            
            };


            //act
            var errors = ValidateEntity(entity, nameof(entity.Username));


            //assert
            Assert.Empty(errors);

        }

        [Theory]
        [InlineData("xxxxxxx")]
        [InlineData("")]
        [InlineData("yyy@")]
        [InlineData("@yyy")]
        [InlineData("@email.com")]
        public void EmailMalformed(string email)
        {
            //arrange
            var entity = new AddUserViewModel()
            {
                Email = email,

            };

            var expectedMessage = string.Format(DefaultValidationMessages.EmailAddressAttribute_Invalid, "Email");


            //act
            var errors = ValidateEntity(entity, nameof(entity.Email));


            //assert
            Assert.Collection(errors, x => Assert.Equal(expectedMessage, x.ErrorMessage));

        }

        [Theory]
        [InlineData("yyyy@pressemot.com")]
        [InlineData("yy.yy@pressemot.com")]
        [InlineData("yyyy@pressemot.jp.com")]
        public void EmailValid(string email)
        {
            //arrange
            var entity = new AddUserViewModel()
            {
                Email = email,

            };


            //act
            var errors = ValidateEntity(entity, nameof(entity.Email));


            //assert
            Assert.Empty(errors);

        }



        [Fact]
        public void PasswordsDontMatch()
        {
            //arrange
            var entity = new AddUserViewModel()
            {
                Password = "abcd",
                ConfirmPassword = "efgh"

            };

            var expectedMessage = "Passwords don't match";


            //act
            var errors = ValidateEntity(entity, nameof(entity.ConfirmPassword));


            //assert
            Assert.Collection(errors, x => Assert.Equal(expectedMessage, x.ErrorMessage));

        }



        [Fact]
        public void PasswordsTooWeak()
        {
            //arrange
            var entity = new AddUserViewModel()
            {
                Password = "zzxcvbn"

            };

            var expectedMessage = "Passwords are not strong enough.";


            //act
            var errors = ValidateEntity(entity, nameof(entity.Password));


            //assert
            Assert.Collection(errors, x => Assert.Equal(expectedMessage, x.ErrorMessage));

        }


        [Fact]
        public void PasswordsValid()
        {
            //arrange
            var entity = new AddUserViewModel()
            {
                Password = "C4KQFc4xkDw3Mn",
                ConfirmPassword = "C4KQFc4xkDw3Mn"

            };



            //act
            var passwordErrors = ValidateEntity(entity, nameof(entity.Password));
            var confirmPasswordErrors = ValidateEntity(entity, nameof(entity.ConfirmPassword));


            //assert
            Assert.Empty(passwordErrors);
            Assert.Empty(confirmPasswordErrors);

        }







    }
}
