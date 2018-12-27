using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using IPMI.Models;
using IPMI.Master;
using System;
using System.Collections.Generic;

namespace IPMI.Controllers
{
    public class UserController : Controller
    {
        private IPMI.ApplicationUserManager _userManager;
        private IPMI.ApplicationRoleManager _roleManager;

        // Controllers

        // GET: /User/
        //[Authorize(Roles = "Administrator")]
        #region public ActionResult Index(string searchStringUserNameOrEmail)
        public ActionResult Index()
        {
            try
            {
                MasterRepository MasterRepository = new MasterRepository();
                var Result = MasterRepository.GetListUsers("");

                return View(Result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<ExpandedUserDTO> Result = new List<ExpandedUserDTO>();

                return View(Result);
            }
        }
        #endregion

        // Users *****************************

        // GET: /User/Edit/Create 
        [Authorize(Roles = "Administrator")]
        #region public ActionResult Create()
        public ActionResult Create()
        {
            ExpandedUserDTO objExpandedUserDTO = new ExpandedUserDTO();
            ModelState.Clear();
            //ViewBag.IdLevel = GetAllLevelAsSelectList();
            ViewBag.IdDept = GetDeptToList();
            ViewBag.Roles = GetAllRolesAsSelectList();
            //ViewBag.PartnerID = GetAllPartnerAsSelectList();

            return View(objExpandedUserDTO);
        }
        #endregion

        // PUT: /User/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region public ActionResult Create(ExpandedUserDTO paramExpandedUserDTO)
        public ActionResult Create(ExpandedUserDTO paramExpandedUserDTO)
        {
            try
            {
                if (paramExpandedUserDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ApplicationUser result = UserManager.FindByName(paramExpandedUserDTO.NameIdentifier);
                if (result == null)
                {
                    var NameIdentifier = paramExpandedUserDTO.NameIdentifier.Trim();
                    var Email = paramExpandedUserDTO.Email.Trim();
                    //int IdLevel = paramExpandedUserDTO.IdLevel;
                    var IdDept = paramExpandedUserDTO.IdDept;
                    var UserName = paramExpandedUserDTO.NameIdentifier.Trim();
                    var Password = paramExpandedUserDTO.Password.Trim();
                    //var PartnerID = paramExpandedUserDTO.PartnerID.Trim();

                    if (NameIdentifier == "")
                    {
                        throw new Exception("No Name Identifier");
                    }
                    if (Email == "")
                    {
                        throw new Exception("No Email");
                    }

                    if (Password == "")
                    {
                        throw new Exception("No Password");
                    }

                    // UserName is LowerCase of the Email
                    //UserName = Email.ToLower();

                    // Create user

                    var objNewUserUser = new ApplicationUser { IdDept = IdDept, NameIdentifier = NameIdentifier, UserName = UserName, Email = Email };
                    var UserUserCreateResult = UserManager.Create(objNewUserUser, Password);

                    if (UserUserCreateResult.Succeeded == true)
                    {
                        string strNewRole = Convert.ToString(Request.Form["Roles"]);

                        if (strNewRole != "0")
                        {
                            // Put user in role
                            UserManager.AddToRole(objNewUserUser.Id, strNewRole);
                        }

                        return Redirect("~/User/Index");
                    }
                    else
                    {
                        //ViewBag.IdLevel = GetAllLevelAsSelectList();
                        //ViewBag.IdGroup = GetAllGroupAsSelectList();
                        ViewBag.Roles = GetAllRolesAsSelectList();
                        ViewBag.IdDept = GetDeptToList();
                        //ViewBag.PartnerID = GetAllPartnerAsSelectList();
                        ModelState.AddModelError(string.Empty,
                            "Error: Failed to create the user. Check password requirements or username has been taken");
                        return View(paramExpandedUserDTO);
                    }
                }
                else
                {
                    throw new Exception("Username has been taken, input another one");
                }
                
            }
            catch (Exception ex)
            {
                //ViewBag.IdLevel = GetAllLevelAsSelectList();
                //ViewBag.IdGroup = GetAllGroupAsSelectList();
                ViewBag.Roles = GetAllRolesAsSelectList();
                ViewBag.IdDept = GetDeptToList();
                //ViewBag.PartnerID = GetAllPartnerAsSelectList();
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("Create");
            }
        }
        #endregion

        // GET: /User/Edit/TestUser 
        [Authorize(Roles = "Administrator")]
        #region public ActionResult EditUser(string UserName)
        public ActionResult EditUser(string UserName)
        {
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);
            //ViewBag.IdLevel = GetAllLevelAsSelectList();
            //ViewBag.IdGroup = GetAllGroupAsSelectList();
            //ViewBag.PartnerID = GetAllPartnerAsSelectList();
            ViewBag.IdDept = GetDeptToList();
            if (objExpandedUserDTO == null)
            {
                return HttpNotFound();
            }
            return View(objExpandedUserDTO);

            //try
            //{
            //    MasterRepository MasterRepository = new MasterRepository();
            //    ViewBag.IdLevel = GetAllLevelAsSelectList();
            //    ViewBag.IdGroup = GetAllGroupAsSelectList();
            //    var Result = MasterRepository.GetListUsers(UserName);

            //    return View(Result);
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError(string.Empty, "Error: " + ex);
            //    List<ExpandedUserDTO> Result = new List<ExpandedUserDTO>();

            //    return View(Result);
            //}
        }
        #endregion

        // PUT: /User/EditUser
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region public ActionResult EditUser(ExpandedUserDTO paramExpandedUserDTO)
        public ActionResult EditUser(ExpandedUserDTO paramExpandedUserDTO)
        {
            try
            {
                if (paramExpandedUserDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ExpandedUserDTO objExpandedUserDTO = UpdateDTOUser(paramExpandedUserDTO);

                if (objExpandedUserDTO == null)
                {
                    return HttpNotFound();
                }

                return Redirect("~/User/Index");
            }
            catch (Exception ex)
            {
                ViewBag.IdDept = GetDeptToList();
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditUser", GetUser(paramExpandedUserDTO.UserName));
            }
        }
        #endregion

        // DELETE: /User/DeleteUser
        [Authorize(Roles = "Administrator")]
        #region public ActionResult DeleteUser(string UserName)
        public ActionResult DeleteUser(string UserName)
        {
            try
            {
                if (UserName == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (UserName.ToLower() == this.User.Identity.Name.ToLower())
                {
                    ModelState.AddModelError(
                        string.Empty, "Error: Cannot delete the current user");

                    return View("EditUser");
                }

                ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);

                if (objExpandedUserDTO == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    DeleteUser(objExpandedUserDTO);
                }

                return Redirect("~/User/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditUser", GetUser(UserName));
            }
        }
        #endregion

        // GET: /User/EditRoles/TestUser 
        [Authorize(Roles = "Administrator")]
        #region ActionResult EditRoles(string UserName)
        public ActionResult EditRoles(string UserName)
        {
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserName = UserName.ToLower();

            // Check that we have an actual user
            ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);

            if (objExpandedUserDTO == null)
            {
                return HttpNotFound();
            }

            UserAndRolesDTO objUserAndRolesDTO =
                GetUserAndRoles(UserName);

            return View(objUserAndRolesDTO);
        }
        #endregion

        // PUT: /User/EditRoles/TestUser 
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region public ActionResult EditRoles(UserAndRolesDTO paramUserAndRolesDTO)
        public ActionResult EditRoles(UserAndRolesDTO paramUserAndRolesDTO)
        {
            try
            {
                if (paramUserAndRolesDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                string UserName = paramUserAndRolesDTO.UserName;
                string strNewRole = Convert.ToString(Request.Form["AddRole"]);

                if (strNewRole != "No Roles Found")
                {
                    // Go get the User
                    ApplicationUser user = UserManager.FindByName(UserName);

                    // Put user in role
                    UserManager.AddToRole(user.Id, strNewRole);
                }

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                UserAndRolesDTO objUserAndRolesDTO =
                    GetUserAndRoles(UserName);

                return View(objUserAndRolesDTO);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditRoles");
            }
        }
        #endregion

        // DELETE: /User/DeleteRole?UserName="TestUser&RoleName=Administrator
        [Authorize(Roles = "Administrator")]
        #region public ActionResult DeleteRole(string UserName, string RoleName)
        public ActionResult DeleteRole(string UserName, string RoleName)
        {
            try
            {
                if ((UserName == null) || (RoleName == null))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                UserName = UserName.ToLower();

                // Check that we have an actual user
                ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);

                if (objExpandedUserDTO == null)
                {
                    return HttpNotFound();
                }

                if (UserName.ToLower() ==
                    this.User.Identity.Name.ToLower() && RoleName == "Administrator")
                {
                    ModelState.AddModelError(string.Empty,
                        "Error: Cannot delete Administrator Role for the current user");
                }

                // Go get the User
                ApplicationUser user = UserManager.FindByName(UserName);
                // Remove User from role
                UserManager.RemoveFromRoles(user.Id, RoleName);
                UserManager.Update(user);

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                return RedirectToAction("EditRoles", new { UserName = UserName });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                UserAndRolesDTO objUserAndRolesDTO =
                    GetUserAndRoles(UserName);

                return View("EditRoles", objUserAndRolesDTO);
            }
        }
        #endregion

        // Roles *****************************

        // GET: /User/ViewAllRoles
        [Authorize(Roles = "Administrator")]
        #region public ActionResult ViewAllRoles()
        public ActionResult ViewAllRoles()
        {
            var roleManager =
                new RoleManager<IdentityRole>
                (
                    new RoleStore<IdentityRole>(new ApplicationDbContext())
                    );

            List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles
                                        select new RoleDTO
                                        {
                                            Id = objRole.Id,
                                            RoleName = objRole.Name
                                        }).ToList();

            return View(colRoleDTO);
        }
        #endregion

        // GET: /User/AddRole
        [Authorize(Roles = "Administrator")]
        #region public ActionResult AddRole()
        public ActionResult AddRole()
        {
            
            RoleDTO objRoleDTO = new RoleDTO();
            this.AddToastMessage("Congratulations", "You have implemented a Government Toast", ToastType.Success);
            return View(objRoleDTO);
        }
        #endregion

        // PUT: /User/AddRole
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region public ActionResult AddRole(RoleDTO paramRoleDTO)
        public ActionResult AddRole(RoleDTO paramRoleDTO)
        {
            try
            {
                if (paramRoleDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var RoleName = paramRoleDTO.RoleName.Trim();

                if (RoleName == "")
                {
                    throw new Exception("No RoleName");
                }

                // Create Role
                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext())
                        );

                if (!roleManager.RoleExists(RoleName))
                {
                    roleManager.Create(new IdentityRole(RoleName));
                }

                return Redirect("~/User/ViewAllRoles");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("AddRole");
            }
        }
        #endregion

        // DELETE: /User/DeleteUserRole?RoleName=TestRole
        [Authorize(Roles = "Administrator")]
        #region public ActionResult DeleteUserRole(string RoleName)
        public ActionResult DeleteUserRole(string RoleName)
        {
            try
            {
                if (RoleName == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (RoleName.ToLower() == "Administrator")
                {
                    throw new Exception(String.Format("Cannot delete {0} Role.", RoleName));
                }

                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext()));

                var UsersInRole = roleManager.FindByName(RoleName).Users.Count();
                if (UsersInRole > 0)
                {
                    throw new Exception(
                        String.Format(
                            "Canot delete {0} Role because it still has users.",
                            RoleName)
                            );
                }

                var objRoleToDelete = (from objRole in roleManager.Roles
                                       where objRole.Name == RoleName
                                       select objRole).FirstOrDefault();
                if (objRoleToDelete != null)
                {
                    roleManager.Delete(objRoleToDelete);
                }
                else
                {
                    throw new Exception(
                        String.Format(
                            "Canot delete {0} Role does not exist.",
                            RoleName)
                            );
                }

                List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles
                                            select new RoleDTO
                                            {
                                                Id = objRole.Id,
                                                RoleName = objRole.Name
                                            }).ToList();

                return View("ViewAllRoles", colRoleDTO);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);

                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext()));

                List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles
                                            select new RoleDTO
                                            {
                                                Id = objRole.Id,
                                                RoleName = objRole.Name
                                            }).ToList();

                return View("ViewAllRoles", colRoleDTO);
            }
        }
        #endregion


        // Utility

        #region public ApplicationUserManager UserManager
        public IPMI.ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                    HttpContext.GetOwinContext()
                    .GetUserManager<IPMI.ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion

        #region public ApplicationRoleManager RoleManager
        public IPMI.ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ??
                    HttpContext.GetOwinContext()
                    .GetUserManager<IPMI.ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion

        #region private List<SelectListItem> GetAllRolesAsSelectList()
        private List<SelectListItem> GetAllRolesAsSelectList()
        {
            List<SelectListItem> SelectRoleListItems =
                new List<SelectListItem>();

            var roleManager =
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var colRoleSelectList = roleManager.Roles.OrderBy(x => x.Name).ToList();

            SelectRoleListItems.Add(
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                });

            foreach (var item in colRoleSelectList)
            {
                SelectRoleListItems.Add(
                    new SelectListItem
                    {
                        Text = item.Name.ToString(),
                        Value = item.Name.ToString()
                    });
            }

            return SelectRoleListItems;
        }

        private List<SelectListItem> GetAllLevelAsSelectList()
        {
            List<SelectListItem> SelectLevelListItems =
                new List<SelectListItem>();

            MasterRepository MasterRepository = new MasterRepository();
            var colRoleSelectList = MasterRepository.GetLevels();

            SelectLevelListItems.Add(
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                });

            foreach (var item in colRoleSelectList)
            {
                SelectLevelListItems.Add(
                    new SelectListItem
                    {
                        Text = item.Text.ToString(),
                        Value = item.Value.ToString()
                    });
            }

            return SelectLevelListItems;
        }

        private List<SelectListItem> GetAllGroupAsSelectList()
        {
            List<SelectListItem> SelectGroupListItems =
                new List<SelectListItem>();

            MasterRepository MasterRepository = new MasterRepository();
            var colRoleSelectList = MasterRepository.GetGroups();

            SelectGroupListItems.Add(
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                });

            foreach (var item in colRoleSelectList)
            {
                SelectGroupListItems.Add(
                    new SelectListItem
                    {
                        Text = item.Text.ToString(),
                        Value = item.Value.ToString()
                    });
            }

            return SelectGroupListItems;
        }

        private List<SelectListItem> GetDeptToList()
        {
            List<SelectListItem> SelectGroupListItems =
                new List<SelectListItem>();

            MasterRepository MasterRepository = new MasterRepository();
            var colRoleSelectList = MasterRepository.GetDept();

            SelectGroupListItems.Add(
                new SelectListItem
                {
                    Text = "PILIH DEPARTEMEN",
                    Value = "0"
                });

            foreach (var item in colRoleSelectList)
            {
                SelectGroupListItems.Add(
                    new SelectListItem
                    {
                        Text = item.Text.ToString(),
                        Value = item.Value.ToString()
                    });
            }

            return SelectGroupListItems;
        }
        private List<SelectListItem> GetAllPartnerAsSelectList()
        {
            List<SelectListItem> SelectGroupListItems =
                new List<SelectListItem>();

            MasterRepository MasterRepository = new MasterRepository();
            var colRoleSelectList = MasterRepository.GetPartner(); ;

            SelectGroupListItems.Add(
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                });

            foreach (var item in colRoleSelectList)
            {
                SelectGroupListItems.Add(
                    new SelectListItem
                    {
                        Text = item.Text.ToString(),
                        Value = item.Value.ToString()
                    });
            }

            return SelectGroupListItems;
        }

        public JsonResult GetPartner(string Id)
        {
            MasterRepository MasterRepository = new MasterRepository();
            var result = MasterRepository.GetDetailById(Id);

            if (result != null)
            {
                return new JsonResult { Data = result[0], JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return null;
        }

        #endregion

        #region private ExpandedUserDTO GetUser(string paramUserName)
        private ExpandedUserDTO GetUser(string paramUserName)
        {
            ExpandedUserDTO objExpandedUserDTO = new ExpandedUserDTO();

            var result = UserManager.FindByName(paramUserName);

            // If we could not find the user, throw an exception
            if (result == null) throw new Exception("Could not find the User");

            objExpandedUserDTO.UserName = result.UserName;
            objExpandedUserDTO.NameIdentifier = result.NameIdentifier;
            objExpandedUserDTO.Email = result.Email;
            objExpandedUserDTO.LockoutEndDateUtc = result.LockoutEndDateUtc;
            objExpandedUserDTO.AccessFailedCount = result.AccessFailedCount;
            objExpandedUserDTO.PhoneNumber = result.PhoneNumber;
            objExpandedUserDTO.IdDept = result.IdDept;
            return objExpandedUserDTO;
        }
        #endregion
        private RoleDTO GetRole(string RoleName)
        {
            RoleDTO objRoleDTO = new RoleDTO();

            var result = RoleManager.FindByName(RoleName);

            // If we could not find the user, throw an exception
            if (result == null) throw new Exception("Could not find the User");

            objRoleDTO.Id = result.Id;
            objRoleDTO.RoleName = result.Name;



            return objRoleDTO;
        }
        #region private ExpandedUserDTO UpdateDTOUser(ExpandedUserDTO objExpandedUserDTO)
        private ExpandedUserDTO UpdateDTOUser(ExpandedUserDTO paramExpandedUserDTO)
        {
            ApplicationUser result =
                UserManager.FindByName(paramExpandedUserDTO.UserName);

            // If we could not find the user, throw an exception
            if (result == null)
            {
                throw new Exception("Could not find the User");
            }

            result.NameIdentifier = paramExpandedUserDTO.NameIdentifier;
            result.Email = paramExpandedUserDTO.Email;
            //result.IdLevel = paramExpandedUserDTO.IdLevel;
            result.IdDept = paramExpandedUserDTO.IdDept;
            //result.PartnerID = paramExpandedUserDTO.PartnerID;

            // Lets check if the account needs to be unlocked
            if (UserManager.IsLockedOut(result.Id))
            {
                // Unlock user
                UserManager.ResetAccessFailedCountAsync(result.Id);
            }

            UserManager.Update(result);

            // Was a password sent across?
            if (!string.IsNullOrEmpty(paramExpandedUserDTO.Password))
            {
                // Remove current password
                var removePassword = UserManager.RemovePassword(result.Id);
                if (removePassword.Succeeded)
                {
                    // Add new password
                    var AddPassword =
                        UserManager.AddPassword(
                            result.Id,
                            paramExpandedUserDTO.Password
                            );

                    if (AddPassword.Errors.Count() > 0)
                    {
                        throw new Exception(AddPassword.Errors.FirstOrDefault());
                    }
                }
            }

            return paramExpandedUserDTO;
        }
        #endregion

        #region private void DeleteUser(ExpandedUserDTO paramExpandedUserDTO)
        private void DeleteUser(ExpandedUserDTO paramExpandedUserDTO)
        {
            ApplicationUser user =
                UserManager.FindByName(paramExpandedUserDTO.UserName);

            // If we could not find the user, throw an exception
            if (user == null)
            {
                throw new Exception("Could not find the User");
            }

            UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());
            UserManager.Update(user);
            UserManager.Delete(user);
        }
        #endregion

        #region private UserAndRolesDTO GetUserAndRoles(string UserName)
        private UserAndRolesDTO GetUserAndRoles(string UserName)
        {
            // Go get the User
            ApplicationUser user = UserManager.FindByName(UserName);

            List<UserRoleDTO> colUserRoleDTO =
                (from objRole in UserManager.GetRoles(user.Id)
                 select new UserRoleDTO
                 {
                     RoleName = objRole,
                     UserName = UserName
                 }).ToList();

            if (colUserRoleDTO.Count() == 0)
            {
                colUserRoleDTO.Add(new UserRoleDTO { RoleName = "No Roles Found" });
            }

            ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

            // Create UserRolesAndPermissionsDTO
            UserAndRolesDTO objUserAndRolesDTO =
                new UserAndRolesDTO();
            objUserAndRolesDTO.UserName = UserName;
            objUserAndRolesDTO.colUserRoleDTO = colUserRoleDTO;
            return objUserAndRolesDTO;
        }
        #endregion

        #region private List<string> RolesUserIsNotIn(string UserName)
        private List<string> RolesUserIsNotIn(string UserName)
        {
            // Get roles the user is not in
            var colAllRoles = RoleManager.Roles.Select(x => x.Name).ToList();

            // Go get the roles for an individual
            ApplicationUser user = UserManager.FindByName(UserName);

            // If we could not find the user, throw an exception
            if (user == null)
            {
                throw new Exception("Could not find the User");
            }

            var colRolesForUser = UserManager.GetRoles(user.Id).ToList();
            var colRolesUserInNotIn = (from objRole in colAllRoles
                                       where !colRolesForUser.Contains(objRole)
                                       select objRole).ToList();

            if (colRolesUserInNotIn.Count() == 0)
            {
                colRolesUserInNotIn.Add("No Roles Found");
            }

            return colRolesUserInNotIn;
        }
        #endregion
    }
}