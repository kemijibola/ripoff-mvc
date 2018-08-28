using System.Web;
using System.Web.Mvc;
using everything.Helpers;

namespace everything
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
            filters.Add(new CustomAuthorizeAttribute());
            //filters.Add(new RequireHttpsAttribute());
        }
    }
}
