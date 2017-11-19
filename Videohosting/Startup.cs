using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Videohosting.Startup))]
namespace Videohosting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
