using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClientSystem.Startup))]
namespace ClientSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
