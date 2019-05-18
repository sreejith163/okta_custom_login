using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACRLoginPortal.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using ACRLoginPortal.Models;
using Microsoft.AspNetCore.Authentication;

namespace ACRLoginPortal.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ProfileController : Controller
    {
        DataProtector dp;
        private readonly IOptions<OktaConfig> _Config;

        public ProfileController(IOptions<OktaConfig> config, IDataProtectionProvider provider)
        {
            _Config = config;
            dp = new DataProtector(provider, "ACRProtect");
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(Request.Query["appUrl"]) && string.IsNullOrEmpty(Request.Query["operation"]))
            {
                TempData["Message"] = "Sorry something went wrong, please try again!"; //"Required parameters not found, please initiate the request from the application.";
                return View("~/Views/Error.cshtml");
            }

            if (!Uri.IsWellFormedUriString(Request.Query["appUrl"].ToString(), UriKind.Absolute))
            {
                TempData["Message"] = "Sorry something went wrong, please try again!"; //"path parameter is not a valid Url, please initiate the request from the application.";
                return View("~/Views/Error.cshtml");
            }

            string operation = Request.Query["operation"].ToString().ToUpper();

            string encryptedStr = dp.ProtectStr(Request.Query["appUrl"].ToString());

            if (operation == "EDIT_PROFILE")
                return RedirectToAction("EditProfile", "Profile", new { key = encryptedStr });
            else if (operation == "CHANGE_PWD")
                return RedirectToAction("ChangePassword", "Profile", new { key = encryptedStr });
            else
            {
                TempData["Message"] = "Sorry something went wrong, please try again!"; //"operation value is not valid, please initiate the request from the application.";
                return View("~/Views/Error.cshtml");
            }
        }

        [HttpGet]
        public async Task<IActionResult> BackToApp()
        {
            if(User.Identity.IsAuthenticated)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");

                OktaHelper oktaHelper = new OktaHelper(_Config);
                var response = await oktaHelper.RevokeToken(accessToken);

                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }
            }

            string appUrl = dp.UnprotectStr(Request.Query["key"].ToString());

            if (!Uri.IsWellFormedUriString(appUrl, UriKind.Absolute))
            {
                TempData["Message"] = "Sorry something went wrong, please try again!"; //"No valid Url detected to redirect, please initiate the request from the application.";
                return View("~/Views/Error.cshtml");
            }
            else
                return Redirect(appUrl);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfile(string key)
        {
            string userName = User.Claims
               .FirstOrDefault(x => x.Type == "preferred_username")
               ?.Value.ToString();

            OktaHelper oktaHelper = new OktaHelper(_Config);
            var result = await oktaHelper.GetOktaUser(userName);
            if (result.IsSuccessStatusCode)
            {
                JObject user = oktaHelper.GetJsonObject(result);
                OktaUserModel oktaUser = user.ToObject<OktaUserModel>();

                oktaUser.Key = key;
                oktaUser.profile.login = userName;
                return View($"~/Views/Profile/EditProfile.cshtml", oktaUser);
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(OktaUserModel oktaUser)
        {
            if (ModelState.IsValid)
            {
                OktaHelper oktaHelper = new OktaHelper(_Config);
                var result = await oktaHelper.UpdateUser(oktaUser);
                if (result.IsSuccessStatusCode)
                {
                    //TempData["Message"] = "Profile is updated successfully.";
                    return RedirectToAction("EditProfileMessage", "Profile", new { key = oktaUser.Key, status = "success" });
                }
                else
                {
                    ModelState.AddModelError("Error", result.ReasonPhrase);
                    return View($"~/Views/Profile/EditProfile.cshtml", oktaUser);
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Sorry, we found some errors. Please review the form and make corrections.");
                return View($"~/Views/Profile/EditProfile.cshtml", oktaUser);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProfileMessage()
        {
            if (User.Identity.IsAuthenticated)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");

                OktaHelper oktaHelper = new OktaHelper(_Config);
                var response = await oktaHelper.RevokeToken(accessToken);

                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }
            }

            if (Request.Query["status"].ToString().ToUpper() == "SUCCESS")
                ViewData["Message"] = "Profile is updated successfully. Please click the below link to go back to the application";

            ViewBag.Key = Request.Query["key"];
            return View($"~/Views/Profile/EditProfileMessage.cshtml");

        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword(string key)
        {
            string userName = User.Claims
               .FirstOrDefault(x => x.Type == "preferred_username")
               ?.Value.ToString();
            ChangePasswordModel changePasswordModel = new ChangePasswordModel() { Key = key, UserId = userName };
            return View($"~/Views/Profile/ChangePassword.cshtml", changePasswordModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (ModelState.IsValid)
            {
                OktaHelper oktaHelper = new OktaHelper(_Config);
                var result = await oktaHelper.ChangePassword(changePasswordModel);
                if (result.IsSuccessStatusCode)
                {
                    //TempData["Message"] = "Password was changed successfully";
                    return RedirectToAction("ChangePasswordMessage", "Profile", new { key = changePasswordModel.Key, status = "success" });
                }
                else
                {
                    var error = oktaHelper.GetJsonObject(result);
                    ModelState.AddModelError("Error", error["errorCauses"][0]["errorSummary"].Value<string>());
                    //ModelState.AddModelError("Error", result.ReasonPhrase);
                    return View($"~/Views/Profile/ChangePassword.cshtml", changePasswordModel);
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Sorry, we found some errors. Please review the form and make corrections.");
                return View($"~/Views/Profile/ChangePassword.cshtml", changePasswordModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangePasswordMessage()
        {
            if (User.Identity.IsAuthenticated)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");

                OktaHelper oktaHelper = new OktaHelper(_Config);
                var response = await oktaHelper.RevokeToken(accessToken);

                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }
            }

            if (Request.Query["status"].ToString().ToUpper() == "SUCCESS")
                ViewData["Message"] = "Password is changed successfully. Please click the below link to go back to the application";

            ViewBag.Key = Request.Query["key"];
            return View($"~/Views/Profile/ChangePasswordMessage.cshtml");

        }
    }
}