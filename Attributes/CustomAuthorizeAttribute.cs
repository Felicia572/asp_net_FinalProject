﻿using System.Web;
using System.Web.Mvc;

namespace Asp_Net_FinalProject.Attributes
{
    public class CustomAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        private readonly string _staticUser;
        public CustomAuthorizeAttribute()
        {
            _staticUser = "admin@example.com";
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            string dynamicUser = httpContext.User.Identity.Name;

            // Allow access if the user is the static user (admin@example.com) or if the user is authenticated
            if (dynamicUser == _staticUser || httpContext.Request.IsAuthenticated)
            {
                return true;
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Errors/AccessDenied.cshtml"
            };
        }
    }
}
