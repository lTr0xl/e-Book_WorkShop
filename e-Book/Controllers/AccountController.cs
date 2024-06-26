﻿using e_Book.Data;
using e_Book.Interfaces;
using e_Book.Models;
using e_Book.Repository;
using e_Book.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace e_Book.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IAccountRepository _accountRepository;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context,
            IAccountRepository accountRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        private void MapUserEdit(AppUser user, EditUserProfileViewModel editVM)
        {
            user.Id = editVM.Id;
            user.UserName = editVM.Username;
        }
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if(user != null)
            {
                //User is found, check password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);  
                if (passwordCheck)
                {
                    //Password correct, sign in
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Book");
                    }
                }
                //Password is incorrect
                TempData["Error"] = "Wrong password. Please, try again";
                return View(loginVM);
            }
            //User not found
            TempData["Error"] = "Wrong email address. Please, try again";
            return View(loginVM);
        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            var newUser = new AppUser()
            {
                Email = registerVM.EmailAddress,
                UserName = registerVM.UserName,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            return RedirectToAction("Index", "Book");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Book");
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _accountRepository.GetCurrentUserId();
            var user = await _accountRepository.GetUserById(curUserId);
            if (user == null)
            {
                return View("Error");
            }
            var editUserProfileVM = new EditUserProfileViewModel()
            {
                Id = curUserId,
                Username = user.UserName,
            };
            return View(editUserProfileVM);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserProfileViewModel editVM)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile",editVM);
            }

            AppUser user = await _accountRepository.GetUserByIdNoTracking(editVM.Id);
            if(!await _userManager.CheckPasswordAsync(user, editVM.CurrentPassword))
            {
                TempData["Error"] = "Wrong password";
                return RedirectToAction("EditUserProfile", editVM);
            }

            if(editVM.NewPassword != null)
            {
                var newPass = await _userManager.ChangePasswordAsync(user, editVM.CurrentPassword, editVM.NewPassword);
                if (!newPass.Succeeded)
                {
                    TempData["Error"] = "Failed changing password";
                    return RedirectToAction("EditUserProfile", editVM);
                }
            }
            
            MapUserEdit(user, editVM);
            _accountRepository.Update(user);
            return RedirectToAction("Index", "Book");
        }
    }
}
