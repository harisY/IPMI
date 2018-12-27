using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IPMI.Startup))]
namespace IPMI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
