using Microsoft.Extensions.Localization;
using Moq;
using PresseMots_Web.Controllers;
using PresseMots_Web.Models;
using PresseMots_Web.Services;
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
        public List<ValidationResult> ValidateEntity(object entity, string property)
        {
            /** ARRANGE */
            // Mock le provider de traduction du ViewModel AddUserViewModel.
            var mockLocals = new Mock<IStringLocalizer<AddUserViewModel>>();
            var errorMessage = "Passwords are not strong enough.";
            mockLocals.Setup( _ => _[errorMessage]).Returns( new LocalizedString(errorMessage, errorMessage) );

            // Mock le provider service
            // Injection de dépendance de la traduction
            var mockServices = new Mock<IServiceProvider>();
            mockServices.Setup(s => s.GetService(typeof(IStringLocalizer<AddUserViewModel>))).Returns(mockLocals.Object);

            // Création du context de validation
            // On a besoin de l'entité à valider et des mockServices requis.
            // Ici nous avons besoin du service de traduction inclu dans les ServicesProvider
            var validationContext = new ValidationContext(entity, mockServices.Object, null);
            var results = new List<ValidationResult>();

            /** ACT */
            // Valider toutes les propriétés de l'entité
            Validator.TryValidateObject(entity, validationContext, results, validateAllProperties: true);

            // Valider le IValidatableObject. Au fond valider la validation ????
            Validator.TryValidateObject(entity, validationContext, results); 

            return results.Where(x => x.MemberNames.Any(y => property == null || y == property)).ToList();

        }

        [Fact]
        public void UsernameTooLong()
        {
            /** ARRANGE */
            var entity = new AddUserViewModel(){ Username = new string('x', 200) };
            var expectedMessage = string.Format(DefaultValidationMessages.StringLengthAttribute_ValidationError, "Username", 100);

            /** ACT */
            var error = ValidateEntity(entity, nameof(entity.Username));

            /** ASSERT */
            Assert.Collection(error, x => Assert.Equal(expectedMessage, x.ErrorMessage));
        }

        [Fact]
        public void UsernameValid()
        {
            /** ARRANGE */
            var entity = new AddUserViewModel(){ Username = new string('x', 99) };

            /** ACT */
            var error = ValidateEntity(entity, nameof(entity.Username));

            /** ASSERT */
            Assert.Empty(error);
        }

        [Theory]
        [InlineData("xxxxxxx")]
        [InlineData("")]
        [InlineData("yyy@")]
        [InlineData("@yyy")]
        [InlineData("@email.com")]
        public void EmailMalformed(string email)
        {
            /** ARRANGE */
            var entity = new AddUserViewModel(){ Email = email };
            var expectedMessage = string.Format(DefaultValidationMessages.EmailAddressAttribute_Invalid, "Email");

            /** ACT */
            var error = ValidateEntity(entity, nameof(entity.Email));

            /** ASSERT */
            Assert.Collection(error, x => Assert.Equal(expectedMessage, x.ErrorMessage));
        }

        [Theory]
        [InlineData("yyyy@pressemot.com")]
        [InlineData("yy.yy@pressemot.com")]
        [InlineData("yyyy@pressemot.jp.com")]
        public void EmailValid(string email)
        {
            /** ARRANGE */
            var entity = new AddUserViewModel(){ Email = email };

            /** ACT */
            var error = ValidateEntity(entity, nameof(entity.Email));

            /** ASSERT */
            Assert.Empty(error);
        }

        [Fact]
        public void PasswordsDontMatch()
        {
            /** ARRANGE */
            var entity = new AddUserViewModel() { Password = "1234", ConfirmPassword = "4321" };
            var expectedMessage = "Passwords don't match";

            /** ACT */
            var error = ValidateEntity(entity, nameof(entity.ConfirmPassword));

            /** ASSERT */
            Assert.Collection(error, x => Assert.Equal(expectedMessage, x.ErrorMessage));
        }

        [Fact]
        public void PasswordsTooWeak()
        {
            /** ARRANGE */
            var entity = new AddUserViewModel() { Password = "1234" };
            var expectedMessage = "Passwords are not strong enough.";

            /** ACT */
            var error = ValidateEntity(entity, nameof(entity.Password));

            /** ASSERT */
            Assert.Collection(error, x => Assert.Equal(expectedMessage, x.ErrorMessage));
        }

        [Fact]
        public void PasswordsValid()
        {
            /** ARRANGE */
            var entity = new AddUserViewModel() { Password = "asdf211wERRE&?" };

            /** ACT */
            var error = ValidateEntity(entity, nameof(entity.Password));

            /** ASSERT */
            Assert.Empty(error);
        }

    }
}
