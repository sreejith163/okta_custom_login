using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using okta_custom_login.Helpers;
using okta_custom_login.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net;

namespace okta_custom_login.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class AccountController : Controller
    {
        DataProtector dp;
        private readonly IOptions<OktaConfig> _Config;

        public AccountController(IOptions<OktaConfig> config, IDataProtectionProvider provider)
        {
            _Config = config;
            dp = new DataProtector(provider, "ACRProtect");
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(Request.Query["path"]))
            {
                TempData["Message"] = "Sorry something went wrong, please try again!"; //"Required parameters not found, please initiate the request from the application.";
                return View("~/Views/Error.cshtml");
            }

            if(!Uri.IsWellFormedUriString(Request.Query["path"].ToString(), UriKind.Absolute))
            {
                TempData["Message"] = "Sorry something went wrong, please try again!"; //"path parameter is not a valid Url, please initiate the request from the application.";
                return View("~/Views/Error.cshtml");
            }

            string encryptedStr = dp.ProtectStr(Request.Query["path"].ToString());

            return RedirectToAction("Login", "Account", new { key = encryptedStr });
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginModel login = new LoginModel() { RememberMe = false, Key = Request.Query["key"] };

            ViewBag.OktaDomain = _Config.Value.Okta_OrgUri;
            return View($"~/Views/Account/Login.cshtml", login);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            string returnUrl = "";
            string path = dp.UnprotectStr(login.Key);

            if (!Uri.IsWellFormedUriString(path, UriKind.Absolute))
            {
                TempData["Message"] = "Sorry something went wrong, please try again!"; //"No valid Url detected to redirect, please initiate the request from the application.";
                return View("~/Views/Error.cshtml");
            }

            if (login.IsOktaSessionExists)
            {
                if (path.Contains("?"))
                    returnUrl = $"{path}&isAuthenticated=true";
                else
                    returnUrl = $"{path}?isAuthenticated=true";

                return Redirect(returnUrl);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    OktaHelper oktaHelper = new OktaHelper(_Config);
                    HttpResponseMessage response = await oktaHelper.Login(login);
                    if (response.IsSuccessStatusCode)
                    {
                        JObject jObj = oktaHelper.GetJsonObject(response);
                        if (jObj["status"] != null)
                        {
                            var status = jObj["status"].Value<string>();
                            switch (status)
                            {
                                case "SUCCESS":
                                    var sessionToken = jObj["sessionToken"].Value<string>();

                                    if (path.Contains("?"))
                                        returnUrl = WebUtility.UrlEncode($"{path}&isAuthenticated=true");
                                    else
                                        returnUrl = WebUtility.UrlEncode($"{path}?isAuthenticated=true");

                                    return Redirect($"{_Config.Value.Okta_OrgUri}/login/sessionCookieRedirect?token={sessionToken}&redirectUrl={returnUrl}");

                                case "PASSWORD_EXPIRED":
                                    return ChangePassword(login.Key, jObj["_embedded"]["user"]["id"].Value<string>());
                                default:
                                    break;
                            }
                            ModelState.AddModelError("Error", "Invalid email or password.");
                            return View($"~/Views/Account/Login.cshtml", login);
                        }
                        else
                        {
                            ModelState.AddModelError("Error", "Invalid email or password.");
                            return View($"~/Views/Account/Login.cshtml", login);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Invalid email or password.");
                        return View($"~/Views/Account/Login.cshtml", login);
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "Sorry, we found some errors. Please review the form and make corrections.");
                    return View($"~/Views/Account/Login.cshtml", login);
                }
            }
        }
        
        [HttpGet]
        public IActionResult ChangePassword(string key, string userId)
        {
            ChangePasswordModel changePasswordModel = new ChangePasswordModel() { Key = key, UserId = userId};
            return View($"~/Views/Account/ChangePassword.cshtml", changePasswordModel);
        }

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
                    return RedirectToAction("ChangePasswordMessage", "Account", new { key = changePasswordModel.Key, status = "success"});
                }
                else
                {
                    var error = oktaHelper.GetJsonObject(result);
                    ModelState.AddModelError("Error", error["errorCauses"][0]["errorSummary"].Value<string>());
                    //ModelState.AddModelError("Error", result.ReasonPhrase);
                    return View($"~/Views/Account/ChangePassword.cshtml", changePasswordModel);
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Sorry, we found some errors. Please review the form and make corrections.");
                return View($"~/Views/Account/ChangePassword.cshtml", changePasswordModel);
            }
        }

        [HttpGet]
        public IActionResult ChangePasswordMessage()
        {
            if (Request.Query["status"].ToString().ToUpper() == "SUCCESS")
                ViewData["Message"] = "Password is changed successfully. Please click the below link to login";

            ViewBag.Key = Request.Query["key"];
            return View($"~/Views/Account/ChangePasswordMessage.cshtml");

        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            ForgotPasswordModel reactivation = new ForgotPasswordModel() { Key = Request.Query["key"] };

            return View($"~/Views/Account/ForgotPassword.cshtml", reactivation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel reactivation)
        {
            if (ModelState.IsValid)
            {
                OktaHelper oktaHelper = new OktaHelper(_Config);
                HttpResponseMessage response = await oktaHelper.ForgotPassword(reactivation.Email, false);
                if (response.IsSuccessStatusCode)
                {
                    JObject jObj = oktaHelper.GetJsonObject(response);// (JObject)JsonConvert.DeserializeObject(strJson);
                    var url = jObj["resetPasswordUrl"].Value<string>().ToString().Split('/');
                    var token = url[url.Length - 1];

                    try
                    {
                        var resetPasswordLink = $"{Request.Scheme}://{Request.Host}/Account/ResetPassword?token={token}&key={reactivation.Key}";
                        //Body = $"Please click the link to reset password.<a href='{Request.Scheme}://{Request.Host}/Account/ResetPassword?token={token}&key={reactivation.Key}'> Click here </a>",
                        EmailService.SendEmail(new EmailModel
                        {
                            Subject = "Account password reset",
                            To = new List<string>() { reactivation.Email },
                            Body = EmailService.PasswordResetEmailBody(reactivation.Email, resetPasswordLink),
                            IsHtml = true,
                            SMTPServer = _Config.Value.SMTP_Server,
                            SMTPPort = Convert.ToInt32(_Config.Value.SMTP_Port),
                            SMTPUser = _Config.Value.SMTP_Username,
                            SMTPPassword = _Config.Value.SMTP_Password
                        });
                        //TempData["Message"] = "Password reset link has been sent to your email adress.";

                        return RedirectToAction("ForgotPasswordMessage", "Account", new { key = reactivation.Key, status = "success" });
                    }
                    catch
                    {
                        //TempData["Message"] = "Email Delivery Failed.";
                        return RedirectToAction("ForgotPasswordMessage", "Account", new { key = reactivation.Key, status = "fail" });
                    }
                }
                else
                {
                    //TempData["Message"] = "Password reset link has been sent to your email adress.";
                    return RedirectToAction("ForgotPasswordMessage", "Account", new { key = reactivation.Key, status = "success" });
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Sorry, we found some errors. Please review the form and make corrections.");
                return View($"~/Views/Account/ForgotPassword.cshtml", reactivation);
            }
        }

        [HttpGet]
        public IActionResult ForgotPasswordMessage()
        {
            if (Request.Query["status"].ToString().ToUpper() == "SUCCESS")
                ViewData["Message"] = "Password reset link has been sent to your email adress. Please check your inbox";

            if (Request.Query["status"].ToString().ToUpper() == "FAIL")
                ViewData["Message"] = "Email Delivery Failed.";

            if (Request.Query["status"].ToString().ToUpper() == "ERROR")
                ViewData["Message"] = "User has been already activated. Please use Forget Password instead";

            ViewBag.Key = Request.Query["key"];
            return View($"~/Views/Account/ForgotPasswordMessage.cshtml");

        }

        [HttpGet]
        public async Task<ActionResult> ResetPassword(string token)
        {
            string key = Request.Query["key"];

            OktaHelper oktaHelper = new OktaHelper(_Config);
            HttpResponseMessage response = await oktaHelper.VerifyRecoveryToken(token);

            if (response.IsSuccessStatusCode)
            {
                var jObj = oktaHelper.GetJsonObject(response);
                var userId = jObj["_embedded"]["user"]["id"].Value<string>();
                ResetPasswordModel resetPassword = new ResetPasswordModel() { UserId = userId, Key = Request.Query["key"] };
                return View($"~/Views/Account/ResetPassword.cshtml", resetPassword);
            }
            else
            {
                var jObj = oktaHelper.GetJsonObject(response);
                string errorMessage = jObj["errorSummary"].Value<string>();
                TempData["Message"] = errorMessage;

                return RedirectToAction("ResendActivationLink", new { key = Request.Query["key"] });

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel resetPassword)
        {
            if (ModelState.IsValid)
            {
                OktaHelper oktaHelper = new OktaHelper(_Config);
                HttpResponseMessage response = await oktaHelper.ResetPassword(resetPassword);
                if (response.IsSuccessStatusCode)
                {
                    //TempData["Message"] = "Password reset Success. Please click Sign In link below to log in.";
                    return RedirectToAction("ResetPasswordMessage", "Account", new { key = resetPassword.Key, status = "success" });
                }
                else
                {
                    JObject jObj = oktaHelper.GetJsonObject(response);
                    string errMsg = jObj["errorCauses"][0]["errorSummary"].Value<string>().Replace("Password requirements were not met. ", "");
                    ModelState.AddModelError("Error", errMsg);

                    return View($"~/Views/Account/ResetPassword.cshtml", resetPassword);
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Sorry, we found some errors. Please review the form and make corrections.");
                return View($"~/Views/Account/ResetPassword.cshtml", resetPassword);
            }
        }

        [HttpGet]
        public IActionResult ResetPasswordMessage()
        {
            if (Request.Query["status"].ToString().ToUpper() == "SUCCESS")
                ViewData["Message"] = "Password reset success. Please click on the below link to login";

            ViewBag.Key = Request.Query["key"];
            return View($"~/Views/Account/ResetPasswordMessage.cshtml");
        }

        [HttpGet]
        public ActionResult ResendActivationLink()
        {
            ForgotPasswordModel reactivation = new ForgotPasswordModel() { Key = Request.Query["key"]};

            return View($"~/Views/Account/ResendActivationLink.cshtml", reactivation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResendActivationLink(ForgotPasswordModel reactivation)
        {
            if (ModelState.IsValid)
            {
                OktaHelper oktaHelper = new OktaHelper(_Config);
                HttpResponseMessage response = await oktaHelper.SearchUserByEmail(reactivation.Email);
                if (response.IsSuccessStatusCode)
                {
                    JArray jArr = oktaHelper.GetJsonArray(response);
                    if (jArr.Count == 1)
                    {
                        string userId = jArr[0]["id"].Value<string>();
                        response = await oktaHelper.ReactivateUser(userId);
                        if (response.IsSuccessStatusCode)
                        {
                            JObject jobj = oktaHelper.GetJsonObject(response);
                            var token = jobj["activationToken"].Value<string>().ToString();
                            try
                            {
                                var resetPasswordLink = $"{Request.Scheme}://{Request.Host}/Account/ResetPassword?token={token}&key={reactivation.Key}";
                                //Body = $"Please click the link to reset password.<a href='{Request.Scheme}://{Request.Host}/Account/ResetPassword?token={token}&key={reactivation.Key}'> Click here </a>",
                                EmailService.SendEmail(new EmailModel
                                {
                                    Subject = "Account password reset",
                                    To = new List<string>() { reactivation.Email },
                                    Body = EmailService.PasswordResetEmailBody(reactivation.Email, resetPasswordLink),
                                    IsHtml = true,
                                    SMTPServer = _Config.Value.SMTP_Server,
                                    SMTPPort = Convert.ToInt32(_Config.Value.SMTP_Port),
                                    SMTPUser = _Config.Value.SMTP_Username,
                                    SMTPPassword = _Config.Value.SMTP_Password
                                });
                                //TempData["Message"] = "Password reset link has been sent to your email adress.";

                                return RedirectToAction("ForgotPasswordMessage", new { key = reactivation.Key, status = "success" });

                            }
                            catch
                            {
                                //TempData["Message"] = "Email Delivery Failed";
                                return RedirectToAction("ForgotPasswordMessage", new { key = reactivation.Key, status = "fail" });
                            }
                        }
                        else
                        {
                            //TempData["Message"] = "User has been already activated. Please use Forget Password instead.";
                            return RedirectToAction("ForgotPasswordMessage", new { key = reactivation.Key, status = "error" });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Invalid email");

                        return View($"~/Views/Account/ResendActivationLink.cshtml", reactivation);
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "Invalid email");

                    return View($"~/Views/Account/ResendActivationLink.cshtml", reactivation);
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Sorry, we found some errors. Please review the form and make corrections.");

                return View($"~/Views/Account/ResendActivationLink.cshtml", reactivation);
            }
        }
    }
}