using okta_custom_login.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace okta_custom_login.Helpers
{
    public class OktaHelper
    {
        private string OktaAuthnURL = "/api/v1/authn";
        //private string OktaCreateSesssionURL = "/api/v1/sessions?additionalFields=cookieToken";

        HttpClient client = new HttpClient();
        private IOptions<OktaConfig> _Config;

        public OktaHelper(IOptions<OktaConfig> config)
        {
            _Config = config;
        }
        public JObject GetJsonObject(HttpResponseMessage response)
        {
            string strJson = response.Content.ReadAsStringAsync().Result;
            return (JObject)JsonConvert.DeserializeObject(strJson);
        }

        public JArray GetJsonArray(HttpResponseMessage response)
        {
            string strJson = response.Content.ReadAsStringAsync().Result;
            return (JArray)JsonConvert.DeserializeObject(strJson);
        }

        internal async Task<HttpResponseMessage> Login(LoginModel login)
        {
            string url = _Config.Value.Okta_OrgUri + OktaAuthnURL;

            var postValue = new
            {
                username = login.Username,
                password = login.Password,
                options = new
                {
                    multiOptionalFactorEnroll = false,
                    warnBeforePasswordExpired = false
                }
            };
            var data = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postValue)));
            data.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await client.PostAsync(url, data);
            return response;
        }

        internal async Task<HttpResponseMessage> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            string url = $"{_Config.Value.Okta_OrgUri}/api/v1/users/{changePasswordModel.UserId}/credentials/change_password";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "SSWS " + _Config.Value.Okta_APIToken);

            var post_data = new
            {
                oldPassword = new { value = changePasswordModel.OldPassword },
                newPassword = new { value = changePasswordModel.NewPassword }
            };

            var data = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(post_data)));
            data.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await client.PostAsync(url, data);
        }

        internal async Task<HttpResponseMessage> ForgotPassword(string userId, bool SendEmail = false)
        {
            var url = $"{_Config.Value.Okta_OrgUri}/api/v1/users/{userId}/credentials/forgot_password?sendEmail={SendEmail}";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "SSWS " + _Config.Value.Okta_APIToken);
            return await client.PostAsync(url, null);
        }

        internal async Task<HttpResponseMessage> VerifyRecoveryToken(string recoveryToken)
        {
            var url = $"{_Config.Value.Okta_OrgUri}/api/v1/authn/recovery/token";
            var postValue = new
            {
                recoveryToken
            };
            var data = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postValue)));
            data.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return await client.PostAsync(url, data);
        }

        internal async Task<HttpResponseMessage> ResetPassword(ResetPasswordModel resetPassword)
        {
            var url = $"{_Config.Value.Okta_OrgUri}/api/v1/users/{resetPassword.UserId}";
            var postValue = new
            {
                credentials = new
                {
                    password = new
                    {
                        value = resetPassword.Password
                    }
                }
            };
            var data = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postValue)));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "SSWS " + _Config.Value.Okta_APIToken);
            data.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await client.PutAsync(url, data);
        }

        internal async Task<HttpResponseMessage> CreateUser(RegistrationModel register, bool activationStatus = false)
        {
            string url = _Config.Value.Okta_OrgUri + $"/api/v1/users?activate={activationStatus}";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "SSWS " + _Config.Value.Okta_APIToken);
            var postContent = new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json");
            return await client.PostAsync(url, postContent);
        }

        internal async Task<HttpResponseMessage> ActivateUser(string UserID)
        {
            string url = _Config.Value.Okta_OrgUri + $"/api/v1/users/{UserID}/lifecycle/activate?sendEmail=false";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "SSWS " + _Config.Value.Okta_APIToken);
            var data = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(null)));
            data.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await client.PostAsync(url, data);
        }

        internal async Task<HttpResponseMessage> SearchUserByEmail(string email)
        {
            var url = $"{_Config.Value.Okta_OrgUri}/api/v1/users?search=profile.email+eq+\"{HttpUtility.UrlEncode(email)}\"+and+status+eq+\"Active\"";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "SSWS " + _Config.Value.Okta_APIToken);
            return await client.GetAsync(url);
        }

        internal async Task<HttpResponseMessage> ReactivateUser(string userId)
        {
            var url = $"{_Config.Value.Okta_OrgUri}/api/v1/users/{userId}/lifecycle/reactivate?sendEmail=false";

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "SSWS " + _Config.Value.Okta_APIToken);

            return await client.PostAsync(url, null);
        }

        internal async Task<HttpResponseMessage> GetOktaUser(string login)
        {
            string url = $"{_Config.Value.Okta_OrgUri}/api/v1/users/{HttpUtility.UrlEncode(login)}";

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "SSWS " + _Config.Value.Okta_APIToken);
            return await client.GetAsync(url);
        }

        internal async Task<HttpResponseMessage> UpdateUser(OktaUserModel oktaUser)
        {
            string url = _Config.Value.Okta_OrgUri + $"/api/v1/users/{HttpUtility.UrlEncode(oktaUser.profile.login)}";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "SSWS " + _Config.Value.Okta_APIToken);
            var postContent = new StringContent(JsonConvert.SerializeObject(oktaUser), Encoding.UTF8, "application/json");
            return await client.PostAsync(url, postContent);
        }

        internal async Task<HttpResponseMessage> RevokeToken(string accessToken)
        {
            string url = $"{_Config.Value.Okta_OrgUri}/oauth2/{_Config.Value.Okta_AuthServer}/v1/revoke?client_id={_Config.Value.Okta_ClientId}&client_secret={_Config.Value.Okta_ClientSecret}&token={accessToken}&token_type_hint=access_token";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            var postContent = new StringContent("", Encoding.UTF8, "application/x-www-form-urlencoded");
            return await client.PostAsync(url, postContent);
        }
    }
}
