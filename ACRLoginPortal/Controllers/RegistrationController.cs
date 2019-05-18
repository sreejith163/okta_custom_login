using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACRLoginPortal.Helpers;
using ACRLoginPortal.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace ACRLoginPortal.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class RegistrationController : Controller
    {
        DataProtector dp;
        private readonly IOptions<OktaConfig> _Config;

        public RegistrationController(IOptions<OktaConfig> config, IDataProtectionProvider provider)
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

            if (!Uri.IsWellFormedUriString(Request.Query["path"].ToString(), UriKind.Absolute))
            {
                TempData["Message"] = "Sorry something went wrong, please try again!"; //"path parameter is not a valid Url, please initiate the request from the application.";
                return View("~/Views/Error.cshtml");
            }

            string encryptedStr = dp.ProtectStr(Request.Query["path"].ToString());

            return RedirectToAction("SignUp", "Registration", new { key = encryptedStr });
        }

        [HttpGet]
        [Route("Account/SignUp")]
        public IActionResult SignUp()
        {
            RegistrationModel register = new RegistrationModel() { Key = Request.Query["key"] };

            return View($"~/Views/Registration/SignUp.cshtml", register);
        }

        [HttpPost]
        [Route("Account/SignUp")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegistrationModel register)
        {
            if(ModelState.IsValid)
            {
                register.profile.login = register.profile.email;
                OktaHelper oktaHelper = new OktaHelper(_Config);
                var result = await oktaHelper.CreateUser(register);

                if(result.IsSuccessStatusCode)
                {
                    var user = oktaHelper.GetJsonObject(result);
                    var activationLink = $"{Request.Scheme}://{Request.Host}/ACRLoginPortal/Account/Activate?userID={user["id"].Value<string>()}&key={register.Key}";
                    //Body = $"<p>Hi {register.profile.firstName},<br/><br/>Welcome to American College of Radiology!<br/><br/>To verify your email address and activate your account,please click the following link: <br/> <a href = \"{Request.Scheme}://{Request.Host}/Account/Activate?userID={user["id"].Value<string>()}&key={register.Key}\">Activate Account</a></p>",
                    EmailModel email = new EmailModel
                    {
                        Body = EmailService.AccountActivationEmailBody(register.profile.firstName, activationLink),
                        Subject = $"Activation URL",
                        IsHtml = true,
                        To = new List<string>() { user["profile"]["email"].Value<string>() },
                        SMTPServer = _Config.Value.SMTP_Server,
                        SMTPPort = Convert.ToInt32(_Config.Value.SMTP_Port),
                        SMTPUser = _Config.Value.SMTP_Username,
                        SMTPPassword = _Config.Value.SMTP_Password,
                        EnableSsl = _Config.Value.SMTP_EnableSSl
                    };

                    EmailService.SendEmail(email);

                    //TempData["Message"] = "Account created successfully. The account activation link has been sent to your email address. Please check the inbox.";

                    return RedirectToAction("SignUpMessage", "Registration", new { key = register.Key, status = "success" });
                }
                else
                {
                    var error = oktaHelper.GetJsonObject(result);
                    ModelState.AddModelError("Error", error["errorCauses"][0]["errorSummary"].Value<string>());

                    return View($"~/Views/Registration/SignUp.cshtml", register);
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Sorry, we found some errors.Please review the form and make corrections.");
                return View($"~/Views/Registration/SignUp.cshtml", register);
            }
        }

        [HttpGet]
        [Route("Account/SignUpMessage")]
        public IActionResult SignUpMessage()
        {
            ViewBag.Key = Request.Query["key"];

            if (Request.Query["status"].ToString().ToUpper() == "SUCCESS")
                ViewData["Message"] = "Account created successfully. The account activation link has been sent to your email address. Please check the inbox.";

            return View($"~/Views/Registration/SignUpMessage.cshtml");
        }

        [HttpGet]
        [Route("Account/Activate")]
        public async Task<IActionResult> Activate(string userID)
        {
            //ViewBag.Key = Request.Query["key"];

            OktaHelper oktaHelper = new OktaHelper(_Config);
            var result = await oktaHelper.ActivateUser(userID);

            if (result.IsSuccessStatusCode || result.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                //TempData["Message"] = "The account verified successfully. Please login using the below link.";
                return RedirectToAction("ActivateMessage", "Registration", new { key = Request.Query["key"], status = "success" });
            }
            else
            {
                //Need to improve
                //TempData["Message"] = "The account verification failed.";
                return RedirectToAction("ActivateMessage", "Registration", new { key = Request.Query["key"], status = "fail" });
            }
        }

        [HttpGet]
        [Route("Account/ActivateMessage")]
        public IActionResult ActivateMessage()
        {
            if (Request.Query["status"].ToString().ToUpper() == "SUCCESS")
                ViewData["Message"] = "The account verified successfully. Please login using the below link";

            if (Request.Query["status"].ToString().ToUpper() == "FAIL")
                ViewData["Message"] = "The account verification failed";

            ViewBag.Key = Request.Query["key"];
            return View($"~/Views/Registration/ActivateMessage.cshtml");
        }
    }
}