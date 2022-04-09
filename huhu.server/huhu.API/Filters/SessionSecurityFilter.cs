using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace huhu.API.Filters
{
    public class SessionSecurityFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext ActionContext)
        {
            
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}