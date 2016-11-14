using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharePointAddIn_ProductManagementWeb.Helpers
{
    public static class SharePointHelper
    {
        public static User GetUserProfile(HttpContextBase httpContext)
        {
            User spUser = null;

            var spContext = SharePointContextProvider.Current.GetSharePointContext(httpContext);

            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    spUser = clientContext.Web.CurrentUser;

                    clientContext.Load(spUser, user => user.Title);

                    clientContext.ExecuteQuery();

                    return spUser;
                }

                return null;
            }
        }


        public static string GetUserDisplayName(HttpContextBase httpContext)
        {
            User spUser = null;

            var spContext = SharePointContextProvider.Current.GetSharePointContext(httpContext);

            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    spUser = clientContext.Web.CurrentUser;

                    clientContext.Load(spUser, user => user.Title);

                    clientContext.ExecuteQuery();

                    return spUser.Title;
                }

                return string.Empty;
            }
        }
    }
}