using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(everything.Startup))]
namespace everything
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
