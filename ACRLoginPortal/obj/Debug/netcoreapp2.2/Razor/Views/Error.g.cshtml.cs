#pragma checksum "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Error.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9273bea30b8415522368d88b588224f27ba8e997"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Error), @"mvc.1.0.view", @"/Views/Error.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Error.cshtml", typeof(AspNetCore.Views_Error))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9273bea30b8415522368d88b588224f27ba8e997", @"/Views/Error.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f8105c1e86fa7e089b99ab7097cd5ebf112d3907", @"/Views/_ViewImports.cshtml")]
    public class Views_Error : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Error.cshtml"
  
    ViewData["Title"] = "Error";

#line default
#line hidden
            BeginContext(43, 80, true);
            WriteLiteral("\r\n<h4 class=\"credential-title\">Error</h4>\r\n<div class=\"credential-form-group\">\r\n");
            EndContext();
#line 8 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Error.cshtml"
     if (TempData["Message"] != null)
    {

#line default
#line hidden
            BeginContext(169, 36, true);
            WriteLiteral("        <label for=\"\">\r\n            ");
            EndContext();
            BeginContext(206, 30, false);
#line 11 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Error.cshtml"
       Write(TempData["Message"].ToString());

#line default
#line hidden
            EndContext();
            BeginContext(236, 1, true);
            WriteLiteral(" ");
            EndContext();
#line 11 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Error.cshtml"
                                              TempData.Remove("Message");

#line default
#line hidden
            BeginContext(269, 18, true);
            WriteLiteral("        </label>\r\n");
            EndContext();
#line 13 "E:\ACR\ACR Assist Projects\okta_custom_login\ACRLoginPortal\Views\Error.cshtml"
    }

#line default
#line hidden
            BeginContext(294, 8, true);
            WriteLiteral("</div>\r\n");
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