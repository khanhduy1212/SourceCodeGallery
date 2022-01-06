using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;
using XProject.Domain.Helpers;
using XProject.Web.Areas.Admin.Models;
using XProject.Web.Infrastructure.Filters;
using XProject.Web.Infrastructure.Utility;

using Resources;
using StringHelper = XProject.Web.Infrastructure.Utility.StringHelper;

namespace XProject.Web.Areas.Admin.Controllers
{
    [RequestAuthorize]
    public class UsersController : Controller
    {
        private readonly IMembershipService _membershipService;
        private readonly IRoleService _roleService;
        private readonly ILoginTracker _loginTracker;
        private readonly IUnitRepository _repoUnit;

        public UsersController(IMembershipService membershipService, IRoleService roleService, ILoginTracker loginTracker, IUnitRepository repoUnit)
        {
            _membershipService = membershipService;
            _roleService = roleService;
            _loginTracker = loginTracker;
            _repoUnit = repoUnit;
        }
        [DisplayName("UserLogin management")]
        public ActionResult Index(int role = 0)
        {
            IEnumerable<UserLogin> users = _membershipService.GetUsers().Where(u => (role == 0 || u.Roles.Select(m => m.ID).Contains(role)) && !u.Roles.Any(m => m.Name.ToUpper().Contains("DEALER")));
            ViewBag.roles = _roleService.GetAllRoles().Where(n => !n.Name.ToUpper().Contains("DEALER"));
            return View(users);
        }

        [DisplayName("List deleted UserLogin")]
        public ActionResult ListDeletedUsers()
        {
            IEnumerable<UserLogin> users = _membershipService.GetUsersDeleted();
            ViewBag.roles = _roleService.GetAllRoles();
            return View(users);
        }
        public ActionResult Create()
        {
            var model = new CreateUserModel
                {
                    UserRoles = new List<int>(),
                    Roles = _roleService.GetAllRoles(),
                };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateUserModel model)
        {
            if (_membershipService.GetUserByName(model.Username) != null)
                ModelState.AddModelError("DisplayName", Resource.UserNameExists);

            if (_membershipService.GetUserByEmail(model.Email) != null)
                ModelState.AddModelError("Email", Resource.UserEmailExists);

            IEnumerable<int> userRoles = StringHelper.Ensure(Request.Form["SelectedRoles"])
                                                     .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(id => Convert.ToInt32(id));
            IEnumerable<int> branches = StringHelper.Ensure(Request.Form["SelectedBranches"])
                                                     .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(id => Convert.ToInt32(id));

            if (!ModelState.IsValid)
            {
                model.UserRoles = userRoles.ToList();
                model.Roles = _roleService.GetAllRoles();
                return View(model);
            }
            var user = new UserLogin
                {
                    DisplayName = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    Phone = model.Phone,
                    MobilePhone = model.MobilePhone,
                };

            user = _membershipService.CreateUser(user);

            string userPicture = UserPicture.Upload(user.ID, model.Picture);
            if (!string.IsNullOrEmpty(userPicture))
                _membershipService.UpdateUserPicture(user.ID, userPicture);

            _roleService.AssignRoles(user, userRoles);
            //_roleService.AssignBranches(UserLogin, new List<int> {UserLogin.BranchID});
            TempData["message"] = Resource.AddSuccessful;
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            UserLogin userLogin = _membershipService.GetUser(id);
            var model = new EditUserModel(userLogin)
                {
                    Roles = _roleService.GetAllRoles(),
                };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditUserModel model)
        {
            UserLogin userLogin = _membershipService.GetUserByName(model.Username);
            if (userLogin != null && userLogin.ID != model.ID)
                ModelState.AddModelError("DisplayName", Resource.UserNameExists);

            userLogin = _membershipService.GetUserByEmail(model.Email);
            if (userLogin != null && userLogin.ID != model.ID)
                ModelState.AddModelError("Email", Resource.UserEmailExists);

            if (string.IsNullOrEmpty(model.Password) && model.Password != model.ConfirmPassword)
                ModelState.AddModelError("UserLogin.Password", Resource.PasswordMismatch);

            IEnumerable<int> userRoles = StringHelper.Ensure(Request.Form["SelectedRoles"])
                                                     .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(id => Convert.ToInt32(id));
            //IEnumerable<int> branches = StringHelper.Ensure(Request.Form["SelectedBranches"])
            //                                         .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            //                                         .Select(id => Convert.ToInt32(id));
          
            if (!ModelState.IsValid)
            {
                var oldUser = _membershipService.GetUser(model.ID);
                model.UserRoles = _roleService.GetAllRoles().Where(m => userRoles.Contains(m.ID));
                model.Roles = _roleService.GetAllRoles();
                return View(model);
            }

            userLogin = _membershipService.GetUser(model.ID);
            userLogin.DisplayName = model.Username;
            userLogin.Email = model.Email;
            userLogin.Phone = model.Phone;
            userLogin.MobilePhone = model.MobilePhone;
            if (!string.IsNullOrEmpty(model.Password))
            {
                userLogin.Password = EncryptHelper.EncryptPassword(model.Password);
            }
            var success = _membershipService.UpdateUser(userLogin);
            string userPicture = UserPicture.Upload(model.ID, model.Picture);
            if (!string.IsNullOrEmpty(userPicture))
                _membershipService.UpdateUserPicture(userLogin.ID, userPicture);

            _roleService.AssignRoles(userLogin, userRoles);

            _loginTracker.ReloadUser(userLogin.Email, userLogin);
            if (success)
            {
                TempData["message"] = Resource.SaveSuccessful;
                return RedirectToAction("Index");
            }
            ViewBag.Success = true;
            ViewBag.Message = Resource.SaveFailed;
            return RedirectToAction("Edit", new { Id = model.ID });
        }
        [AjaxOnly]
        public ActionResult Delete_keeptrack(int id)
        {
            UserLogin userLogin = _membershipService.GetUser(id);
            if (userLogin != null)
            {
                _membershipService.DeleteUser(userLogin.ID);
            }
            return Json(1);
        }
        [HttpPost, ActionName("ReActive")]
        public ActionResult ReActive(int id)
        {
            var user = _membershipService.GetUser(id);
            if (_membershipService.GetAllUserByEmail(user.Email).Count() == 1)
            {
                try
                {
                    _membershipService.ReActiveUser(id);
                }
                catch (Exception)
                {
                    TempData["Message"] = Resource.CannotReactiveUser;
                    return RedirectToAction("ListDeletedUsers");
                }
            }
            else
            {
                TempData["Message"] = Resource.UserEmailActive;
                return RedirectToAction("ListDeletedUsers");
            }

            return RedirectToAction("ListDeletedUsers");
        }
    }

}
