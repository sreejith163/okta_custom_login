using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace okta_custom_login.Helpers
{
    public class EmailService
    {
        public static void SendEmail(EmailModel email)
        {
            MailMessage mail = new MailMessage();

            //Attachment attachment = new Attachment()
            
            mail.To.Add(email.To.Aggregate((i, j) => i + "," + j));
            if (email.CC != null)
                mail.CC.Add(email.CC.Aggregate((i, j) => i + "," + j));
            if (email.BCC != null)
                mail.Bcc.Add(email.BCC.Aggregate((i, j) => i + "," + j));
            mail.Subject = email.Subject;
            mail.IsBodyHtml = email.IsHtml;
            mail.From = new MailAddress(email.SMTPUser);
            mail.Body = email.Body;

            using (var client = new SmtpClient(email.SMTPServer))
            {
                client.Port = email.SMTPPort;
                client.Credentials = new NetworkCredential(email.SMTPUser, email.SMTPPassword);
                client.EnableSsl = true;
                client.Send(mail);
            }
        }

        public static string AccountActivationEmailBody(string name, string activationLink)
        {
            string body = $@"<div style=""background-color: rgb(250,250,250);margin: 0;"">
  <table align=""left"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""font-family: &quot;Proxima Nova&quot; , &quot;Century Gothic&quot; , Arial , Verdana , sans-serif;font-size: 14.0px;color: rgb(94,94,94);width: 98.0%;max-width: 600.0px;float: none;margin: 0 auto;"">
    <tbody>
      <tr>
        <td><table bgcolor=""#ffffff"" cellpadding=""0"" style=""width: 100.0%;line-height: 20.0px;padding: 32.0px;border: 1.0px solid;border-color: rgb(240,240,240);"">
            <tbody>
              <tr>
                <td style=""padding-top: 24.0px;vertical-align: bottom;"">
                  Hi {name},                     </td></tr>
              <tr>
                <td style=""padding-top: 24.0px;"">
                  Welcome to American College of Radiology!                     </td></tr>
              <tr>
                <td style=""padding-top: 24.0px;"">
                  To verify your email address and activate your
                  account, <br />                         please click
                  the following link:                     </td></tr>
              <tr>
                <td align=""center""><table border=""0"" cellpadding=""0"" cellspacing=""0"">
                    <tbody>
                      <tr>
                        <td align=""center""
                            style=""height: 39.0px;padding-top: 24.0px;padding-bottom: 8.0px;""><a
                            href=""{activationLink}""
                            id=""registration-activation-link""
                              style=""text-decoration: none;""><span
                              style=""padding: 9.0px 32.0px 7.0px 31.0px;border: 1.0px solid;text-align: center;cursor: pointer;color: rgb(255,255,255);border-radius: 3.0px;background-color: rgb(68,188,152);border-color: rgb(50,140,113) rgb(50,140,113) rgb(47,133,107);box-shadow: rgb(216,216,216) 0 1.0px 0;"">Activate Account</span></a></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></div>";

            return body;
        }

        public static string PasswordResetEmailBody(string email, string resetPasswordLink)
        {
            string body = $@"<div style=""background-color: rgb(250,250,250);margin: 0;"">
  <table align=""left"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""font-family: &quot;Proxima Nova&quot; , &quot;Century Gothic&quot; , Arial , Verdana , sans-serif;font-size: 14.0px;color: rgb(94,94,94);width: 98.0%;max-width: 600.0px;float: none;margin: 0 auto;"">
    <tbody>
      <tr>
        <td><table bgcolor=""#ffffff"" cellpadding=""0"" style=""width: 100.0%;line-height: 20.0px;padding: 32.0px;border: 1.0px solid;border-color: rgb(240,240,240);"">
            <tbody>
              <tr>
                <td
                  style=""color: rgb(94,94,94);font-size: 22.0px;line-height: 22.0px;"">
                  ACR - Account Password Reset Requested                     </td></tr>
              <tr>
                <td style=""padding-top: 24.0px;"">
                  A password reset request was made for your ACR
                  account. If you did not make this request, please
                  contact your system administrator immediately.                     </td></tr>
              <tr>
                <td style=""padding-top: 24.0px;"">
                  Click this link to reset the password for your
                  username, {email}:                     </td></tr>
              <tr>
                <td align=""center""><table border=""0"" cellpadding=""0"" cellspacing=""0"">
                    <tbody>
                      <tr>
                        <td align=""center""
                            style=""height: 32.0px;padding-top: 24.0px;padding-bottom: 8.0px;""><a
                            href=""{resetPasswordLink}""
                            id=""reset-password-link""
                              style=""text-decoration: none;""><span
                              style=""padding: 9.0px 32.0px 7.0px 31.0px;border: 1.0px solid;text-align: center;cursor: pointer;color: rgb(255,255,255);border-radius: 3.0px;background-color: rgb(68,188,152);border-color: rgb(50,140,113) rgb(50,140,113) rgb(47,133,107);box-shadow: 0 1.0px 0 rgb(216,216,216);"">Reset Password</span></a></td></tr>
                      </tbody></table></td></tr>
              </tbody></table></div>";

            return body;
        }
    }
    public class EmailModel
    {
        public List<string> To { get; set; }
        public List<string> CC { get; set; }
        public List<string> BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string SMTPUser { get; set; }
        public string SMTPPassword { get; set; }
    }
}
