using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using XProject.Domain.Entities;
using Resources;

namespace XProject.Web.Areas.Admin.Models
{
    public class CreateUserModel
    {
        public List<int> UserRoles { get; set; }
        public IEnumerable<Role> Roles { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldShouldNotBeEmpty")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldShouldNotBeEmpty")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldPasswordMustBeaMinimumLengthOf6")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldShouldNotBeEmpty")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ConfirmPasswordAndPasswordDoNotMatch")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldShouldNotBeEmpty")]
        [RegularExpression(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheEmailAddressEnteredIsInvalid")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldShouldNotBeEmpty")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9\+]{1,}[0-9\-\ ]{3,15}$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldIsInvalid")]
        public string Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9\+]{1,}[0-9\-\ ]{3,15}$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldIsInvalid")]
        public string MobilePhone { get; set; }

        public HttpPostedFileBase Picture { get; set; }
        public string Address { get; set; }
        public string AddressEnglish { get; set; }
        public string FaxNo { get; set; }
        public string Website { get; set; }
    }

    public class EditUserModel
    {
        public EditUserModel()
        {
        }

        public EditUserModel(UserLogin userLogin)
        {
            ID = userLogin.ID;
            Username = userLogin.DisplayName;
            Email = userLogin.Email;
            Phone = userLogin.Phone;
            MobilePhone = userLogin.MobilePhone;
            UserRoles = userLogin.Roles;
            UserPicture = userLogin.Picture;
            LastAccess = userLogin.LastAccess;
            Address = userLogin.Address;
            AddressEnglish = userLogin.AddressEnglish;
            FaxNo = userLogin.FaxNo;
            Website = userLogin.Website;
        }

        public IEnumerable<Role> UserRoles { get; set; }
        public IEnumerable<Role> Roles { get; set; }

        [HiddenInput]
        public int ID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldShouldNotBeEmpty")]
        public string Username { get; set; }

        [MinLength(6, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldPasswordMustBeaMinimumLengthOf6")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ConfirmPasswordAndPasswordDoNotMatch")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldShouldNotBeEmpty")]
        //[RegularExpression(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheEmailAddressEnteredIsInvalid")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldShouldNotBeEmpty")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9\+]{1,}[0-9\-\ ]{3,15}$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldIsInvalid")]
        public string Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9\+]{1,}[0-9\-\ ]{3,15}$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldIsInvalid")]
        public string MobilePhone { get; set; }

        public HttpPostedFileBase Picture { get; set; }

        public string UserPicture { get; set; }
        public DateTime? LastAccess { get; set; }
        public string Address { get; set; }
        public string AddressEnglish { get; set; }
        public string FaxNo { get; set; }
        public string Website { get; set; }
    }
    public class AccountModel
    {
        public AccountModel()
        {
        }

        public AccountModel(UserLogin userLogin)
        {
            Username = userLogin.DisplayName;
            Email = userLogin.Email;
            Phone = userLogin.Phone;
            MobilePhone = userLogin.MobilePhone;
        }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldShouldNotBeEmpty")]
        public string Username { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldShouldNotBeEmpty")]
        public string OldPassword { get; set; }

        [MinLength(6, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldPasswordMustBeaMinimumLengthOf6")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ConfirmPasswordAndPasswordDoNotMatch")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldShouldNotBeEmpty")]
        //[RegularExpression(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheEmailAddressEnteredIsInvalid")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldShouldNotBeEmpty")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9\+]{1,}[0-9\-\ ]{3,15}$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldIsInvalid")]
        public string Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9\+]{1,}[0-9\-\ ]{3,15}$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheFieldIsInvalid")]
        public string MobilePhone { get; set; }

        public HttpPostedFileBase Picture { get; set; }

    }
}
