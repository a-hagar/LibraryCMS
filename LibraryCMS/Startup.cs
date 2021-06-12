using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LibraryCMS.Startup))]
namespace LibraryCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
