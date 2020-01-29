using System.Web;
using System.Web.Mvc;

namespace KJCFRubberRoller
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            // Require all access to login
            filters.Add(new AuthorizeAttribute());
        }
    }
}
