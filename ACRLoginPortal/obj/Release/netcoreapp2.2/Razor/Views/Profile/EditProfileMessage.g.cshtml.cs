#pragma checksum "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Profile\EditProfileMessage.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "12b9a8306dca6423b449a64d384fae889998e254"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Profile_EditProfileMessage), @"mvc.1.0.view", @"/Views/Profile/EditProfileMessage.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Profile/EditProfileMessage.cshtml", typeof(AspNetCore.Views_Profile_EditProfileMessage))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"12b9a8306dca6423b449a64d384fae889998e254", @"/Views/Profile/EditProfileMessage.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f8105c1e86fa7e089b99ab7097cd5ebf112d3907", @"/Views/_ViewImports.cshtml")]
    public class Views_Profile_EditProfileMessage : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Profile\EditProfileMessage.cshtml"
  
    ViewData["Title"] = "EditProfile";

#line default
#line hidden
            BeginContext(49, 87, true);
            WriteLiteral("\r\n<h4 class=\"credential-title\">Edit Profile</h4>\r\n<div class=\"credential-form-group\">\r\n");
            EndContext();
#line 8 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Profile\EditProfileMessage.cshtml"
     if (ViewData["Message"] != null)
    {

#line default
#line hidden
            BeginContext(182, 36, true);
            WriteLiteral("        <label for=\"\">\r\n            ");
            EndContext();
            BeginContext(219, 30, false);
#line 11 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Profile\EditProfileMessage.cshtml"
       Write(ViewData["Message"].ToString());

#line default
#line hidden
            EndContext();
            BeginContext(249, 1, true);
            WriteLiteral(" ");
            EndContext();
#line 11 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Profile\EditProfileMessage.cshtml"
                                              TempData.Remove("Message");

#line default
#line hidden
            BeginContext(282, 18, true);
            WriteLiteral("        </label>\r\n");
            EndContext();
#line 13 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Profile\EditProfileMessage.cshtml"
    }

#line default
#line hidden
            BeginContext(307, 41, true);
            WriteLiteral("</div>\r\n<div class=\"space-between\">\r\n    ");
            EndContext();
            BeginContext(349, 89, false);
#line 16 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Profile\EditProfileMessage.cshtml"
Write(Html.ActionLink("Back to Application", "BackToApp", "Profile", new { key = ViewBag.Key }));

#line default
#line hidden
            EndContext();
            BeginContext(438, 12, true);
            WriteLiteral("\r\n</div>\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591