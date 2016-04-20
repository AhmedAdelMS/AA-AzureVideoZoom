using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HMLS.Web.Startup))]
namespace HMLS.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
