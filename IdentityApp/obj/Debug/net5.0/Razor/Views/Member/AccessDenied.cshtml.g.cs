#pragma checksum "C:\Users\maniz\OneDrive\Desktop\Udemy\IdentityApp\IdentityApp\Views\Member\AccessDenied.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "cc15e920d3e4572bef093eded6cabef9e5af33ed"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(IdentityApp.Pages.Member.Views_Member_AccessDenied), @"mvc.1.0.view", @"/Views/Member/AccessDenied.cshtml")]
namespace IdentityApp.Pages.Member
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\maniz\OneDrive\Desktop\Udemy\IdentityApp\IdentityApp\Views\_ViewImports.cshtml"
using IdentityApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\maniz\OneDrive\Desktop\Udemy\IdentityApp\IdentityApp\Views\_ViewImports.cshtml"
using IdentityApp.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\maniz\OneDrive\Desktop\Udemy\IdentityApp\IdentityApp\Views\_ViewImports.cshtml"
using IdentityApp.ViewModels;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cc15e920d3e4572bef093eded6cabef9e5af33ed", @"/Views/Member/AccessDenied.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e37afcad50c09c59b04dd95b15baec80f3385a93", @"/Views/_ViewImports.cshtml")]
    public class Views_Member_AccessDenied : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\maniz\OneDrive\Desktop\Udemy\IdentityApp\IdentityApp\Views\Member\AccessDenied.cshtml"
  
    ViewData["Title"] = "AccessDenied";
    Layout = "~/Views/Member/_MemberLayout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"alert alert-danger\">\r\n\r\n    <h4>");
#nullable restore
#line 9 "C:\Users\maniz\OneDrive\Desktop\Udemy\IdentityApp\IdentityApp\Views\Member\AccessDenied.cshtml"
   Write(ViewBag.message);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n</div>\r\n\r\n");
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
