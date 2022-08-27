using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Teman_ApotikProj.Startup))]
namespace Teman_ApotikProj
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
