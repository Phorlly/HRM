using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HRM.Helpers
{
    public class LowerCaseRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            requestContext.RouteData.Values["controller"] = requestContext.RouteData.Values["controller"].ToString().ToLower();
            requestContext.RouteData.Values["action"] = requestContext.RouteData.Values["action"].ToString().ToLower();

            var handler = new MvcHandler(requestContext);
            return handler;
        }
    }
}