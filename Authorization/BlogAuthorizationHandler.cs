using EthanBlog.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace EthanBlog.Authorization
{
    public class BlogAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Blog>
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;
        public BlogAuthorizationHandler(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Blog resource)
        {
            var applicationUser = await userManager.GetUserAsync(context.User);

            if((requirement.Name == Operations.Update.Name || requirement.Name == Operations.Delete.Name) && applicationUser == resource.Creator) 
            {
                context.Succeed(requirement);
            }
        }
    }
}
