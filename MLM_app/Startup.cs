using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MLM_app.Startup))]
namespace MLM_app
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
