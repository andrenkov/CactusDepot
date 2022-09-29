using CactusDepot.Shared;
using CactusDepot.Shared.Models.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CactusDepot.Seeds.Controllers.Administration
{
    [Authorize(Roles = "Administrator,SuperAdmin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger<AdministrationController> logger;
        public AdministrationController(RoleManager<IdentityRole> _roleManager, UserManager<IdentityUser> _userManager, ILogger<AdministrationController> _logger)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            logger = _logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            SharedUtil.WriteLogToConsole("AdministrationController", "GET.AccessDenied()");
            return View();
        }

        #region Edit Roles
        [HttpGet]
        public IActionResult CreateRole()
        {
            SharedUtil.WriteLogToConsole("AdministrationController", "GET.CreateRole()");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                SharedUtil.WriteLogToConsole("AdministrationController", "POST.CreateRole()");
                IdentityRole identityRole = new()
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                //if error
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            IQueryable<IdentityRole> roles = roleManager.Roles;
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string Id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {Id} not found";
                return View("NotFound");
            }

            SharedUtil.WriteLogToConsole("AdministrationController", "GET.EditRole()");

            EditRoleModel model = new()
            {
                Id = role.Id,
                RoleName = role.Name
            };


            foreach (IdentityUser user in await userManager.GetUsersInRoleAsync(role.Name))
            {
                model.Users.Add(user.UserName);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleModel model)
        {
            IdentityRole role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} not found";
                return View("NotFound");
            }
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} not found";
                return View("NotFound");
            }
            else
            {
                SharedUtil.WriteLogToConsole("AdministrationController", "POST.EditRole()");
                role.Name = model.RoleName;
                IdentityResult result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            IdentityRole role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            SharedUtil.WriteLogToConsole("AdministrationController", "GET.EditUsersInRole()");

            List<UserRoleModel> model = new();
            foreach (IdentityUser user in await userManager.Users.ToListAsync())
            {
                UserRoleModel userRoleModel = new()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleModel.IsSelected = true;
                }
                else
                {
                    userRoleModel.IsSelected = false;
                }
                model.Add(userRoleModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleModel> model, string roleId)
        {
            IdentityRole role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMesage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }
            else
            {
                SharedUtil.WriteLogToConsole("AdministrationController", "POST.EditUsersInRole()");
                for (int i = 0; i < model.Count; i++)
                {
                    IdentityUser user = await userManager.FindByIdAsync(model[i].UserId);
                    IdentityResult result;
                    if (model[i].IsSelected && !await userManager.IsInRoleAsync(user, role.Name))
                    {
                        _ = await userManager.AddToRoleAsync(user, role.Name);
                    }
                    else
                    {
                        if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                        {
                            result = await userManager.RemoveFromRoleAsync(user, role.Name);
                        }
                        else
                        {
                            continue;//not selected and not in role
                        }

                        if (result.Succeeded)
                        {
                            if (i < (model.Count - 1))
                            {
                                continue;
                            }
                            else
                            {
                                return RedirectToAction("EditRole", new { Id = roleId });
                            }
                        }
                    }
                }
                return RedirectToAction("EditRole", new { Id = roleId });//if model is empty
            }

        } 
        #endregion

        #region Edit User
        [HttpGet]
        public IActionResult ListUsers()
        {
            IQueryable<IdentityUser> users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string Id)
        {
            IdentityUser user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                SharedUtil.WriteLogToConsole("AdministrationController", "GET.EditUser()");
                // GetClaimsAsync returns the list of user Claims
                IList<System.Security.Claims.Claim> userClaims = await userManager.GetClaimsAsync(user);
                // GetRolesAsync returns the list of user Roles
                IList<string> userRoles = await userManager.GetRolesAsync(user);

                EditUserModel model = new()
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Claims = userClaims.Select(c => c.Value).ToList(),
                    Roles = userRoles
                };

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserModel model)
        {
            IdentityUser user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                SharedUtil.WriteLogToConsole("AdministrationController", "POST.EditUser()");
                user.Email = model.Email;
                user.UserName = model.UserName;

                IdentityResult result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            IdentityUser user = await userManager.FindByIdAsync(id);
            if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                    return View("NotFound");
            }
            else
            {
                SharedUtil.WriteLogToConsole("AdministrationController", "POST.DeleteUser()");
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListUsers");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                SharedUtil.WriteLogToConsole("AdministrationController", "POST.DeleteRole()");
                try 
                {
                    IdentityResult result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ListRoles");
                }
                catch (DbUpdateException ex)
                {
                    //Log the exception to a file. We discussed logging to a file
                    // using Nlog in Part 63 of ASP.NET Core tutorial
                    logger.LogError($"Exception occurred deleting Role: {ex}");

                    // Pass the ErrorTitle and ErrorMessage that you want to show to
                    // the user using ViewBag. The Error view retrieves this data
                    // from the ViewBag and displays to the user.
                    ViewBag.ErrorTitle = $"{role.Name} is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users " + Environment.NewLine +
                        Environment.NewLine +
                        $"in this Role. If you want to delete this Role, please remove the users from "+
                        Environment.NewLine +
                        $"the Role and then try to delete."+
                        Environment.NewLine + Environment.NewLine +
                        $"Error: {ex.Message}";
                    return View("Error");
                }
            }
        }
        #endregion
        
        #region Manage User Roles
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            IdentityUser user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            SharedUtil.WriteLogToConsole("AdministrationController", "GET.ManageUserRoles()");
            List<UserRolesModel> model = new();

            foreach (IdentityRole role in await roleManager.Roles.ToListAsync())
            {
                UserRolesModel userRolesViewModel = new()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesModel> model, string userId)
        {
            IdentityUser user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            SharedUtil.WriteLogToConsole("AdministrationController", "POST.ManageUserRoles()");
            IList<string> roles = await userManager.GetRolesAsync(user);
            //clear all user roles first
            IdentityResult result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = userId });
        }
    } 
    #endregion
}
