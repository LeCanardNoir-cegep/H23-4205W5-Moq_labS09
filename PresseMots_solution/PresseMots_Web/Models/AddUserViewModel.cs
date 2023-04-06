using Microsoft.Extensions.Localization;
using PresseMots_DataModels.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PresseMots_Web.Models
{
    public class AddUserViewModel : IValidatableObject
    {
        public AddUserViewModel(int? id, string username, string email, string password, string confirmPassword)
        {
            Id = id;
            Username = username;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }

        public AddUserViewModel(User user)
        {
            if (user == null) return;
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;


        }

        public AddUserViewModel()
        {

        }

        public int? Id { get; set; }
        [Display(Name = "Username")]
        [StringLength(100)]
        public string Username { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "ConfirmPassword")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance as AddUserViewModel;
            var locals = validationContext.GetService(typeof(IStringLocalizer<AddUserViewModel>)) as IStringLocalizer;
            var result = Zxcvbn.Core.EvaluatePassword(model.Password ?? "x");
            if (result.Score < 3)
            {
                yield return new ValidationResult(locals["Passwords are not strong enough."], new[] { nameof(Password) });
            }



        }
    }
}
