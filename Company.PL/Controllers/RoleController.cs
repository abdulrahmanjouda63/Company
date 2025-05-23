﻿using Company.DAL.Models;
using Company.PL.Dtos;
using Company.PL.Helpers;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<RoleToReturnDto> roles;
            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManager.Roles.Select(R => new RoleToReturnDto()
                {
                    Id = R.Id,
                    Name = R.Name
                });
            }
            else
            {
                roles = _roleManager.Roles.Select(R => new RoleToReturnDto()
                {
                    Id = R.Id,
                    Name = R.Name
                }).Where(R => R.Name.Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase));
            }

            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(model.Name);
                if(role is null)
                {
                    role = new IdentityRole()
                    {
                        Name = model.Name
                    };
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound(new { statusCode = 404, Message = $"role with id : {id} is not found" });
            var dto = new RoleToReturnDto()
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(viewName, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id, string viewName = "Edit")
        {
            return await Details(id, viewName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturnDto model, string viewName = "Edit")
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Id");
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalid Id");
                var roleResult = await _roleManager.FindByNameAsync(model.Name);
                if (roleResult is null)
                {
                    role.Name = model.Name;
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                ModelState.AddModelError("", "Invalid Update Operation !!");
            }
            return View(viewName);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Edit(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Id");
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalid Id");
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Invalid Update Operation !!");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return BadRequest("Invalid Id");
            ViewData["RoleId"] = roleId;
            var usersInRole = new List<UsersInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }
                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return BadRequest("Invalid Id");
            if (ModelState.IsValid)
            {
                foreach(var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if(appUser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if(!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = roleId });
            }
            return View(users);
        }

    }
}
