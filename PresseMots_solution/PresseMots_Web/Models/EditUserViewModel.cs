using Microsoft.Extensions.Localization;
using PresseMots_DataAccess.Services;
using PresseMots_DataModels.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PresseMots_Web.Models
{
    public class EditUserViewModel : AddUserViewModel
    {
        public EditUserViewModel()
        {

        }

        public EditUserViewModel(User user)
        {
            if (user == null) return;
            this.Id = user.Id;
            this.Username = user.Username;
            this.Email = user.Email;

        }

        public EditUserViewModel(int? id, string username, string email, string password, string confirmPassword, string oldPassword) : base(id, username, email, password, confirmPassword)
        {
            OldPassword = oldPassword;
        }

        [Display(Name = "Old password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            var results = base.Validate(validationContext).ToList();

            foreach (var item in results)
            {
                yield return item;
            }


            var userService = validationContext.GetService(typeof(IServiceBase<User>)) as IServiceBase<User>;
            var locals = validationContext.GetService(typeof(IStringLocalizer<EditUserViewModel>)) as IStringLocalizer;

            var viewmodel = validationContext.ObjectInstance as EditUserViewModel;
            var model = userService.GetById(viewmodel.Id.Value);
            if (model.Password != viewmodel.OldPassword)
            {
                yield return new ValidationResult(locals["Old password is not validated"], new[] { nameof(OldPassword) });

            }


        }
    }
}
