#pragma checksum "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8b79206a99eaf9171145dc85de14868b9b8c1b4b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_ResendActivationLink), @"mvc.1.0.view", @"/Views/Account/ResendActivationLink.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Account/ResendActivationLink.cshtml", typeof(AspNetCore.Views_Account_ResendActivationLink))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\_ViewImports.cshtml"
using ACRLoginPortal;

#line default
#line hidden
#line 2 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\_ViewImports.cshtml"
using ACRLoginPortal.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8b79206a99eaf9171145dc85de14868b9b8c1b4b", @"/Views/Account/ResendActivationLink.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f8105c1e86fa7e089b99ab7097cd5ebf112d3907", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_ResendActivationLink : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ACRLoginPortal.Models.ForgotPasswordModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(50, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
  
    ViewData["Title"] = "ResendActivationLink";

#line default
#line hidden
            BeginContext(108, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 7 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
 using (Html.BeginForm(FormMethod.Post))
{
    

#line default
#line hidden
            BeginContext(160, 23, false);
#line 9 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
Write(Html.AntiForgeryToken());

#line default
#line hidden
            EndContext();
            BeginContext(185, 55, true);
            WriteLiteral("    <h4 class=\"credential-title\">Forgot Password</h4>\r\n");
            EndContext();
            BeginContext(245, 28, false);
#line 11 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
Write(Html.ValidationSummary(true));

#line default
#line hidden
            EndContext();
            BeginContext(275, 41, true);
            WriteLiteral("    <div class=\"credential-form-group\">\r\n");
            EndContext();
#line 13 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
         if (@Html.ValidationMessage("Error") != null)
        {
            

#line default
#line hidden
            BeginContext(396, 31, false);
#line 15 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
       Write(Html.ValidationMessage("Error"));

#line default
#line hidden
            EndContext();
#line 15 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
                                            
        }

#line default
#line hidden
            BeginContext(440, 53, true);
            WriteLiteral("    </div>\r\n    <div class=\"credential-form-group\">\r\n");
            EndContext();
#line 19 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
         if (TempData["Message"] != null)
        {

#line default
#line hidden
            BeginContext(547, 61, true);
            WriteLiteral("            <div onclick=\"this.remove()\" class=\"text-danger\">");
            EndContext();
            BeginContext(609, 30, false);
#line 21 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
                                                        Write(TempData["Message"].ToString());

#line default
#line hidden
            EndContext();
            BeginContext(639, 1, true);
            WriteLiteral(" ");
            EndContext();
#line 21 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
                                                                                               TempData.Remove("Message");

#line default
#line hidden
            BeginContext(670, 8, true);
            WriteLiteral("</div>\r\n");
            EndContext();
#line 22 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
        }

#line default
#line hidden
            BeginContext(689, 98, true);
            WriteLiteral("    </div>\r\n    <div class=\"credential-form-group\">\r\n        <label for=\"\">Email</label>\r\n        ");
            EndContext();
            BeginContext(788, 119, false);
#line 26 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
   Write(Html.TextBoxFor(model => model.Email, new { @class = "form-input", placeholder = "Enter email address", id = "email" }));

#line default
#line hidden
            EndContext();
            BeginContext(907, 10, true);
            WriteLiteral("\r\n        ");
            EndContext();
            BeginContext(918, 89, false);
#line 27 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
   Write(Html.ValidationMessageFor(model => model.Email, "", new { id = "emailErrorMessageSpan" }));

#line default
#line hidden
            EndContext();
            BeginContext(1007, 55, true);
            WriteLiteral("\r\n    </div>\r\n    <div class=\"space-between\">\r\n        ");
            EndContext();
            BeginContext(1063, 71, false);
#line 30 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
   Write(Html.ActionLink("Sign In", "Login", "Account", new { key = Model.Key }));

#line default
#line hidden
            EndContext();
            BeginContext(1134, 10, true);
            WriteLiteral("\r\n        ");
            EndContext();
            BeginContext(1145, 34, false);
#line 31 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"
   Write(Html.HiddenFor(model => model.Key));

#line default
#line hidden
            EndContext();
            BeginContext(1179, 112, true);
            WriteLiteral("\r\n        <input class=\"credential-btn\" type=\"submit\" value=\"Resend Mail\" id=\"resendMailSubmit\" />\r\n    </div>\r\n");
            EndContext();
#line 34 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Account\ResendActivationLink.cshtml"





}

#line default
#line hidden
            BeginContext(1304, 185, true);
            WriteLiteral("<script src=\"https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js\"></script>\r\n\r\n<script>\r\n\r\n    $(\'#resendMailSubmit\').click(function () {\r\n\r\n        var regex = /^([\\w-\\.]+");
            EndContext();
            BeginContext(1490, 567, true);
            WriteLiteral(@"@([\w-]+\.)+[\w-]{2,4})?$/;
        if (!regex.test($('#email').val())) {
            $('#emailErrorMessageSpan').attr('class', 'field-validation-error');
            $('#emailErrorMessageSpan').text('Enter a valid email');
            $('#emailErrorMessageSpan').show();
            $('#email').focus();
            return false;
        } else {
            return true;
        }
    });

    $('#email').on(""input"", function () {
        $('#emailErrorMessageSpan').text('');
        $('#emailErrorMessageSpan').hide();
    });

</script>



");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ACRLoginPortal.Models.ForgotPasswordModel> Html { get; private set; }
    }
}
#pragma warning restore 1591