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
            var _localsMobk = new Mock<IStringLocalizer<AddUserViewModel>>();
            var errorMessage = "Passwords are not string enough.";
            _localsMobk.Setup( _ => _[errorMessage]).Returns( new LocalizedString(errorMessage, errorMessage) );

            // Mock le provider service
            // Injection de dépendance de la traduction
            var services = new Mock<IServiceProvider>();
            services.Setup(s => s.GetService(typeof(IStringLocalizer<AddUserViewModel>))).Returns(_localsMobk.Object);

            // Création du context de validation
            // On a besoin de l'entité à valider et des services requis.
            // Ici nous avons besoin du service de traduction inclu dans les ServicesProvider
            var validationContext = new ValidationContext(entity, services.Object, null);
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
            var entity = new AddUserViewModel
            {
                Username = new string('x', 200)
            };

            //var expectedMessage = string.Format(DefaultValidationMessages);

            /** ACT */
            /** ASSERT */
        }

        [Fact]
        public void UsernameValid()
        {
            /** ARRANGE */
            /** ACT */
            /** ASSERT */
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
            /** ACT */
            /** ASSERT */
        }

        [Theory]
        [InlineData("yyyy@pressemot.com")]
        [InlineData("yy.yy@pressemot.com")]
        [InlineData("yyyy@pressemot.jp.com")]
        public void EmailValid(string email)
        {
            /** ARRANGE */
            /** ACT */
            /** ASSERT */
        }

        [Fact]
        public void PasswordsDontMatch()
        {
            /** ARRANGE */
            /** ACT */
            /** ASSERT */
        }

        [Fact]
        public void PasswordsTooWeak()
        {
            /** ARRANGE */
            /** ACT */
            /** ASSERT */
        }

        [Fact]
        public void PasswordsValid()
        {
            /** ARRANGE */
            /** ACT */
            /** ASSERT */
        }

    }
}
