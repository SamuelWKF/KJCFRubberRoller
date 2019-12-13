using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KJCFRubberRoller.Startup))]
namespace KJCFRubberRoller
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
