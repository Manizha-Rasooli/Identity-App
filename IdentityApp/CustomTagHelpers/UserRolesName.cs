using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.CustomTagHelpers
{
    [HtmlTargetElement("td",Attributes="user-roles")]
    public class UserRolesName:TagHelper
    {
        public UserManager<AppUser> userManager { get; set; }

        public UserRolesName(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        [HtmlAttributeName("user-roles")]
        public string UserId { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) // this method set string or list of string to a html tag
        {
            AppUser user = await userManager.FindByIdAsync(UserId);
            IList<string> userRoles=  await userManager.GetRolesAsync(user);

            string html = string.Empty;

            userRoles.ToList().ForEach(x => {
                html += $"<span class='badge badge-info'>{x}</span>";
            });
            output.Content.SetHtmlContent(html);
        }

    }

    
}
