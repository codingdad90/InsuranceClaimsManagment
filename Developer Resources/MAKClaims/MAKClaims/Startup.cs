using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MAKClaims.Startup))]
namespace MAKClaims
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
