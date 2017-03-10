using System;
using System.Web.Mvc;

namespace SharePointAddIn_ProductManagementWeb
{
    /// <summary>
    /// SharePoint action filter attribute.
    /// </summary>
    public class SharePointContextFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            switch (SharePointContextProvider.CheckRedirectionStatus(filterContext.HttpContext, out Uri redirectUrl))
            {
                case RedirectionStatus.Ok:
                    return;
                case RedirectionStatus.ShouldRedirect:
                    filterContext.Result = new RedirectResult(redirectUrl.AbsoluteUri);
                    break;
                case RedirectionStatus.CanNotRedirect:
                    filterContext.Result = new ViewResult { ViewName = "Error" };
                    break;
                default:
                    filterContext.Result = new ViewResult { ViewName = "Error" };
                    break;
            }
        }
    }
}
