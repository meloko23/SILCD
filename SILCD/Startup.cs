using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SILCD.Startup))]
namespace SILCD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
