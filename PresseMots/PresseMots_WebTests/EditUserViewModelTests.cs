using Microsoft.Extensions.Localization;
using Moq;
using PresseMots_DataAccess.Services;
using PresseMots_DataModels.Entities;
using PresseMots_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresseMots_WebTests
{
    public class EditUserViewModelTests
    {
        private string oldPassword = "avSJaSVg25D2EJ";
        public List<ValidationResult> ValidateEntity(object entity)
        {
            return ValidateEntity(entity, null);
        }

        private List<ValidationResult> ValidateEntity(object entity, string property)
        {
            /** ARRANGE */

            /** 
             *  Mock du service de traduction de AddUserViewModel
             *  RÉFÉRENCE: 
             *  PresseMots_Web.Models.AddUserViewModel.Validate(...) ligne 55 
            */
            var mockLocalsAddUser = new Mock<IStringLocalizer<AddUserViewModel>>();
            var errorMessageAddUser = "Passwords are not strong enough.";
            mockLocalsAddUser.Setup(_ => _[errorMessageAddUser]).Returns(new LocalizedString(errorMessageAddUser, errorMessageAddUser));


            /** 
             *  Mock du service de traduction de EditUserViewModel
             *  
             *  RÉFÉRENCE: 
             *  PresseMots_Web.Models.EditUserViewModel.Validate(...) ligne 48
            */
            var mockLocalsEditUser = new Mock<IStringLocalizer<EditUserViewModel>>();
            var errorMessageEditUser = "Old password is not validated";
            mockLocalsEditUser.Setup(_ => _[errorMessageEditUser]).Returns(new LocalizedString(errorMessageEditUser, errorMessageEditUser));

            /** 
             *  Mock du service d'access aux données Repository pattern générique
             *  parce que la validation dans le ViewModel va chercher l'entité correspondante
             *  afin de vérifier que le vieux password est différent du nouveau.
             *  
             *  RÉFÉRENCE: 
             *  PresseMots_Web.Models.EditUserViewModel.Validate(...) ligne 47
            */
            var mockUserService = new Mock<IServiceBase<User>>();
            mockUserService.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Password = oldPassword });

            /** 
             *  Mock du ServiceProvider nécessaire à l'interface  IValidatableObject
             *  
             *  RÉFÉRENCE: 
             *  PresseMots_Web.Models.AddUserViewModel.Validate(...) ligne 55
             *  
             *  RÉFÉRENCE: 
             *  PresseMots_Web.Models.EditUserViewModel.Validate(...) ligne 47 et 48
            */
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IStringLocalizer<AddUserViewModel>))).Returns(mockLocalsAddUser.Object);
            mockServiceProvider.Setup(x => x.GetService(typeof(IStringLocalizer<EditUserViewModel>))).Returns(mockLocalsEditUser.Object);
            mockServiceProvider.Setup(x => x.GetService(typeof(IServiceBase<User>))).Returns(mockUserService.Object);

            /** 
             *  Tester les validations des modèles.
             *  Création du context de validation
             *  On a besoin de l'entité à valider et des mockServiceProvider requis.
             *  Ici nous avons besoin des IStringLocalizer et IServiceBase dans le ServicesProvider 
             *  
             *  RÉFÉRENCE:
             *  v. PDF S09_Moq.pdf p. 15
            */
            var validationContext = new ValidationContext(entity, mockServiceProvider.Object, null);
            var results = new List<ValidationResult>();

            /** ACT */
            Validator.TryValidateObject(
                instance: entity,
                validationContext: validationContext,
                validationResults: results,
                validateAllProperties: true
                );
            Validator.TryValidateObject(
                instance: entity,
                validationContext: validationContext,
                validationResults: results
                );

            return results.Where(r => r.MemberNames.Any(m => property == null || m == property)).ToList();
        }

        [Theory]
        [InlineData("xxxxxx")]
        [InlineData("")]
        [InlineData("yyy@")]
        [InlineData("@yyy")]
        [InlineData("@email.com")]
        public void EntityInvalidCheckEmailValidity(string email)
        {
            // ARRANGE
            var entity = new EditUserViewModel
            {
                Id = 0,
                ConfirmPassword = "b",
                Password = "a",
                Email = email,
                Username = new string('x', 200),
                OldPassword = "c"
            };
            // ACT
            var error = ValidateEntity(entity).OrderBy(o => o.MemberNames.FirstOrDefault() ?? String.Empty);

            // ASSERT
            Assert.Collection(error,
                x => Assert.Equal("Passwords don't match", x.ErrorMessage),
                x => Assert.Equal(string.Format(DefaultValidationMessages.EmailAddressAttribute_Invalid, "Email"), x.ErrorMessage),
                x => Assert.Equal("Old password is not validated", x.ErrorMessage),
                x => Assert.Equal("Passwords are not strong enough.", x.ErrorMessage),
                x => Assert.Equal(string.Format(DefaultValidationMessages.StringLengthAttribute_ValidationError, "Username", 100), x.ErrorMessage)
                );

        }

        [Theory]
        [InlineData("a")]
        [InlineData("aa")]
        [InlineData("abcd")]
        [InlineData("@yyy")]
        [InlineData("zxcvbn")]
        public void EntityInvalidCheckValidityPassword(string password)
        {
            // ARRANGE
            var entity = new EditUserViewModel
            {
                Id = 0,
                ConfirmPassword = "b",
                Password = password,
                Email = "test",
                Username = new string('x', 200),
                OldPassword = "c"
            };
            // ACT
            var error = ValidateEntity(entity).OrderBy(o => o.MemberNames.FirstOrDefault() ?? String.Empty);

            // ASSERT
            Assert.Collection(error,
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
            // ARRANGE
            var entity = new EditUserViewModel
            {
                Id = 0,
                ConfirmPassword = "lklkasjd12R",
                Password = "lklkasjd12R",
                Email = "test@test.com",
                Username = new string('x', 50),
                OldPassword = oldPassword
            };
            // ACT
            var error = ValidateEntity(entity).OrderBy(o => o.MemberNames.FirstOrDefault() ?? String.Empty);

            // ASSERT
            Assert.Empty(error);

        }

    }
}
