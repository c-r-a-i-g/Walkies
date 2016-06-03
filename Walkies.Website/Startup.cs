using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Walkies.Website.Startup))]
namespace Walkies.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
